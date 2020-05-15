using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	public enum eBounds
	{
		NONE,
		X,
		Y
	}

	[SerializeField] [Range(10, 100)] int m_size = 10;
	[SerializeField] [Range(0, 5)] float m_cellSize = 1;
	[SerializeField] [Range(0, 1)] float m_diffuse = 1;
	[SerializeField] [Range(0, 0.1f)] float m_viscosity = 1;
	[SerializeField] [Range(0, 1)] float m_reduce = 1;
	[SerializeField] [Range(1, 20)] int m_iterations = 1;
	[SerializeField] [Range(30, 90)] int m_fps = 60;
	[SerializeField] FluidCell m_fluidCell = null;

	int GetIndex(int x, int y) { return x + (y * (m_size + 2)); }
	FluidGrid m_grid;

	float timeAccumulator { get; set; } = 0.0f;

	private void Start()
	{
		// cells
		int n = (m_size + 2) * (m_size + 2);
		m_grid.Create(n);

		// create grid cell objects
		float xo = transform.position.x - ((m_size * 0.5f) * m_cellSize);
		float yo = transform.position.y - ((m_size * 0.5f) * m_cellSize);
		float zo = transform.position.z;
		Vector3 position = Vector3.zero;
		for (int x = 0; x <= m_size + 1; x++)
		{
			for (int y = 0; y <= m_size + 1; y++)
			{
				position.x = xo + (x * m_cellSize);
				position.y = yo + (y * m_cellSize);
				position.z = zo;

				FluidCell fcv = Instantiate(m_fluidCell, position, Quaternion.identity, transform);
				fcv.x = x;
				fcv.y = y;
				fcv.size = Vector3.one * m_cellSize;
				m_grid.cells[GetIndex(x, y)] = fcv;
			}
		}
	}

	private void Update()
	{
		float fixedTimeStep = 1.0f / m_fps;
		timeAccumulator = timeAccumulator + Time.deltaTime;
		while (timeAccumulator > fixedTimeStep)
		{
			// VELOCITY
			FluidGrid.Swap(ref m_grid.prev_vx, ref m_grid.vx);
			FluidGrid.Swap(ref m_grid.prev_vy, ref m_grid.vy);

			Diffuse(ref m_grid.vx, ref m_grid.prev_vx, m_viscosity, fixedTimeStep, m_iterations, eBounds.X);
			Diffuse(ref m_grid.vy, ref m_grid.prev_vy, m_viscosity, fixedTimeStep, m_iterations, eBounds.Y);
			Project(ref m_grid.vx, ref m_grid.vy, ref m_grid.prev_vx, ref m_grid.prev_vy, m_iterations);

			FluidGrid.Swap(ref m_grid.prev_vx, ref m_grid.vx);
			FluidGrid.Swap(ref m_grid.prev_vy, ref m_grid.vy);

			Advect(ref m_grid.vx, ref m_grid.prev_vx, ref m_grid.prev_vx, ref m_grid.prev_vy, fixedTimeStep, eBounds.X);
			Advect(ref m_grid.vy, ref m_grid.prev_vy, ref m_grid.prev_vx, ref m_grid.prev_vy, fixedTimeStep, eBounds.Y);

			Project(ref m_grid.vx, ref m_grid.vy, ref m_grid.prev_vx, ref m_grid.prev_vy, m_iterations);

			// DENSITY
			FluidGrid.Swap(ref m_grid.prev_density, ref m_grid.density);
			Diffuse(ref m_grid.density, ref m_grid.prev_density, m_diffuse, fixedTimeStep, m_iterations, eBounds.NONE);
			FluidGrid.Swap(ref m_grid.prev_density, ref m_grid.density);
			Advect(ref m_grid.density, ref m_grid.prev_density, ref m_grid.vx, ref m_grid.vy, fixedTimeStep, eBounds.NONE);

			timeAccumulator = timeAccumulator - fixedTimeStep;
		}

		// reduce density
		for (int i = 0; i < m_grid.prev_density.Length; i++)
		{
			m_grid.density[i] = m_grid.density[i] * Mathf.Pow(m_reduce, Time.deltaTime);
		}

		// set grid cell object density
		for (int i = 0; i < m_grid.cells.Length; i++)
		{
			m_grid.cells[i].density = m_grid.density[i];
			m_grid.cells[i].velocity = new Vector2(m_grid.vx[i], m_grid.vy[i]);
		}
	}

	public void Inject(Ray ray, float density, Vector2 velocity)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			FluidCell fcv = raycastHit.collider.gameObject.GetComponent<FluidCell>();
			if (fcv != null)
			{
				int index = GetIndex(fcv.x, fcv.y);
				if (density != 0)
				{
					m_grid.density[index] += density;
				}

				if (velocity.sqrMagnitude > 0)
				{
					m_grid.vx[index] = velocity.x;
					m_grid.vy[index] = velocity.y;
				}
			}
		}
	}

	public void Diffuse(ref float[] current, ref float[] previous, float diffuse, float dt, int iterations, eBounds bounds)
	{
		float a = dt * diffuse * m_size * m_size;
		float c = (1 + 4 * a);

		SolveLinear(ref current, ref previous, a, c, iterations, bounds);
	}

	public void Advect(ref float[] current, ref float[] previous, ref float[] vx, ref float[] vy, float dt, eBounds bounds)
	{
		float dt0 = dt * m_size;
		for (int i = 1; i <= m_size; i++)
		{
			for (int j = 1; j <= m_size; j++)
			{
				float x = i - (dt0 * vx[GetIndex(i, j)]);
				float y = j - (dt0 * vy[GetIndex(i, j)]);
				x = Mathf.Clamp(x, 0.5f, m_size + 0.5f);
				y = Mathf.Clamp(y, 0.5f, m_size + 0.5f);

				int i0 = (int)x;
				int i1 = i0 + 1;
				int j0 = (int)y;
				int j1 = j0 + 1;
				float s1 = x - i0;
				float s0 = 1 - s1;
				float t1 = y - j0;
				float t0 = 1 - t1;

				current[GetIndex(i, j)] = s0 * ((t0 * previous[GetIndex(i0, j0)])
											 +  (t1 * previous[GetIndex(i0, j1)])) +
										  s1 * ((t0 * previous[GetIndex(i1, j0)]) +
											 +  (t1 * previous[GetIndex(i1, j1)]));
			}
		}

		ApplyBounds(ref current, bounds);
	}

	void Project(ref float[] vx, ref float[] vy, ref float[] p, ref float[] div, int iterations)
	{
		for (int i = 1; i <= m_size; i++)
		{
			for (int j = 1; j <= m_size; j++)
			{
				div[GetIndex(i, j)] = -0.5f * (vx[GetIndex(i + 1, j)] - vx[GetIndex(i - 1, j)] +
											   vy[GetIndex(i, j + 1)] - vy[GetIndex(i, j - 1)])
											   / m_size;
				p[GetIndex(i, j)] = 0;
			}
		}

		ApplyBounds(ref div, eBounds.NONE);
		ApplyBounds(ref p, eBounds.NONE);

		SolveLinear(ref p, ref div, 1, 4, iterations, eBounds.NONE);

		for (int i = 1; i <= m_size; i++)
		{
			for (int j = 1; j <= m_size; j++)
			{
				vx[GetIndex(i, j)] -= 0.5f * m_size * (p[GetIndex(i + 1, j)] - p[GetIndex(i - 1, j)]);
				vy[GetIndex(i, j)] -= 0.5f * m_size * (p[GetIndex(i, j + 1)] - p[GetIndex(i, j - 1)]);
			}
		}

		ApplyBounds(ref vx, eBounds.X);
		ApplyBounds(ref vy, eBounds.Y);
	}

	public void SolveLinear(ref float[] current, ref float[] previous, float a, float c, int iterations, eBounds bounds)
	{
		float rc = 1.0f / c;
		for (int iter = 0; iter < iterations; iter++)
		{
			for (int i = 1; i <= m_size; i++)
			{
				for (int j = 1; j <= m_size; j++)
				{
					current[GetIndex(i, j)] = (previous[GetIndex(i, j)] +
								a * (current[GetIndex(i - 1, j)] +
									 current[GetIndex(i + 1, j)] +
									 current[GetIndex(i, j - 1)] +
									 current[GetIndex(i, j + 1)])) * rc;
				}
			}
			ApplyBounds(ref current, bounds);
		}
	}

	public void ApplyBounds(ref float[] f, eBounds axis)
	{
		for (int i = 1; i <= m_size; i++)
		{
			f[GetIndex(0, i)] = (axis == eBounds.X) ? -f[GetIndex(1, i)] : f[GetIndex(1, i)];
			f[GetIndex(m_size + 1, i)] = (axis == eBounds.X) ? -f[GetIndex(m_size, i)] : f[GetIndex(m_size, i)];
			f[GetIndex(i, 0)] = (axis == eBounds.Y) ? -f[GetIndex(i, 1)] : f[GetIndex(i, 1)];
			f[GetIndex(i, m_size + 1)] = (axis == eBounds.Y) ? -f[GetIndex(i, m_size)] : f[GetIndex(i, m_size)];
		}
		
		f[GetIndex(0, 0)] = 0.5f * (f[GetIndex(1, 0)] + f[GetIndex(0, 1)]);
		f[GetIndex(0, m_size + 1)] = 0.5f * (f[GetIndex(1, m_size + 1)] + f[GetIndex(0, m_size)]);
		f[GetIndex(m_size + 1, 0)] = 0.5f * (f[GetIndex(m_size, 0)] + f[GetIndex(m_size + 1, 1)]);
		f[GetIndex(m_size + 1, m_size + 1)] = 0.5f * (f[GetIndex(m_size, m_size + 1)] + f[GetIndex(m_size + 1, m_size)]);
	}
}

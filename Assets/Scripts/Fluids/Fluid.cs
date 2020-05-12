using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	[SerializeField] [Range(10, 40)] int m_size = 10;
	[SerializeField] [Range(1, 5)] float m_cellSize = 1;
	[SerializeField] [Range(0, 5)] float m_diffusion = 1;
	[SerializeField] [Range(0, 5)] float m_viscosity = 1;
	[SerializeField] FluidCellVisual m_fluidCellVisual = null;

	int GetIndex(int x, int y) { return x + (y * m_size); }
	static int GetIndex(int index, int xo, int yo, int width) { return index + (xo + (yo * width)); }

	float[] m_density;
	FluidCell[] m_cellsA;
	FluidCell[] m_cellsB;
	FluidCellVisual[] m_cellVisuals;

	private void Start()
	{
		// cells
		m_cellsA = new FluidCell[m_size * m_size];
		m_cellsB = new FluidCell[m_size * m_size];

		// visuals
		m_cellVisuals = new FluidCellVisual[m_size * m_size];
		float xo = transform.position.x - ((m_size * 0.5f) * m_cellSize);
		float yo = transform.position.y - ((m_size * 0.5f) * m_cellSize);
		float zo = transform.position.z;
		Vector3 position = Vector3.zero;
		for (int x = 0; x < m_size; x++)
		{
			for (int y = 0; y < m_size; y++)
			{
				position.x = xo + (x * m_cellSize * 1.1f);
				position.y = yo + (y * m_cellSize * 1.1f);
				position.z = zo;

				FluidCellVisual fcv = Instantiate(m_fluidCellVisual, position, Quaternion.identity, transform);
				fcv.x = x;
				fcv.y = y;
				m_cellVisuals[GetIndex(x, y)] = fcv;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < m_cellsA.Length; i++)
		{
			m_cellVisuals[i].density = m_cellsA[i].density;
		}
	}

	public void Inject(Ray ray, float density, bool apply)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			FluidCellVisual fcv = raycastHit.collider.gameObject.GetComponent<FluidCellVisual>();
			if (fcv != null)
			{
				if (apply)
				{
					AddDensity(fcv.x, fcv.y, density, ref m_cellsA);
				}
			}
		}
	}

	void AddDensity(int x, int y, float density, ref FluidCell[] cells)
	{
		int index = GetIndex(x, y);
		cells[index].density = cells[index].density + density;
	}

	void AddVelocity(int x, int y, Vector3 velocity, ref FluidCell[] cells)
	{
		int index = GetIndex(x, y);
		cells[index].velocity = cells[index].velocity + velocity;
	}

	public static void Diffuse(int b, ref FluidCell[] v1, ref FluidCell[] v2, float diffuse, float dt, int iterations, int count)
	{
		float a = dt * diffuse * (count - 2) * (count - 2);
		SolveLinear(b, ref v1, ref v2, a, 1 + 6 * a, iterations, count);
	}
	
	public static void SolveLinear(int b, ref FluidCell[] v1, ref FluidCell[] v2, float a, float c, int iterations, int count)
	{
		float rc = 1.0f / c;
		for (int i = 0; i < iterations; i++)
		{
			for (int index = 0; index < count; i++)
			{
				v2[index].velocity = v1[index].velocity +  
									(v1[GetIndex(index, -1,  0, count)].velocity + 
									 v1[GetIndex(index,  1,  0, count)].velocity + 
									 v1[GetIndex(index,  0, -1, count)].velocity + 
									 v1[GetIndex(index,  0,  1, count)].velocity) * a * rc;    
			}
		}
	}

}

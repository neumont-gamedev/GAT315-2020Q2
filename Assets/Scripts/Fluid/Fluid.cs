using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
	[SerializeField] [Range(10, 40)] int m_size;
	[SerializeField] [Range(1, 5)] float m_cellSize;
	[SerializeField] [Range(0, 5)] float m_diffusion;
	[SerializeField] [Range(0, 5)] float m_viscosity;

	int GetIndex(int x, int y) { return x + (y * m_size); }
	static int GetIndex(int index, int xo, int yo, int width) { return index + (xo + (yo * width)); }

	float[] m_density;
	FluidCell[] m_cells;
	FluidCell[] m_pcells;

	void AddDensity(int x, int y, float density)
	{
		int index = GetIndex(x, y);
		m_cells[index].density = m_cells[index].density + density;
	}

	void AddVelocity(int x, int y, Vector3 velocity)
	{
		int index = GetIndex(x, y);
		m_cells[index].velocity = m_cells[index].velocity + velocity;
	}

	public static void Diffuse(int b, FluidCell[] v1, ref FluidCell[] v2, float diffuse, float dt, int iterations, int count)
	{
		float a = dt * diffuse * (count - 2) * (count - 2);
		SolveLinear(b, v1, ref v2, a, 1 + 6 * a, iterations, count);
	}
	
	public static void SolveLinear(int b, FluidCell[] v1, ref FluidCell[] v2, float a, float c, int iterations, int count)
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

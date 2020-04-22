using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldForce : Force
{
	[SerializeField] VectorFieldData m_vectorField = null;
	[SerializeField] bool m_drawGrid = false;
	[SerializeField] bool m_drawForces = false;

	Vector2[,] m_grid;

	public override void ApplyForce(PhysicsBody body)
	{
		AABB world = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
		Vector2 size = world.size * m_vectorField.gridScale;
		Vector2 gridSize = Vector2.one / m_vectorField.gridScale;

		// convert body position to grid
		Vector2 position = body.position;
		position = (position + world.extents) * m_vectorField.gridScale;
		int x = Mathf.FloorToInt(position.x);
		int y = Mathf.FloorToInt(position.y);

		if ((x < 0 || x >= m_grid.GetLength(0)) || (y < 0 || y >= m_grid.GetLength(1))) return;

		//Vector2 linePosition = new Vector2((x * gridSize.x), (y * gridSize.y)) + (world.min) + (gridSize * 0.5f);
		//Vector2 direction = m_grid[x, y];
		//Debug.DrawLine(linePosition, linePosition + direction, Color.red);

		Vector3 force = m_grid[x, y] * m_vectorField.force;
		body.ApplyForce(force, PhysicsBody.eForceMode.ACCELERATION);
	}

	private void Start()
	{
		AABB world = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
		Vector2 size = world.size * m_vectorField.gridScale;
		Vector2 gridSize = Vector2.one / m_vectorField.gridScale;

		m_grid = new Vector2[Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y)];
		
		for (int x = 0; x < m_grid.GetLength(0); x++)
		{
			for (int y = 0; y < m_grid.GetLength(1); y++)
			{
				float nx = m_vectorField.noiseOffset + ((x * gridSize.x) * m_vectorField.noiseScale);
				float ny = m_vectorField.noiseOffset + ((y * gridSize.y) * m_vectorField.noiseScale);
				float angle = Mathf.PerlinNoise(nx, ny) * Mathf.PI * 2;

				Vector2 direction = Vector2.zero;
				direction.x = Mathf.Cos(angle);
				direction.y = Mathf.Sin(angle);

				m_grid[x, y] = direction;
			}
		}
	}

	void OnDrawGizmos()
	{
		AABB world = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
		Vector2 size = world.size * m_vectorField.gridScale;
		Vector2 gridSize = Vector2.one / m_vectorField.gridScale;

		if (m_drawGrid)
		{
			Gizmos.color = Color.white;

			for (int x = 0; x < Mathf.CeilToInt(size.x); x++)
			{
				Gizmos.DrawLine(new Vector2((x * gridSize.x) + world.min.x, world.min.y), new Vector2((x * gridSize.x) + world.min.x, world.max.y));
			}
			for (int y = 0; y < Mathf.CeilToInt(size.y); y++)
			{
				Gizmos.DrawLine(new Vector2(world.min.x, (y * gridSize.y) + world.min.y), new Vector2(world.max.x, (y * gridSize.y) + world.min.y));
			}
		}

		if (m_drawForces)
		{
			for (int x = 0; x < Mathf.CeilToInt(size.x); x++)
			{
				for (int y = 0; y < Mathf.CeilToInt(size.y); y++)
				{
					float nx = m_vectorField.noiseOffset + ((x * gridSize.x) * m_vectorField.noiseScale);
					float ny = m_vectorField.noiseOffset + ((y * gridSize.y) * m_vectorField.noiseScale);
					float angle = Mathf.PerlinNoise(nx, ny) * Mathf.PI * 2;

					Vector2 position = new Vector2((x * gridSize.x), (y * gridSize.y)) + (world.min) + (gridSize * 0.5f);
					Vector2 direction = Vector2.zero;
					direction.x = Mathf.Cos(angle);
					direction.y = Mathf.Sin(angle);

					Gizmos.DrawLine(position, position + direction);
				}
			}
		}

	}
}

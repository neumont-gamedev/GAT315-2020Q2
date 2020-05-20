using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldForce : Force
{
    [SerializeField] VectorFieldData m_vectorField = null;
    [SerializeField] bool m_showGrid = false;
    [SerializeField] bool m_showVectors = false;

    Vector2[,] m_grid;

    public override void ApplyForce(PhysicsBody body, float strength)
    {
        AABB aabb = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
        Vector2 size = aabb.size * m_vectorField.gridScale;
        Vector2 gridSize = Vector2.one / m_vectorField.gridScale;

        Vector2 position = body.position;
        position = (position + aabb.extents) * m_vectorField.gridScale;

        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        if (x < 0 || x >= m_grid.GetLength(0) || y < 0 || y >= m_grid.GetLength(1)) return;

        Vector2 force = m_grid[x, y] * strength;
        body.ApplyForce(force, PhysicsBody.eForceMode.ACCELERATION);
    }

    void Start()
    {
        AABB aabb = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
        Vector2 size = aabb.size * m_vectorField.gridScale;
        Vector2 gridSize = Vector2.one / m_vectorField.gridScale;

        m_grid = new Vector2[Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y)];

        for (int x = 0; x < Mathf.CeilToInt(size.x); x++)
        {
            for (int y = 0; y < Mathf.CeilToInt(size.y); y++)
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

    private void OnDrawGizmos()
    {
        AABB aabb = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);
        Vector2 size = aabb.size * m_vectorField.gridScale;
        float gridSize = 1.0f / m_vectorField.gridScale;

        if (m_showGrid)
        {
            Gizmos.color = Color.white;
            for (int x = 0; x < Mathf.CeilToInt(size.x); x++)
            {
                Gizmos.DrawLine(new Vector2((x * gridSize) + aabb.min.x, aabb.min.y), new Vector2((x * gridSize) + aabb.min.x, aabb.max.y));
            }
            for (int y = 0; y < Mathf.CeilToInt(size.y); y++)
            {
                Gizmos.DrawLine(new Vector2(aabb.min.x, (y * gridSize) + aabb.min.y), new Vector2(aabb.max.x, (y * gridSize) + aabb.min.y));
            }
        }

        if (m_showVectors)
        {
            for (int x = 0; x < Mathf.CeilToInt(size.x); x++)
            {
                for (int y = 0; y < Mathf.CeilToInt(size.y); y++)
                {
                    float nx = m_vectorField.noiseOffset + ((x * gridSize) * m_vectorField.noiseScale);
                    float ny = m_vectorField.noiseOffset + ((y * gridSize) * m_vectorField.noiseScale);
                    float angle = Mathf.PerlinNoise(nx, ny) * Mathf.PI * 2;

                    Vector2 direction = Vector2.zero;
                    direction.x = Mathf.Cos(angle);
                    direction.y = Mathf.Sin(angle);

                    Vector2 position = new Vector2((x * gridSize), (y * gridSize)) + aabb.min + new Vector2(gridSize * 0.5f, gridSize * 0.5f);

                    Gizmos.DrawLine(position, position + direction);
                }
            }
        }
    }

}

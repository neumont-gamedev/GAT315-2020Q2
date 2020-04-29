using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
	AABB m_aabb;
	int m_capacity;
	List<PhysicsBody> m_bodies = new List<PhysicsBody>();
	bool m_subdivided = false;

	QuadtreeNode m_northeast = null;
	QuadtreeNode m_northwest = null;
	QuadtreeNode m_southeast = null;
	QuadtreeNode m_southwest = null;

	public QuadtreeNode(AABB aabb, int capacity)
	{
		m_aabb = aabb;
		m_capacity = capacity;
	}

	public void Insert(PhysicsBody body)
	{
		AABB aabb = body.shape.ComputeAABB(body.position);

		if (!m_aabb.Contains(aabb)) return;

		if (m_bodies.Count < m_capacity)
		{
			m_bodies.Add(body);
		}
		else
		{
			if (!m_subdivided)
			{
				Subdivide();
			}

			m_northeast.Insert(body);
			m_northwest.Insert(body);
			m_southeast.Insert(body);
			m_southwest.Insert(body);
		}
	}

	public void Query(AABB aabb, ref List<PhysicsBody> bodies)
	{
		if (!m_aabb.Contains(aabb)) return;

		foreach (PhysicsBody body in m_bodies)
		{
			AABB queryAABB = body.shape.ComputeAABB(body.position);

			//Debug.DrawLine(aabb.center, queryAABB.center);
			if (queryAABB.Contains(aabb))
			{
				bodies.Add(body);
			}
		}

		if (m_subdivided)
		{
			m_northeast.Query(aabb, ref bodies);
			m_northwest.Query(aabb, ref bodies);
			m_southeast.Query(aabb, ref bodies);
			m_southwest.Query(aabb, ref bodies);
		}
	}

	void Subdivide()
	{
		m_subdivided = true;

		m_northeast = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x + (m_aabb.extents.x * 0.5f), m_aabb.center.y + (m_aabb.extents.y * 0.5f)), m_aabb.extents), m_capacity);
		m_northwest = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x - (m_aabb.extents.x * 0.5f), m_aabb.center.y + (m_aabb.extents.y * 0.5f)), m_aabb.extents), m_capacity);
		m_southeast = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x + (m_aabb.extents.x * 0.5f), m_aabb.center.y - (m_aabb.extents.y * 0.5f)), m_aabb.extents), m_capacity);
		m_southwest = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x - (m_aabb.extents.x * 0.5f), m_aabb.center.y - (m_aabb.extents.y * 0.5f)), m_aabb.extents), m_capacity);
	}

	public void Draw()
	{
		Color color = Color.red;
		m_aabb.Draw(color);
		if (m_subdivided)
		{
			m_northeast.Draw();
			m_northwest.Draw();
			m_southeast.Draw();
			m_southwest.Draw();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
	AABB m_aabb;
	int m_capacity;
	List<PhysicsBody> m_bodies = null;
	bool m_subdivided = false;
	int m_level = 0;

	QuadtreeNode m_northeast = null;
	QuadtreeNode m_northwest = null;
	QuadtreeNode m_southeast = null;
	QuadtreeNode m_southwest = null;

	public QuadtreeNode(AABB aabb, int capacity, int level)
	{
		m_aabb = aabb;
		m_capacity = capacity;
		m_bodies = new List<PhysicsBody>();
		m_level = level;
	}

	public void Insert(PhysicsBody body)
	{
		AABB aabb = body.shape.ComputeAABB(body.position);

		// check if within boundary
		if (!m_aabb.Contains(aabb)) return;

		// check if under capacity, if so add to node bodies
		if (m_bodies.Count < m_capacity)
		{
			m_bodies.Add(body);
		}
		else
		{
			// exceeded capactity, subdivide aabb and insert in to containing boundry
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
			BroadPhase.NumberOfTests++;
			AABB queryAABB = body.shape.ComputeAABB(body.position);
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
		float xo = m_aabb.extents.x * 0.5f;
		float yo = m_aabb.extents.y * 0.5f;

		m_northeast = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x - xo, m_aabb.center.y + yo), m_aabb.extents), m_capacity, m_level + 1);
		m_northwest = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x + xo, m_aabb.center.y + yo), m_aabb.extents), m_capacity, m_level + 1);
		m_southeast = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x - xo, m_aabb.center.y - yo), m_aabb.extents), m_capacity, m_level + 1);
		m_southwest = new QuadtreeNode(new AABB(new Vector2(m_aabb.center.x + xo, m_aabb.center.y - yo), m_aabb.extents), m_capacity, m_level + 1);

		m_subdivided = true;
	}

	public void Draw()
	{
		Color color = BroadPhase.colors[m_level % BroadPhase.colors.Length];

		AABB aabb = new AABB(m_aabb.center, m_aabb.size * 0.98f);
		aabb.Draw(color);
		m_bodies.ForEach(body => body.shape.color = color);
		m_bodies.ForEach(body => Debug.DrawLine(body.position, m_aabb.center, color));

		if (m_subdivided)
		{
			m_northeast.Draw();
			m_northwest.Draw();
			m_southeast.Draw();
			m_southwest.Draw();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
	public AABB m_aabb;
	public List<PhysicsBody> m_bodies = null;
	public BVHNode m_left = null;
	public BVHNode m_right = null;
	int m_level = 0;

	public BVHNode(List<PhysicsBody> bodies, int level)
	{
		m_bodies = bodies;
		m_level = level;

		ComputeBoundary();
		Split();
	}

	public void Query(AABB aabb, ref List<PhysicsBody> bodies)
	{
		if (!m_aabb.Contains(aabb))	return;

		if (m_bodies.Count > 0)
		{
			bodies.AddRange(m_bodies);
		}

		if (m_left != null)  m_left.Query(aabb, ref bodies);
		if (m_right != null) m_right.Query(aabb, ref bodies);
	}

	public void ComputeBoundary()
	{
		if (m_bodies.Count > 0)
		{
			m_aabb.center = m_bodies[0].position;
			m_aabb.size = Vector3.zero;

			foreach (PhysicsBody body in m_bodies)
			{
				AABB aabb = body.shape.ComputeAABB(body.position);
				m_aabb.Expand(aabb);
			}
		}
	}

	public void Split()
	{
		int length = m_bodies.Count;
		int half = length / 2;
		if (half >= 1)
		{
			m_left  = new BVHNode(m_bodies.GetRange(0, half), m_level + 1);
			m_right = new BVHNode(m_bodies.GetRange(half, half + (length % 2)), m_level + 1);

			m_bodies.Clear();
		}
	}

	public void Draw()
	{
		Color color = BroadPhase.colors[m_level % BroadPhase.colors.Length];

		AABB aabb = new AABB(m_aabb.center, m_aabb.size * 1.025f);
		aabb.Draw(color);

		if (m_left != null)  m_left.Draw();
		if (m_right != null) m_right.Draw();
	}
}

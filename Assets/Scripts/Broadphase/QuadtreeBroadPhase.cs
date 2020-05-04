using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeBroadPhase : BroadPhase
{
	public int capacity { get; set; } = 4;

	QuadtreeNode m_rootNode = null;

	

	public override void Build(AABB aabb, ref List<PhysicsBody> bodies)
	{
		NumberOfTests = 0;
		m_rootNode = new QuadtreeNode(aabb, capacity, 0);
		bodies.ForEach(body => { m_rootNode.Insert(body); });
	}

	public override void Query(AABB aabb, ref List<PhysicsBody> bodies)
	{
		m_rootNode.Query(aabb, ref bodies);
	}

	public override void Query(PhysicsBody body, ref List<PhysicsBody> bodies)
	{
		AABB aabb = body.shape.ComputeAABB(body.position);
		m_rootNode.Query(aabb, ref bodies);
	}

	public override void Draw()
	{
		if (m_rootNode != null)
		{
			m_rootNode.Draw();
		}
	}
}

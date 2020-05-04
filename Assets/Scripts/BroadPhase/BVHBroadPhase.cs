using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHBroadPhase : BroadPhase
{
	BVHNode m_rootNode = null;

	public override void Build(AABB aabb, ref List<PhysicsBody> bodies)
	{
		List<PhysicsBody> bvhBodies = new List<PhysicsBody>(bodies);
	
		bvhBodies.Sort((a, b) => (a.position.x.CompareTo(b.position.x)));
		m_rootNode = new BVHNode(bvhBodies, 0);
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

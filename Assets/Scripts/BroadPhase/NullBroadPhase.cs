using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullBroadPhase : BroadPhase
{
	public List<PhysicsBody> bodies { get; set; } = new List<PhysicsBody>();

	public override void Build(AABB aabb, ref List<PhysicsBody> bodies)
	{
		this.bodies.Clear();
		this.bodies.AddRange(bodies);
	}
	
	public override void Query(AABB aabb, ref List<PhysicsBody> bodies)
	{
		bodies.AddRange(this.bodies);
	}

	public override void Query(PhysicsBody body, ref List<PhysicsBody> bodies)
	{
		Query(body.shape.ComputeAABB(body.position), ref bodies);
	}

	public override void Draw()
	{
		//
	}
}

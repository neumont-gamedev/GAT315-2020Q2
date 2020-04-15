using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Contact
{
	public Manifold manifold;
	public float friction;
	public float restitution;

	public PhysicsBody bodyA;
	public PhysicsBody bodyB;
}

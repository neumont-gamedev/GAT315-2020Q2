using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSpringJoint : PhysicsJoint
{
	public override void ApplyForce(float dt)
	{
		throw new System.NotImplementedException();
	}

	public static Vector2 SpringForce(Vector2 anchor, Vector2 body, float restLength, float k)
	{
		Vector2 direction = body - anchor;
		float length = direction.magnitude;
		float x = length - restLength;

		return (-k * x) * direction.normalized;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSpringJoint : PhysicsJoint
{
    public float restLength { get; set; } = 0.0f;
	public float k { get; set; } = 20.0f;

    public override void ApplyForce(float dt)
    {
		Vector2 force = SpringForce(bodyA.position, bodyB.position, restLength, k);
		float modifier = (bodyA.type == BodyTypeEnumRef.eType.Static || bodyB.type == BodyTypeEnumRef.eType.Static) ? 1.0f : 0.5f;

		bodyA.ApplyForce(-force * modifier, PhysicsBody.eForceMode.IMPULSE);
		bodyB.ApplyForce( force * modifier, PhysicsBody.eForceMode.IMPULSE);
	}

	public static Vector2 SpringForce(Vector2 anchor, Vector2 body, float springRestLength, float springK)
	{
		Vector2 direction = body - anchor;
		float length = direction.magnitude;
		float x = length - springRestLength;

		return (-springK * x) * direction.normalized;
	}
}

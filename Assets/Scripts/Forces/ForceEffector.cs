using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEffector : Force
{
	[SerializeField] Shape m_shape = null;

	public Vector2 position { get { return transform.position; } set { transform.position = value; } }
	public float forceMagnitude { get; set; }
	public float drag { get; set; }
	public Shape shape { get => m_shape; set => m_shape = value; }

	public override void ApplyForce(PhysicsBody body)
	{
		if (Collision.TestOverlap(shape, position, body.shape, body.position))
		{
			Vector2 velocity = body.velocity;
			CircleShape circle = (CircleShape)shape;

			float density = 1;
			float speed = velocity.magnitude;
			float area = circle.radius;

			float force = 0.5f * (density * area * drag * (speed * speed));
			body.ApplyForce(-velocity.normalized * force, PhysicsBody.eForceMode.FORCE);

			Debug.DrawLine(body.position, body.position + -velocity.normalized * force);

			//Vector2 v = body.position - position;
			//float distance = v.magnitude;
			//float t = Mathf.Clamp(distance /circle.radius, 0, 1);
			//float force = (1 - (t / circle.radius)) * forceMagnitude;
			//float force = forceMagnitude;
			//body.ApplyForce(v.normalized * force, PhysicsBody.eForceMode.FORCE);
		}
	}
}

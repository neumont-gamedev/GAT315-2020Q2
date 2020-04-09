using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
	public enum eForceMode
	{
		ACCELERATION,
		VELOCITY,
		IMPULSE,
		FORCE
	}

	[SerializeField] Shape m_shape = null;

	public Vector2 position { get { return transform.position; } set { transform.position = value; } }
	public Vector2 force { get; set; } = Vector2.zero;
	public Vector2 acceleration { get; set; } = Vector2.zero;
	public Vector2 velocity { get; set; } = Vector2.zero;
	public float damping { get; set; } = 1.0f;
	public float mass { get; set; } = 1.0f;
	public float gravityScale { get; set; } = 1.0f;
	public Shape shape { get => m_shape; set => m_shape = value; }

	public void ApplyForce(Vector2 force, eForceMode mode)
	{
		switch (mode)
		{
			case eForceMode.ACCELERATION:
				this.acceleration = force;
				break;
			case eForceMode.VELOCITY:
				this.velocity = force;
				break;
			case eForceMode.IMPULSE:
				this.force = this.force + force;
				break;
			case eForceMode.FORCE:
				this.force = this.force + (force * PhysicsWorld.fixedTimeStep);
				break;
			default:
				break;
		}
	}

	public void Step(float dt)
	{
		acceleration = acceleration + (PhysicsWorld.gravity * gravityScale) + (force / mass);
	}
}

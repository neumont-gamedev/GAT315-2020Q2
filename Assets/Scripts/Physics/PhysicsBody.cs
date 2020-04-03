using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
	public enum eType
	{
		DYNAMIC,
		STATIC,
		KINEMATIC
	}

	public enum eForceMode
	{
		ACCELERATION,
		VELOCITY,
		IMPULSE,
		FORCE
	}

	public Vector2 position { get { return transform.position; } set { transform.position = value; } }

	public Vector2 force { get; set; } = Vector2.zero;
	public Vector2 acceleration { get; set; } = Vector2.zero;
	public Vector2 velocity { get; set; } = Vector2.zero;
	public float mass { get; set; } = 1.0f; // kg
	public float density { get; set; } = 1.0f;
	public float gravityScale { get; set; } = 0.0f;
	
	public void ApplyForce(Vector2 force, eForceMode mode = eForceMode.FORCE)
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
				this.force += this.force + (force / mass);
				break;
			case eForceMode.FORCE:
				this.force += this.force + (force / mass) * PhysicsWorld.fixedTimeStep;
				break;
			default:
				break;
		}
	}

	public void Step(float dt)
	{
		Debug.DrawLine(position, position + force, Color.white);
		Debug.DrawLine(position, position + (PhysicsWorld.gravity * gravityScale), Color.blue);

		acceleration = (PhysicsWorld.gravity * gravityScale) + force;

		velocity = velocity + acceleration * dt;
		position = position + velocity * dt;

		//velocity += dt * (1 / mass) * (b->m_gravity * b->m_mass * gravity + b->m_force);
		//acceleration =  invMass * (gravityScale * b->m_mass * gravity + b->m_force);
	}
}

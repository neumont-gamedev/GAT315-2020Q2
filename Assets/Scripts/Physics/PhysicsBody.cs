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

	public enum eState
	{
		ACTIVE		= (1 << 0),
		COLLIDED	= (1 << 1),
		AWAKE		= (1 << 2)
	}

	// 0, 0, 0, 0, 0, 1, 1, 1

	[SerializeField] Shape m_shape = null;

	public BodyTypeEnumRef.eType type { get; set; } = BodyTypeEnumRef.eType.Dynamic;
	public Vector2 position { get { return transform.position; } set { transform.position = value; } }
	public Vector2 force { get; set; } = Vector2.zero;
	public Vector2 acceleration { get; set; } = Vector2.zero;
	public Vector2 velocity { get; set; } = Vector2.zero;
	public float averageSpeed { get; set; } = 0.5f;
	public float damping { get; set; } = 1.0f;
	public float mass { get; set; } = 3.0f;
	public float inverseMass { get { return (mass == 0) ? 0 : 1.0f / mass; } }
	public float gravityScale { get; set; } = 1.0f;
	public Shape shape { get => m_shape; set => m_shape = value; }
	public eState state { get; set; } = eState.ACTIVE | eState.AWAKE; // state = 1, 0, 1

	public void ApplyForce(Vector2 force, eForceMode mode)
	{
		if (type != BodyTypeEnumRef.eType.Dynamic) return;

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
		shape.color = ((state & eState.AWAKE) != 0) ? Color.white : Color.gray;

		if (type != BodyTypeEnumRef.eType.Dynamic) return;

		// update acceleration
		acceleration = acceleration + (PhysicsWorld.gravity * gravityScale) + (force / mass);
	}

	public void SetAwake(bool awake = true)
	{
		if (awake)
		{
			state |= eState.AWAKE;
			averageSpeed = 0.5f;
		}
		else
		{
			state &= ~eState.AWAKE;
			velocity = Vector3.zero;
		}
	}

	public void UpdateSleep(float limit)
	{
		float bias = 0.5f;
		float speed = velocity.magnitude;
		averageSpeed = (bias * averageSpeed) + ((1.0f - bias) * speed);
		if (averageSpeed < limit)
		{
			SetAwake(false);
		}
	}

	static public void UpdateAwake(ref List<Contact> contacts)
	{
		foreach (Contact contact in contacts)
		{
			if (contact.bodyA.type == BodyTypeEnumRef.eType.Dynamic && ((contact.bodyA.state & eState.AWAKE) == 0))
			{
				contact.bodyA.SetAwake(true);
			}
			if (contact.bodyB.type == BodyTypeEnumRef.eType.Dynamic && ((contact.bodyB.state & eState.AWAKE) == 0))
			{
				contact.bodyB.SetAwake(true);
			}
		}
	}
}

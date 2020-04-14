using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAction : Action
{
	[SerializeField] GameObject m_gameObject = null;

	[SerializeField] EmissionEnumRef m_emission = null;
	[SerializeField] FloatRef m_velocity = null;

	[SerializeField] BodyTypeEnumRef m_bodyType = null;
	[SerializeField] FloatRef m_damping = null;
	[SerializeField] FloatRef m_radius = null;

	float timer { get; set; } = 0.0f;

	private void Update()
	{
		if (!active) return;

		switch (m_emission.type)
		{
			case EmissionEnumRef.eType.Single:
				{
					Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Vector2 velocity = Random.insideUnitCircle.normalized * m_velocity.value;
					Create(position, velocity);
					active = false;
				}
				break;

			case EmissionEnumRef.eType.Burst:
				for (int i = 0; i < 20; i++)
				{
					Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Vector2 velocity = Random.insideUnitCircle.normalized * m_velocity.value;
					Create(position, velocity);
					active = false;
				}
				break;

			case EmissionEnumRef.eType.Stream:
				{
					float rateTime = 1.0f / 30.0f;
					timer = timer + Time.deltaTime;
					while (timer > rateTime)
					{
						Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						Vector2 velocity = Random.insideUnitCircle.normalized * m_velocity.value;
						Create(position, velocity);

						timer = timer - rateTime;
					}
				}
				break;

			default:
				break;
		}
	}

	void Create(Vector2 position, Vector2 velocity)
	{
		GameObject go = Instantiate(m_gameObject, position, Quaternion.identity);
		PhysicsBody body = go.GetComponent<PhysicsBody>();

		body.type = m_bodyType.type;
		body.damping = m_damping.value;

		((CircleShape)body.shape).radius = m_radius;
		body.mass = body.shape.ComputeMass(2.0f);
		
		body.ApplyForce(velocity, PhysicsBody.eForceMode.VELOCITY);

		m_physicsWorld.bodies.Add(body);
	}
	
	public override void StartEvent()
	{
		active = true;
	}

	public override void StopEvent()
	{
		active = false;
	}
}

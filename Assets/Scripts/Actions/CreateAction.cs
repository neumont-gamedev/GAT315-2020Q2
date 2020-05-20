using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAction : Action
{
	[SerializeField] GameObject[] m_gameObject = null;

	[SerializeField] BodyTypeEnumRef m_bodyType = null;
	[SerializeField] ShapeEnumRef m_shapeType = null;
	[SerializeField] EmissionEnumRef m_emission = null;
	[SerializeField] FloatRef m_velocity = null;
	[SerializeField] FloatRef m_resitution = null;

	[SerializeField] FloatRef m_damping = null;
	[SerializeField] FloatRef m_restitution = null;
	[SerializeField] FloatRef m_size = null;

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
					//Debug.Break();
					Vector3 offset = Random.insideUnitCircle.normalized * m_size.value * 0.5f;
					Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
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
						Vector3 offset = Random.insideUnitCircle.normalized * m_size.value * 0.5f;
						Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
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
		GameObject go = Instantiate(m_gameObject[m_shapeType.index], position, Quaternion.identity);
		PhysicsBody body = go.GetComponent<PhysicsBody>();

		body.type = m_bodyType.type;
		body.damping = m_damping.value;
		body.restitution = m_resitution.value;

		if (body.shape.type == Shape.eType.CIRCLE)
		{
			((CircleShape)body.shape).radius = m_size.value;
		}
		else
		{
			((BoxShape)body.shape).size = new Vector2(m_size.value, m_size.value);
		}
		body.mass = (body.type == BodyTypeEnumRef.eType.Static) ? 0.0f : body.shape.ComputeMass(1.0f);
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

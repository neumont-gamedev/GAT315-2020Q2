using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : Action
{
	[SerializeField] ShapeEnumRef m_shape = null;
	[SerializeField] FloatRef m_size = null;
	[SerializeField] FloatRef m_density = null;

	[SerializeField] EmissionEnumRef m_emission = null;
	[SerializeField] FloatRef m_velocity = null;

	[SerializeField] GameObject m_gameObject = null;

	public bool active { get; set; } = false;

	public override void StartEvent()
	{
		Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject go = Instantiate(m_gameObject, position, Quaternion.identity);
		PhysicsBody body = go.GetComponent<PhysicsBody>();
		body.ApplyForce(Random.insideUnitCircle.normalized * m_velocity.value, PhysicsBody.eForceMode.VELOCITY);

		CircleShape circleShape = body.shape as CircleShape;
		if (circleShape != null)
		{
			circleShape.radius = m_size.value;
			body.mass = circleShape.ComputeMass(m_density.value);
		}

		m_physicsWorld.bodies.Add(body);
	}

	public override void StopEvent()
	{
	}
}

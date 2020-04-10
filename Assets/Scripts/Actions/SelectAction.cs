using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : Action
{
	[SerializeField] LineRenderer m_lineRenderer = null;

	PhysicsBody bodySelect { get; set; } = null;

	private void Update()
	{
		m_lineRenderer.enabled = active;
		if (bodySelect != null)
		{
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			m_lineRenderer.SetPosition(0, bodySelect.position);
			m_lineRenderer.SetPosition(1, position);

			if (bodySelect.type == BodyTypeEnumRef.eType.Dynamic)
			{
				Vector2 force = PhysicsSpringJoint.SpringForce(position, bodySelect.position, 0.01f, 30.0f);
				bodySelect.ApplyForce(force, PhysicsBody.eForceMode.IMPULSE);
			}
			else
			{
				bodySelect.position = position;
			}
		}
	}

	public override void StartEvent()
	{
		PhysicsBody body = PhysicsWorld.GetPhysicsBodyFromPosition(Input.mousePosition);
		if (body != null)
		{
			bodySelect = body;
			active = true;
		}
	}

	public override void StopEvent()
	{
		bodySelect = null;
		active = false;
	}
}

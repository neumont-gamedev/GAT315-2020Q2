using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointAction : Action
{
	[SerializeField] LineRenderer m_lineRenderer = null;
	[SerializeField] FloatRef m_k = null;

	PhysicsBody bodyAnchor { get; set; } = null;
	
	private void Update()
	{
		m_lineRenderer.enabled = active;
		if (bodyAnchor != null)
		{
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			m_lineRenderer.SetPosition(0, bodyAnchor.position);
			m_lineRenderer.SetPosition(1, position);
		}
	}

	void Create(PhysicsBody bodyA, PhysicsBody bodyB, float restLength, float k)
	{
		PhysicsSpringJoint joint = new PhysicsSpringJoint();
		joint.bodyA = bodyA;
		joint.bodyB = bodyB;
		joint.restLength = restLength;
		joint.k = k;

		m_physicsWorld.joints.Add(joint);
	}
	
	public override void StartEvent()
	{
		PhysicsBody body = PhysicsWorld.GetPhysicsBodyFromPosition(Input.mousePosition);
		if (body != null)
		{
			bodyAnchor = body;
			active = true;
		}
	}

	public override void StopEvent()
	{
		if (bodyAnchor != null)
		{
			PhysicsBody body = PhysicsWorld.GetPhysicsBodyFromPosition(Input.mousePosition);
			if (body != null && body != bodyAnchor)
			{
				float restLength = (bodyAnchor.position - body.position).magnitude;
				Create(bodyAnchor, body, restLength, m_k.value);
			}
		}

		bodyAnchor = null;
		active = false;
	}
}

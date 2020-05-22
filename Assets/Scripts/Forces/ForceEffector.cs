using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEffector : Force
{
	[SerializeField] Shape m_shape = null;

	public float magnitude { get; set; } = 0;
	public float distanceScale { get; set; } = 1;
	public float drag { get; set; } = 1;
	// mode (constant, inverse linear, inverse squared)

	public Shape shape { get => m_shape; set => m_shape = value; }
	
	public override void ApplyForce(PhysicsBody body)
	{
		Vector2 v = body.position - (Vector2)transform.position;
		float distance = v.magnitude;
	}
}

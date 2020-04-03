using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
	public Vector2 position { get { return transform.position; } set { transform.position = value; } }

	public Vector2 force { get; set; }
	public Vector2 acceleration { get; set; }
	public Vector2 velocity { get; set; }
	
	public void Step(float dt)
	{
		acceleration = force;
		velocity = velocity + acceleration * dt;
		position = position + velocity * dt;
	}
}

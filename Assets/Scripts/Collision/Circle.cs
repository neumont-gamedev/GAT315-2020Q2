using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Circle
{
	public Vector2 center { get; set; }
	public float radius { get; set; }

	public Circle(Vector2 center, float radius)
	{
		this.center = center;
		this.radius = radius;
	}

	public bool Contains(Circle circle)
	{
		Vector2 v = (this.center) - circle.center;
		float distance = v.sqrMagnitude;

		return (distance <= ((this.radius * this.radius) + (circle.radius * circle.radius)));
	}
}

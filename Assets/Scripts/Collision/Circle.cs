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

	public bool Contains(Vector2 point)
	{
		Vector3 dv = center - point;
		float distance = dv.sqrMagnitude;

		return (distance <= (radius * radius));
	}

	public bool Contains(Circle circle)
	{
		Vector3 dv = center - circle.center;
		float distance = dv.sqrMagnitude;

		return (distance <= ((radius * radius) + (circle.radius * circle.radius)));
	}

	public void Expand(Circle circle)
	{
		Vector2 dv = center - circle.center;
		float distance = dv.sqrMagnitude;

		float dr = radius - circle.radius;
		// larger sphere fully encloses the other sphere
		if (distance <= (dr * dr))
		{
			center = (radius > circle.radius) ? center : circle.center;
			radius = (radius > circle.radius) ? radius : circle.radius;
		}
		else
		{
			distance = dv.magnitude;
			float maxRadius = (distance + radius + circle.radius) * 0.5f;
			if (distance  > float.Epsilon)
			{
				center = center + ((maxRadius - radius) / distance) * dv;
			}
		}
	}

	public void ConvertTo(ref AABB aabb)
	{
		aabb.SetMinMax(center - new Vector2(radius, radius), center + new Vector2(radius, radius));
	}

	public static bool Intersects(Circle circleA, Circle circleB)
	{
		return circleA.Contains(circleB);
	}
}

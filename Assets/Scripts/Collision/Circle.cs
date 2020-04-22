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
		float sqrDistance = dv.sqrMagnitude;
		float sqrRadius = (radius * radius);

		return (sqrDistance <= sqrRadius);
	}

	public bool Contains(Circle circle)
	{
		Vector3 dv = center - circle.center;
		float sqrDistance = dv.sqrMagnitude;
		float sqrRadius = ((radius + circle.radius) * (radius + circle.radius));

		return (sqrDistance <= sqrRadius);
	}

	public bool Contains(AABB aabb)
	{
		Vector2 v = center - aabb.center;

		// closest point on AABB to center of Circle
		Vector2 closest = v;

		// clamp point to edges of the AABB
		closest.x = Mathf.Clamp(closest.x, -aabb.extents.x, aabb.extents.x);
		closest.y = Mathf.Clamp(closest.y, -aabb.extents.y, aabb.extents.y);

		// circle inside AABB
		if (v == closest) return true;

		Vector2 normal = v - closest;
		float sqrDistance = normal.sqrMagnitude;
		float sqrRadius = (radius * radius);

		return (sqrDistance < sqrRadius);
	}
}

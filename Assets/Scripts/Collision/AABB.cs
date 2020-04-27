using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AABB
{
	public Vector2 center;
	public Vector2 size;
	public Vector2 extents { get { return size * 0.5f; } }

	public Vector2 min { get { return center - extents; } set { SetMinMax(value, max); } }
	public Vector2 max { get { return center + extents; } set { SetMinMax(min, value); } }

	public AABB(Vector2 center, Vector2 size)
	{
		this.center = center;
		this.size = size;
	}

	public bool Contains(AABB aabb)
	{
		return (aabb.max.x >= min.x && aabb.min.x <= max.x) &&
			   (aabb.max.y >= min.y && aabb.min.y <= max.y);
	}

	public bool Contains(Vector2 point)
	{
		return (point.x >= min.x && point.x <= max.x) &&
			   (point.y >= min.y && point.y <= max.y);
	}

	public bool Contains(Circle circle)
	{
		Vector2 v = circle.center - center;

		// closest point on AABB to center of Circle
		Vector2 closest = v;

		// clamp point to edges of the AABB
		closest.x = Mathf.Clamp(closest.x, -extents.x, extents.x);
		closest.y = Mathf.Clamp(closest.y, -extents.y, extents.y);

		//Debug.DrawLine(center + closest, circle.center, Color.red);

		// circle inside AABB
		if (v == closest) return true;

		Vector2 normal = v - closest;
		float sqrDistance = normal.sqrMagnitude;
		float sqrRadius = (circle.radius * circle.radius);
		
		return (sqrDistance < sqrRadius);
	}

	public void SetMinMax(Vector2 min, Vector2 max)
	{
		size = (max - min);
		center = min + extents;
	}

	public void Draw(Color color, float duration = 0.0f)
	{
		Debug.DrawLine(new Vector2(min.x, max.y), new Vector2(max.x, max.y), color, duration);
		Debug.DrawLine(new Vector2(max.x, max.y), new Vector2(max.x, min.y), color, duration);
		Debug.DrawLine(new Vector2(max.x, min.y), new Vector2(min.x, min.y), color, duration);
		Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(min.x, max.y), color, duration);
	}
}

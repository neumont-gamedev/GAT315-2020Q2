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

	public void SetMinMax(Vector2 min, Vector2 max)
	{
		size = (max - min);
		center = min + extents;
	}
}

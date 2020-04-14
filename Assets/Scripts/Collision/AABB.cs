using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AABB
{
	public Vector3 center;
	public Vector3 size;
	public Vector3 extents { get { return size * 0.5f; } }

	public Vector3 min { get { return center - extents; } set { SetMinMax(value, max); } }
	public Vector3 max { get { return center + extents; } set { SetMinMax(min, value); } }

	public AABB(Vector3 center, Vector3 size)
	{
		this.center = center;
		this.size = size;
	}

	public bool Contains(AABB aabb)
	{
		return (aabb.max.x >= min.x && aabb.min.x <= max.x) &&
			   (aabb.max.y >= min.y && aabb.min.y <= max.y) &&
			   (aabb.max.z >= min.z && aabb.min.z <= max.z);
	}

	public bool Contains(Vector3 point)
	{
		return (point.x >= min.x && point.x <= max.x) &&
			   (point.y >= min.y && point.y <= max.y) &&
			   (point.z >= min.z && point.z <= max.z);
	}

	public void SetMinMax(Vector3 min, Vector3 max)
	{
		size = (max - min);
		center = min + extents;
	}

	public void Expand(Vector3 point)
	{
		SetMinMax(Vector3.Min(min, point), Vector3.Max(max, point));
	}

	public void Expand(AABB aabb)
	{
		Expand(aabb.center - aabb.extents);
		Expand(aabb.center + aabb.extents);
	}

	public void ConvertTo(ref Circle circle)
	{
		circle.center = center;
		circle.radius = (max - min).magnitude * 0.5f;
	}
}

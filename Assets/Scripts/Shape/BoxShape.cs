using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxShape : Shape
{
	public override eType type { get { return eType.BOX; } }

	public Vector2 size { get { return transform.localScale; } set { transform.localScale = value; } }
	public Vector2 extents { get { return size * 0.5f; } set { size = value * 0.5f; } }
	public Vector2 min { get { return (Vector2)transform.position - extents; } }
	public Vector2 max { get { return (Vector2)transform.position + extents; } }


	public override AABB ComputeAABB(Vector2 position)
	{
		return new AABB(position, size);
	}

	public override float ComputeMass(float density)
	{
		return (size.x * size.y) * density;
	}
}

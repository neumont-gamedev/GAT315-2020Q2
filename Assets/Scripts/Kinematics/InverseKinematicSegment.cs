using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicSegment : KinematicSegment
{
	private void Update()
	{
		transform.localScale = Vector3.one * width;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width)
	{
		this.parent = parent;
		this.width = width;

		this.angle = angle;
		this.length = length;
	}

	public void Follow(Vector2 target)
	{
		Vector2 direction = target - start;
		Coordinate.Polar polar = Coordinate.CartesianToPolar(direction);
		angle = polar.angle;

		start = target - (direction.normalized * length);
	}
}

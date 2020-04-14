using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision
{
	public static bool TestOverlap(Shape shapeA, Vector2 positionA, Shape shapeB, Vector2 positionB)
	{
		Circle circleA = new Circle(positionA, ((CircleShape)shapeA).radius);
		Circle circleB = new Circle(positionB, ((CircleShape)shapeB).radius);

		bool intersects = circleA.Contains(circleB);

		return intersects;
	}
}

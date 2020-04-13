using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision
{
	public static bool TestOverlap(Shape shapeA, Vector2 positionA, Shape shapeB, Vector2 positionB)
	{
		bool intersects = false;

		Circle circleA = new Circle(positionA, ((CircleShape)shapeA).radius);
		Circle circleB = new Circle(positionB, ((CircleShape)shapeB).radius);
		intersects = Circle.Intersects(circleA, circleB);

		return intersects;
	}
}

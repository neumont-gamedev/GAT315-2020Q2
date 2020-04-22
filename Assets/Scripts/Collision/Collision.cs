using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision
{
	public static bool TestOverlap(Shape shapeA, Vector2 positionA, Shape shapeB, Vector2 positionB)
	{
		bool intersects = false;
		if (shapeA.type == Shape.eType.CIRCLE && shapeB.type == Shape.eType.CIRCLE)
		{
			Circle circleA = new Circle(positionA, ((CircleShape)shapeA).radius);
			Circle circleB = new Circle(positionB, ((CircleShape)shapeB).radius);
			intersects = circleA.Contains(circleB);
		}
		if (shapeA.type == Shape.eType.BOX && shapeB.type == Shape.eType.BOX)
		{
			AABB aabbA = new AABB(positionA, ((BoxShape)shapeA).size);
			AABB aabbB = new AABB(positionB, ((BoxShape)shapeB).size);
			intersects = aabbA.Contains(aabbB);
		}
		if (shapeA.type == Shape.eType.BOX && shapeB.type == Shape.eType.CIRCLE)
		{
			AABB aabbA = new AABB(positionA, ((BoxShape)shapeA).size);
			Circle circleB = new Circle(positionB, ((CircleShape)shapeB).radius);
			intersects = aabbA.Contains(circleB);
		}
		if (shapeA.type == Shape.eType.CIRCLE && shapeB.type == Shape.eType.BOX)
		{
			Circle circleA = new Circle(positionA, ((CircleShape)shapeA).radius);
			AABB aabbB = new AABB(positionB, ((BoxShape)shapeB).size);
			intersects = aabbB.Contains(circleA);
		}

		return intersects;
	}

	public static void CreateContacts(ref List<PhysicsBody> bodies, out List<Contact> contacts)
	{
		contacts = new List<Contact>();
		for (int i = 0; i < bodies.Count; i++)
		{
			for (int j = i + 1; j < bodies.Count; j++)
			{
				if (Collision.TestOverlap(bodies[i].shape, bodies[i].position, bodies[j].shape, bodies[j].position))
				{
					Contact contact = new Contact();
					contact.bodyA = bodies[i];
					contact.bodyB = bodies[j];

					if (contact.bodyA.shape.type == Shape.eType.CIRCLE && contact.bodyB.shape.type == Shape.eType.CIRCLE)
					{
						CreateManifold(ref contact.manifold, contact.bodyA.position, ((CircleShape)contact.bodyA.shape).radius,
															 contact.bodyB.position, ((CircleShape)contact.bodyB.shape).radius);
					}
					else if (contact.bodyA.shape.type == Shape.eType.BOX && contact.bodyB.shape.type == Shape.eType.BOX)
					{
						CreateManifold(ref contact.manifold, contact.bodyA.position, ((BoxShape)contact.bodyA.shape).size,
															 contact.bodyB.position, ((BoxShape)contact.bodyB.shape).size);
					}
					else if (contact.bodyA.shape.type == Shape.eType.CIRCLE && contact.bodyB.shape.type == Shape.eType.BOX)
					{
						CreateManifold(ref contact.manifold, contact.bodyA.position, ((CircleShape)contact.bodyA.shape).radius,
										   contact.bodyB.position, ((BoxShape)contact.bodyB.shape).size);
															 
					}
					else if (contact.bodyA.shape.type == Shape.eType.BOX && contact.bodyB.shape.type == Shape.eType.CIRCLE)
					{
						CreateManifold(ref contact.manifold, contact.bodyA.position, ((BoxShape)contact.bodyA.shape).size,
															 contact.bodyB.position, ((CircleShape)contact.bodyB.shape).radius);
					}

					//Debug.DrawLine(contact.bodyB.position, contact.bodyB.position + contact.manifold.normal * contact.manifold.depth, Color.white);

					contacts.Add(contact);
				}
			}
		}
	}

	public static void CreateManifold(ref Manifold manifold, Vector2 positionA, float radiusA, Vector2 positionB, float radiusB)
	{
		manifold.normal = (positionA - positionB).normalized;
		manifold.depth = Mathf.Abs((positionA - positionB).magnitude - (radiusA + radiusB));
	}

	public static void CreateManifold(ref Manifold manifold, Vector2 positionA, Vector2 sizeA, Vector2 positionB, Vector2 sizeB)
	{
		// create AABB
		AABB aabbA = new AABB(positionA, sizeA);
		AABB aabbB = new AABB(positionB, sizeB);

		// direction vector between AABB
		Vector2 v = positionA - positionB;

		// calculate overlap on x axis
		float x_overlap = aabbA.extents.x + aabbB.extents.x - Mathf.Abs(v.x);
		// calculate overlap on y axis
		float y_overlap = aabbA.extents.y + aabbB.extents.y - Mathf.Abs(v.y);

		// SAT test on x and y axis
		if (x_overlap > 0 && y_overlap > 0)
		{
			// Find out which axis is axis of least penetration
			if (x_overlap < y_overlap)
			{
				// if a.x < b.x, contact normal is (-1, 0) = a hit left side of b
				// if a.x > b.x, contact normal is ( 1, 0) = a hit right side of b
				manifold.normal = (v.x < 0) ? Vector2.left : Vector2.right;
				manifold.depth = x_overlap;
			}
			else
			{
				// if a.y < b.y, contact normal is (0, -1) = a hit bottom of b
				// if a.y > b.y, contact normal is (0,  1) = a hit top of b
				manifold.normal = (v.y < 0) ? Vector2.down : Vector2.up;
				manifold.depth = y_overlap;
			}
		}
	}

	public static void CreateManifold(ref Manifold manifold, Vector2 positionA, Vector2 sizeA, Vector2 positionB, float radiusB)
	{
		Vector2 v = positionA - positionB;

		// closest point on A to center of B
		Vector2 closest = v;

		AABB aabbA = new AABB(positionA, sizeA);

		// clamp point to edges of the AABB
		closest.x = Mathf.Clamp(closest.x, -aabbA.extents.x, aabbA.extents.x);
		closest.y = Mathf.Clamp(closest.y, -aabbA.extents.y, aabbA.extents.y);

		bool inside = false;
		
		// circle is inside the AABB, so we need to clamp the circle's center
		// to the closest edge
		if (v == closest)
		{
			inside = true;

			// find closest axis
			if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
			{
				// clamp to closest extent
				closest.x = (closest.x > 0) ? aabbA.extents.x : -aabbA.extents.x;
			}
			// y axis is shorter
			else
			{
				// clamp to closest extent
				closest.y = (closest.y > 0) ? aabbA.extents.y : -aabbA.extents.y;
			}
		}

		Vector2 normal = v - closest;
		float distance = normal.magnitude;

		// collision normal needs to be flipped to point outside if circle was
		// inside the AABB
		manifold.normal = (inside) ? -v.normalized : v.normalized;
		manifold.depth = Mathf.Abs(radiusB - distance);
	}

	public static void CreateManifold(ref Manifold manifold, Vector2 positionA, float radiusA, Vector2 positionB, Vector2 sizeB)
	{
		Vector2 v = positionA - positionB;

		// closest point on A to center of B
		Vector2 closest = v;

		AABB aabbA = new AABB(positionB, sizeB);

		// clamp point to edges of the AABB
		closest.x = Mathf.Clamp(closest.x, -aabbA.extents.x, aabbA.extents.x);
		closest.y = Mathf.Clamp(closest.y, -aabbA.extents.y, aabbA.extents.y);

		bool inside = false;

		// circle is inside the AABB, so we need to clamp the circle's center
		// to the closest edge
		if (v == closest)
		{
			inside = true;

			// find closest axis
			if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
			{
				// clamp to closest extent
				closest.x = (closest.x > 0) ? aabbA.extents.x : -aabbA.extents.x;
			}
			// y axis is shorter
			else
			{
				// clamp to closest extent
				closest.y = (closest.y > 0) ? aabbA.extents.y : -aabbA.extents.y;
			}
		}

		Vector2 normal = v - closest;
		float distance = normal.magnitude;

		// collision normal needs to be flipped to point outside if circle was
		// inside the AABB
		manifold.normal = (inside) ? -v.normalized : v.normalized;
		manifold.depth = Mathf.Abs(radiusA - distance);
	}
}

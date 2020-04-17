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
		else if (shapeA.type == Shape.eType.BOX && shapeB.type == Shape.eType.BOX)
		{
			AABB aabbA = new AABB(positionA, ((BoxShape)shapeA).size);
			AABB aabbB = new AABB(positionB, ((BoxShape)shapeB).size);
			intersects = aabbA.Contains(aabbB);
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

					if (contact.bodyA.shape.type == Shape.eType.BOX && contact.bodyB.shape.type == Shape.eType.BOX)
					{
						CreateManifold(ref contact.manifold, contact.bodyA.position, ((BoxShape)contact.bodyA.shape).size,
															 contact.bodyB.position, ((BoxShape)contact.bodyB.shape).size);
					}

					Debug.DrawLine(contact.bodyB.position, contact.bodyB.position + contact.manifold.normal * contact.manifold.depth, Color.white);

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
}

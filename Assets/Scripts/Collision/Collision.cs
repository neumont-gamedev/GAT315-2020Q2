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

					CreateManifold(ref contact.manifold, bodies[i].position, ((CircleShape)bodies[i].shape).radius, 
														 bodies[j].position, ((CircleShape)bodies[j].shape).radius);

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
}

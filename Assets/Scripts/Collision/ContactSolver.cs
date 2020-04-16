using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContactSolver
{
	public static void Resolve(ref List<Contact> contacts)
	{
		foreach(Contact contact in contacts)
		{
			float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;
			if (totalInverseMass == 0) continue;

			float percent = 0.2f;
			Vector2 separation = contact.manifold.normal * (contact.manifold.depth / totalInverseMass) * percent;
			contact.bodyA.position = contact.bodyA.position + separation * contact.bodyA.inverseMass;
			contact.bodyB.position = contact.bodyB.position - separation * contact.bodyB.inverseMass;
		}
	}
}

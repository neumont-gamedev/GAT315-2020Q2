using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContactSolver
{
	public static void Resolve(ref List<Contact> contacts, float restingLimit = 0.1f)
	{
		foreach(Contact contact in contacts)
		{
			float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;
			if (totalInverseMass == 0) continue;

			float percent = 0.8f;
			Vector2 separation = contact.manifold.normal * (contact.manifold.depth / totalInverseMass) * percent;
			contact.bodyA.position = contact.bodyA.position + separation * contact.bodyA.inverseMass;
			contact.bodyB.position = contact.bodyB.position - separation * contact.bodyB.inverseMass;

			// relative velocity
			Vector2 relativeVelocity = contact.bodyA.velocity - contact.bodyB.velocity;

			float normalVelocity = Vector3.Dot(relativeVelocity, contact.manifold.normal);
			if (normalVelocity > 0) continue;

			//// velocity from acceleration
			//float velocityFromAcceleration = 0;
			//if ((contact.bodyA.state & PhysicsBody.eState.AWAKE) != 0)
			//{
			//	velocityFromAcceleration += contact.bodyA.prevAcceleration * Time.deltaTime * contact.manifold.normal;
			//}

			float restitution = (contact.bodyA.restitution + contact.bodyB.restitution) * 0.5f;
			restitution = (Mathf.Abs(normalVelocity) < restingLimit) ? 0.0f : restitution;

			//Debug.Log(Mathf.Abs(normalVelocity));

			float impulseMagnitude = (-(1.0f + restitution) * normalVelocity) / totalInverseMass;
			Vector2 impulse = contact.manifold.normal * impulseMagnitude;

			contact.bodyA.ApplyForce(contact.bodyA.velocity + (impulse * contact.bodyA.inverseMass), PhysicsBody.eForceMode.VELOCITY);
			contact.bodyB.ApplyForce(contact.bodyB.velocity - (impulse * contact.bodyB.inverseMass), PhysicsBody.eForceMode.VELOCITY);
		}
	}
}

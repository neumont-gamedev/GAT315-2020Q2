using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsJoint
{
    public PhysicsBody bodyA { get; set; } = null;
    public PhysicsBody bodyB { get; set; } = null;

    public abstract void ApplyForce(float dt);
    public void DebugDraw()
    {
        Debug.DrawLine(bodyA.position, bodyB.position);
    }
}

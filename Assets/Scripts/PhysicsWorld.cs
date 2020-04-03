using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    [HideInInspector] public List<PhysicsBody> bodies = new List<PhysicsBody>();

    void Update()
    {
        foreach(PhysicsBody body in bodies)
        {
            body.force = Vector2.down * 5f;
            body.Step(Time.deltaTime);
        }
    }
}

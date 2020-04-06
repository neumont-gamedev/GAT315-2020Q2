using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    [SerializeField] FloatRef m_gravity = null;

    [HideInInspector] public List<PhysicsBody> bodies = new List<PhysicsBody>();
    
    static public Vector2 gravity { get; set; } = new Vector2(0, -9.81f);
    static public float fixedTimeStep { get; set; } = (1.0f / 60.0f); // 0.016

    float timeAccumulator { get; set; } = 0.0f;

    void Update()
    {
        gravity = new Vector2(0.0f, m_gravity.value);

        timeAccumulator = timeAccumulator + Time.deltaTime;
        while (timeAccumulator > fixedTimeStep)
        {
            bodies.ForEach(body => body.Step(fixedTimeStep));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedTimeStep));

            timeAccumulator = timeAccumulator - fixedTimeStep;
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }
}

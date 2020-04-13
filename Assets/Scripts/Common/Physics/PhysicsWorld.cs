using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    [SerializeField] FloatRef m_gravity = null;
    [SerializeField] FloatRef m_fps = null;
    [SerializeField] BoolRef m_simulate = null;

    [HideInInspector] public List<PhysicsBody> bodies = new List<PhysicsBody>();
    
    static public Vector2 gravity { get; set; } = new Vector2(0, -9.81f);
    static public float fixedTimeStep { get; set; } = (1.0f / 60.0f);

    float timeAccumulator { get; set; } = 0.0f;

    void Update()
    {
        gravity = new Vector2(0.0f, m_gravity.value);
        fixedTimeStep = 1.0f / m_fps.value;

        timeAccumulator = (m_simulate.value) ? timeAccumulator + Time.deltaTime : 0;
        while (timeAccumulator > fixedTimeStep)
        {
            bodies.ForEach(body => body.Step(fixedTimeStep));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedTimeStep));

            timeAccumulator = timeAccumulator - fixedTimeStep;
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public static PhysicsBody GetPhysicsBodyFromPosition(Vector2 position)
    {
        PhysicsBody body = null;

        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider)
        {
            body = hit.collider.gameObject.GetComponent<PhysicsBody>();
        }

        return body;
    }
}

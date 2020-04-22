using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    [SerializeField] FloatRef m_gravity = null;
    [SerializeField] FloatRef m_fps = null;
    [SerializeField] BoolRef m_simulate = null;
    [SerializeField] VectorFieldForce m_vectorField = null;

    [HideInInspector] public List<PhysicsBody> bodies = new List<PhysicsBody>();
    [HideInInspector] public List<PhysicsJoint> joints = new List<PhysicsJoint>();

    static public Vector2 gravity { get; set; } = new Vector2(0, -9.81f);
    static public float fixedTimeStep { get; set; } = (1.0f / 60.0f);

    float timeAccumulator { get; set; } = 0.0f;

    void Update()
    {
        gravity = new Vector2(0.0f, m_gravity.value);
        fixedTimeStep = 1.0f / m_fps.value;

        bodies.ForEach(body => m_vectorField.ApplyForce(body));
        joints.ForEach(joint => joint.ApplyForce(fixedTimeStep));

        timeAccumulator = (m_simulate.value) ? timeAccumulator + Time.deltaTime : 0;
        while (timeAccumulator > fixedTimeStep)
        {
            bodies.ForEach(body => body.Step(fixedTimeStep));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedTimeStep));

            // check collision
            bodies.ForEach(body => body.isTouching = false);
            Collision.CreateContacts(ref bodies, out List<Contact> contacts);
            contacts.ForEach(contact => { contact.bodyA.isTouching = true; contact.bodyB.isTouching = true; });
            ContactSolver.Resolve(ref contacts);

            timeAccumulator = timeAccumulator - fixedTimeStep;
        }
        joints.ForEach(joint => joint.DebugDraw());

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

        bodies.ForEach(body => body.position = WrapPosition(body.position));
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

    static public Vector2 ScreenWorldSize
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 screenSize = Camera.main.ViewportToWorldPoint(topRightCorner) * 2.0f;
            return screenSize;
        }
    }

    static public Vector2 WrapPosition(Vector2 position)
    {
        AABB world = new AABB(Vector2.zero, ScreenWorldSize);

        if (position.x > world.max.x) position.x = world.min.x;
        if (position.x < world.min.x) position.x = world.max.x;
        if (position.y > world.max.y) position.y = world.min.y;
        if (position.y < world.min.y) position.y = world.max.y;

        return position;
    }
}

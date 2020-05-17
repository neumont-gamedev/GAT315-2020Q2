using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    [SerializeField] FloatRef m_gravity = null;
    [SerializeField] FloatRef m_fps = null;
    [SerializeField] BoolRef m_simulate = null;
    [SerializeField] FloatRef m_sleep = null;
    [SerializeField] VectorFieldForce m_vectorField = null;
    [SerializeField] BroadPhaseEnumRef m_broadPhaseType = null;

    [HideInInspector] public List<PhysicsBody> bodies = new List<PhysicsBody>();
    [HideInInspector] public List<PhysicsJoint> joints = new List<PhysicsJoint>();

    List<BroadPhase> m_broadPhase = new List<BroadPhase> { new NullBroadPhase(), new QuadtreeBroadPhase(), new BVHBroadPhase() };
    BroadPhase broadPhase { get; set; } = null;

    static public Vector2 gravity { get; set; } = new Vector2(0, -9.81f);
    static public float fixedTimeStep { get; set; } = (1.0f / 60.0f);
    static public AABB aabb { get; set; }

    float timeAccumulator { get; set; } = 0.0f;

    private void Start()
    {
        aabb = new AABB(Vector2.zero, ScreenWorldSize);
    }

    void Update()
    {
        gravity = new Vector2(0.0f, m_gravity.value);
        fixedTimeStep = 1.0f / m_fps.value;

        broadPhase = m_broadPhase[m_broadPhaseType.index];

        //bodies.ForEach(body => m_vectorField.ApplyForce(body));
        joints.ForEach(joint => joint.ApplyForce(fixedTimeStep));

        timeAccumulator = (m_simulate.value) ? timeAccumulator + Time.deltaTime : 0;
        while (timeAccumulator > fixedTimeStep)
        {
            bodies.ForEach(body => body.Step(fixedTimeStep));

            // integrate
            bodies.ForEach(body => { if ((body.state & PhysicsBody.eState.AWAKE) != 0) Integrator.SemiImplicitEuler(body, fixedTimeStep); });

            // sleep
            bodies.ForEach(body => body.UpdateSleep(m_sleep));

            // collision
            bodies.ForEach(body => body.state &= ~PhysicsBody.eState.COLLIDED);

            broadPhase.Build(aabb, ref bodies);
            Collision.CreateBroadPhaseContacts(broadPhase, ref bodies, out List<Contact> contacts);
            Collision.CreateNarrowPhaseContacts(ref contacts);
            contacts.ForEach(contact =>
            {
                contact.bodyA.state &= PhysicsBody.eState.COLLIDED;
                contact.bodyB.state &= PhysicsBody.eState.COLLIDED;
            });

            PhysicsBody.UpdateAwake(ref contacts);

            // collision resolution
            ContactSolver.Resolve(ref contacts);

            timeAccumulator = timeAccumulator - fixedTimeStep;
        }
        broadPhase.Draw();
        joints.ForEach(joint => joint.DebugDraw());

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);
        bodies.ForEach(body => body.position = WrapWorld(body.position));
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

    public static Vector2 ScreenWorldSize
    {
        get 
        {
            Vector2 worldSize = Camera.main.ViewportToWorldPoint(Vector2.one) * 2;
            return worldSize;             
        }
    }

    public static Vector2 WrapWorld(Vector2 position)
    {
        AABB world = new AABB(Vector2.zero, PhysicsWorld.ScreenWorldSize);

        if (position.x > world.max.x) position.x = world.min.x;
        if (position.x < world.min.x) position.x = world.max.x;
        if (position.y > world.max.y) position.y = world.min.y;
        if (position.y < world.min.y) position.y = world.max.y;

        return position;
    }
}

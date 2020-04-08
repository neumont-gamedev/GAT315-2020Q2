using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField] GameObject m_gameObject = null;
    [SerializeField] PhysicsWorld m_physicsWorld = null;
    [SerializeField] FloatRef m_damping = null;
    [SerializeField] FloatRef m_velocity = null;

    public bool active { get; set; } = false;

    void Update()
    {
        if (active)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject go = Instantiate(m_gameObject, position, Quaternion.identity);
            PhysicsBody body = go.GetComponent<PhysicsBody>();
            body.ApplyForce(Random.insideUnitCircle.normalized * m_velocity.value, PhysicsBody.eForceMode.VELOCITY);
            body.damping = m_damping;

            m_physicsWorld.bodies.Add(body);
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (PhysicsBody body in m_physicsWorld.bodies)
            {
                Vector2 direction = body.position - position;
                if (direction.magnitude <= 4.0f)
                {
                    body.ApplyForce(direction.normalized * (4.0f - direction.magnitude), PhysicsBody.eForceMode.ACCELERATION);
                }
            }

            //Camera.main.backgroundColor = Color.HSVToRGB(Random.value, 1, 1);
        }
    }

    public void StartEvent()
    {
        Debug.Log("start");
        active = true;
    }

    public void StopEvent()
    {
        active = false;
        Debug.Log("stop");
    }
}

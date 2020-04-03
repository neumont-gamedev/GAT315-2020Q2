using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField] GameObject m_gameObject = null;
    [SerializeField] PhysicsWorld m_physicsWorld = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject go = Instantiate(m_gameObject, position, Quaternion.identity);
            PhysicsBody body = go.GetComponent<PhysicsBody>();
            body.velocity = Random.insideUnitCircle.normalized * 4.0f;

            m_physicsWorld.bodies.Add(body);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    public override eType type => eType.CIRCLE;
    public float radius { get { return transform.localScale.x * 0.5f; } set { transform.localScale = Vector3.one * value; } }
    //public float radius { get { return m_radius; } set { m_radius = transform.localScale.x * 0.5f; transform.localScale = Vector3.one * value; } }
    
    public override AABB ComputeAABB(Vector2 position)
    {
        return new AABB(position, Vector2.one * (radius * 2));
    }

    public override AABB ComputeAABB(Vector2 position)
    {
        return new AABB(position, Vector2.one * (radius * 2));
    }

    public override float ComputeMass(float density)
    {
        return (Mathf.PI * (radius * radius)) * density;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    float m_radius;

    public float radius { get { return m_radius; } set { m_radius = value; transform.localScale = Vector2.one * value; } }
    public override eType type => eType.CIRCLE;

    public override float ComputeMass(float density)
    {
        return density * (Mathf.PI * (radius * radius));
    }
}

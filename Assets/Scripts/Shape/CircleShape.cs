using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    public override eType type => eType.CIRCLE;
    public float radius { get { return transform.localScale.x * 0.5f; } set { transform.localScale = Vector3.one * value; } }

    public override float ComputeMass(float density)
    {
        return (Mathf.PI * (radius * radius)) * density;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    public float radius { get { return transform.localScale.magnitude * 0.1f; } set { transform.localScale = Vector2.one * value; } }
    public override eType type => eType.CIRCLE;

    public override float ComputeMass(float density)
    {
        return density * (Mathf.PI * (radius * radius));
    }
}

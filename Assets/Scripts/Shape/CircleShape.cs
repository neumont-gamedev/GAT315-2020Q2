using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    public float radius { get; set; }

    public override eType type => eType.CIRCLE;

    public override float ComputeMass(float density)
    {
        return (Mathf.PI * (radius * radius)) * density;
    }

    private void Update()
    {
        float scale = (m_spriteRenderer.sprite.pixelsPerUnit - m_spriteRenderer.sprite.rect.width) / m_spriteRenderer.sprite.pixelsPerUnit;
        transform.localScale = Vector3.one * (radius * 0.5f) * (Camera.main.orthographicSize * 2.0f) * scale;
        //Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, Color.green);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCell : MonoBehaviour
{
    [SerializeField] Renderer m_renderer = null;
    [SerializeField] Color m_colorA = Color.black;
    [SerializeField] Color m_colorB = Color.white;

    public float density { get; set; } = 0;
    public Vector2 velocity { get; set; } = Vector2.zero;

    public Vector3 size { get; set; } = Vector3.one;
    public int x { get; set; } = 0;
    public int y { get; set; } = 0;

    Material m_material = null;

    void Start()
    {
        m_material = m_renderer.material;
    }

    void Update()
    {
        m_material.color = Color.Lerp(m_colorA, m_colorB, density);
        transform.localScale = size;

        Debug.DrawLine(transform.position, transform.position + (Vector3)velocity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCell : MonoBehaviour
{
    [SerializeField] Renderer m_renderer = null;
    [SerializeField] Color m_colorA = Color.black;
    [SerializeField] Color m_colorB = Color.white;

    public float size { get { return transform.localScale.x; } set { transform.localScale = Vector3.one * value; } }
    public float density { get; set; } = 0.0f;
    public Vector3 velocity { get; set; } = Vector3.zero;

    Material m_material = null;

    void Start()
    {
        m_material = m_renderer.material;
    }

    void Update()
    {
        m_material.color = Color.Lerp(m_colorA, m_colorB, density);
        Debug.DrawLine(transform.position, transform.position + velocity);
    }
}

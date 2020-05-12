using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCellVisual : MonoBehaviour
{
    enum eType
    {
        COLOR,
        SIZE
    }

    [SerializeField] Renderer m_renderer = null;
    [SerializeField] Color m_colorA = Color.black;
    [SerializeField] Color m_colorB = Color.white;
    [SerializeField] eType m_type = eType.COLOR;

    public float density { get; set; } = 0;
    public int x { get; set; } = 0;
    public int y { get; set; } = 0;

    Material m_material = null;

    void Start()
    {
        m_material = m_renderer.material;
    }

    void Update()
    {
        switch (m_type)
        {
            case eType.COLOR:
                m_material.color = Color.Lerp(m_colorA, m_colorB, density);
                break;
            case eType.SIZE:
                transform.localScale = Vector3.one * density;
                break;
            default:
                break;
        }
        
    }
}

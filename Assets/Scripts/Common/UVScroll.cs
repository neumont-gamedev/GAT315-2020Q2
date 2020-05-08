using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    enum eType
    {
        BASE,
        NORMAL
    }

    [SerializeField] Renderer m_renderer = null;
    [SerializeField] eType m_type = eType.BASE;
    [SerializeField] Vector2 m_uvRate = Vector2.zero;
    [SerializeField] bool m_randomize = false;

    Material m_material = null;
    Vector2 m_uv = Vector2.zero;
    int m_propertyID = 0;

    void Start()
    {
        m_material = m_renderer.material;
        m_uv.x = (m_randomize) ? Random.value : 0;
        m_uv.y = (m_randomize) ? Random.value : 0;
        string textureName = (m_type == eType.BASE) ? "_MainTex" : "_BumpMap";
        m_propertyID = Shader.PropertyToID(textureName);
    }
        
    void Update()
    {
        m_uv.x = m_uv.x + Time.deltaTime * m_uvRate.x;
        m_uv.y = m_uv.y + Time.deltaTime * m_uvRate.y;

        m_material.SetTextureOffset(m_propertyID, m_uv);
    }
}

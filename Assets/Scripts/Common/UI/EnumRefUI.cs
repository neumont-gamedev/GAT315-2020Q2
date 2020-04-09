using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnumRefUI : MonoBehaviour
{
    [SerializeField] TMP_Dropdown m_dropdown = null;
    [SerializeField] EnumRef m_enumType = null;

    private void OnValidate()
    {
        if (m_enumType != null)
        {
            name = m_enumType.name;

            m_dropdown.ClearOptions();
            string[] names = m_enumType.names;
            m_dropdown.AddOptions(new List<string>(names));
        }
    }

    private void Start()
    {
        m_dropdown.value = m_enumType.index;
        m_dropdown.onValueChanged.AddListener(IndexChanged);
    }

    public void IndexChanged(int index)
    {
        m_enumType.index = index;
    }
}

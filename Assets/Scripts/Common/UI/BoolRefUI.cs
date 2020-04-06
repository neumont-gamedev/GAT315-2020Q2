using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoolRefUI : MonoBehaviour
{
    [SerializeField] Toggle m_toggle = null;
    [SerializeField] TMP_Text m_textLabel = null;

    [SerializeField] BoolRef m_value = null;

    private void OnValidate()
    {
        if (m_value != null)
        {
            name = m_value.name;
            m_textLabel.text = name;
        }
    }

    private void Start()
    {
        m_toggle.onValueChanged.AddListener(UpdateValue);
    }

    void Update()
    {
        m_toggle.isOn = m_value.value;
    }

    void UpdateValue(bool uiValue)
    {
        m_value.value = uiValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntRefUI : MonoBehaviour
{
    [SerializeField] Slider m_slider = null;
    [SerializeField] TMP_Text m_textLabel = null;
    [SerializeField] TMP_Text m_textValue = null;
    [SerializeField] int m_min = 0;
    [SerializeField] int m_max = 1;

    [SerializeField] IntRef m_value = null;

    private void OnValidate()
    {
        if (m_value != null)
        {
            name = m_value.name;
            m_textLabel.text = name;
        }
        m_slider.minValue = m_min;
        m_slider.maxValue = m_max;
    }

    private void Start()
    {
        m_slider.onValueChanged.AddListener(UpdateValue);
    }

    void Update()
    {
        m_slider.value = m_value.value;
        m_textValue.text = m_value.value.ToString();
    }

    void UpdateValue(float sliderValue)
    {
        m_value.value = (int)sliderValue;
    }
}

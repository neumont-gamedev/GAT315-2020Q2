using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] Action[] m_actions = null;
    [SerializeField] ActionEnumRef m_action = null;

    public void StartEvent()
    {
        m_actions[m_action.index].StartEvent();
    }

    public void StopEvent()
    {
        m_actions[m_action.index].StopEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] Action m_action = null;

    public void StartEvent()
    {
        m_action.StartEvent();
    }

    public void StopEvent()
    {
        m_action.StopEvent();
    }
}

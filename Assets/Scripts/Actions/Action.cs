using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
	[SerializeField] protected PhysicsWorld m_physicsWorld = null;

	public abstract void StartEvent();
	public abstract void StopEvent();
}

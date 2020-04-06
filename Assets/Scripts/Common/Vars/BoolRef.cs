using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bool", menuName = "Variables/Bool")]
public class BoolRef : ScriptableObject
{
	[SerializeField] bool m_value;

	public bool value { get => m_value; set => m_value = value; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Int", menuName = "Variables/Int")]
public class IntRef : ScriptableObject
{
	[SerializeField] int m_value;

	public int value { get => m_value; set => m_value = value; }
}

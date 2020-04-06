using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Float", menuName = "Variables/Float")]
public class FloatRef : ScriptableObject
{
	[SerializeField] float m_value;

	public float value { get => m_value; set => m_value = value; }
}

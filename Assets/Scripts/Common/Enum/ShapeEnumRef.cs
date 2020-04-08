using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape", menuName = "Variables/Enum/Shape")]
public class ShapeEnumRef : EnumRef
{
	public enum	eType
	{
		Circle,
		Box
	}
	[SerializeField] eType m_type;

	public eType type { get => m_type; set => m_type = value; }

	public override string id { get { return type.ToString(); } }
	public override int index { get { return (int)type; } set { type = (eType)value; } }
	public override string[] names { get { return Enum.GetNames(typeof(eType)); } }

}

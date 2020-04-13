using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Variables/Enum/Action")]
public class ActionEnumRef : EnumRef
{
	public enum	eType
	{
		Create,
		Select,
		Delete,
		Joint
	}
	[SerializeField] eType m_type;

	public eType type { get => m_type; set => m_type = value; }

	public override string id { get { return type.ToString(); } }
	public override int index { get { return (int)type; } set { type = (eType)value; } }
	public override string[] names { get { return Enum.GetNames(typeof(eType)); } }

}

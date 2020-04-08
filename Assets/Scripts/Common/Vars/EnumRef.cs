using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumRef : ScriptableObject
{
	public virtual string id { get; }
	public virtual int index { get; set; }

	public virtual string[] names { get; }
}

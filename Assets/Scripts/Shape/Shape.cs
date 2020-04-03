using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
	public enum eType
	{
		CIRCLE,
		BOX
	}

	public abstract eType type { get; }

	public abstract float ComputeMass(float density);
}

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
	public float density { get; set; } = 1.0f;

	public abstract float ComputeMass(float density);

}

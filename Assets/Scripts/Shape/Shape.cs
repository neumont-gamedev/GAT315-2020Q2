using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
	public enum eType
	{
		POINT,
		CIRCLE,
		BOX
	}

	public abstract eType type { get; }
	public abstract float ComputeMass(float density);

	public float friction { get; set; } = 1.0f; // 0.0 - 1.0
	public float restitution { get; set; } = 1.0f; // 0.0 - 1.0
	public float density { get; set; } = 1.0f;
}

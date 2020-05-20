using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VectorField", menuName = "Physics/Data/VectorField")]
public class VectorFieldData : ScriptableObject
{
	[Range(0.5f, 2)] public float gridScale;
	[Range(1, 10)] public float noiseOffset;
	[Range(0.01f, 1)] public float noiseScale;
}

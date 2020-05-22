using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceEffector", menuName = "Physics/Data/ForceEffector")]
public class ForceEffectorData : ScriptableObject
{
	[Range(-10, 10)] public float magnitude;
	[Range(0, 1)] public float drag;
}

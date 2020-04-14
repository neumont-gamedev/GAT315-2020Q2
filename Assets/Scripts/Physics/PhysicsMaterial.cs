using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsMaterial", menuName = "Physics/Material")]
public class PhysicsMaterial : ScriptableObject
{
	public float friction = 0.5f;
	public float coefficientOfRestitution = 0.5f;
}

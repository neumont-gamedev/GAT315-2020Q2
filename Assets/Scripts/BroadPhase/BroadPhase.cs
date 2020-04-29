﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BroadPhase
{
	public abstract void Build(AABB aabb, ref List<PhysicsBody> bodies);
	public abstract void Query(AABB aabb, ref List<PhysicsBody> bodies);
	public abstract void Query(PhysicsBody body, ref List<PhysicsBody> bodies);
	public abstract void Draw();

	public static int NumberOfTests { get; set; } = 0;
}

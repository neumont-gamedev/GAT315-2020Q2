using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KinematicSegment : MonoBehaviour
{
	public KinematicSegment parent { get; set; }

	public Vector2 start { get { return transform.position; } set { transform.position = value; } }
	public Vector2 end { get; set; }

	public float length { get { return m_polar.length; } set { m_polar.length = value; } }
	public float angle { get { return m_polar.angle; } set { m_polar.angle = value; } }
	public float width { get; set; }

	Coordinate.Polar m_polar;

	public abstract void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width);

	public void CalculateEnd()
	{
		Vector2 offset = Coordinate.PolarToCartesian(m_polar);
		end = start + offset;
	}
}

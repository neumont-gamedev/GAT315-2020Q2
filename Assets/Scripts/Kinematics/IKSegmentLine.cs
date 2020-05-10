using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSegmentLine
{
	public Vector2 start { get; set; }
	public Vector2 end { get; set; }
	public IKSegmentLine parent { get; set; }

	public float length { get { return m_polar.length; } set { m_polar.length = value; } }
	
	Coordinate.Polar m_polar;

	public void Update()
	{
		Debug.DrawLine(start, end, Color.white);
	}

	public void Initialize(IKSegmentLine parent, float angle, float length)
	{
		this.parent = parent;
		m_polar.angle = angle;
		m_polar.length = length;
	}

	public void Follow(Vector2 target)
	{
		Vector2 direction = target - start;
		Coordinate.Polar polar = Coordinate.CartesianToPolar(direction);
		m_polar.angle = polar.angle;

		start = target - (direction.normalized * m_polar.length);
	}

	public void CalculateEnd()
	{
		Vector2 v2 = Coordinate.PolarToCartesian(m_polar);
		end = start + v2;
	}
}


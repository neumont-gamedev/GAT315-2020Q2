using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSegment : MonoBehaviour
{
	public Vector2 start { get { return transform.position; } set { transform.position = value; } }
	public Vector2 end { get; set; }
	public IKSegment parent { get; set; }

	public float length { get { return m_polar.length; } set { m_polar.length = value; } }
	public float width { get; set; }

	Coordinate.Polar m_polar;
	Transform m_childTransform;

	private void Start()
	{
		m_childTransform = transform.GetChild(0);
	}

	private void Update()
	{
		Vector3 position = Vector3.right * (m_polar.length * 0.5f);
		m_childTransform.localPosition = position;
		m_childTransform.localScale = new Vector3(m_polar.length, width, 1.0f);

		transform.rotation = Quaternion.AngleAxis(m_polar.angle * Mathf.Rad2Deg, Vector3.forward);
	}

	public void Initialize(IKSegment parent, float angle, float length, float width)
	{
		this.parent = parent;
		m_polar.angle = angle;
		m_polar.length = length;
		this.width = width;
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematicSegment : KinematicSegment
{
	[SerializeField] [Range(-90.0f, 90.0f)] float m_angle = 0.0f;
	[SerializeField] bool m_enableNoise = false;

	public float baseAngle { get; set; }
	public float noiseRate { get; set; } = 1.0f;
	
	float m_noise;

	private void Start()
	{
		m_noise = Random.value * 100.0f;
	}

	private void Update()
	{
		transform.localScale = Vector3.one * width;
		float localAngle = m_angle;

		if (m_enableNoise)
		{
			m_noise = m_noise + (noiseRate * Time.deltaTime);
			float t = Mathf.PerlinNoise(m_noise, 0);
			localAngle = Mathf.Lerp(-90, 90, t);
		}

		angle = (parent != null) ? (localAngle + parent.angle) : (localAngle + baseAngle);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width)
	{
		this.parent = parent;
		this.width = width;

		this.angle = angle;
		this.length = length;

		start = position;
		baseAngle = angle;
	}


}

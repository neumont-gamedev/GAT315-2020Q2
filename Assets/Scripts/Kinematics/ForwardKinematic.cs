using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematic : MonoBehaviour
{
	[SerializeField] ForwardKinematicSegment m_segment = null;
	[SerializeField] [Range(1, 40)] int m_count = 5;
	[SerializeField] [Range(0.0f, 10.0f)] float m_length = 2.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_width = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_noiseRate = 1.0f;

	List<ForwardKinematicSegment> segments { get; set; } = new List<ForwardKinematicSegment>();

	void Awake()
	{
		Vector2 position = transform.position;
		float angle = transform.eulerAngles.z;

		// base -> end
		ForwardKinematicSegment parent = null;
		for (int i = 1; i < m_count; i++)
		{
			ForwardKinematicSegment segment = Instantiate<ForwardKinematicSegment>(m_segment, transform);
			segment.Initialize(parent, position, angle, m_length, m_width);
			segments.Add(segment);

			parent = segment;
		}
	}

	private void Update()
	{
		float noiseRate = m_noiseRate;
		int count = 0;
		foreach (ForwardKinematicSegment segment in segments)
		{
			segment.length = m_length;
			segment.width = m_width;
			
			noiseRate = Mathf.Lerp(0, m_noiseRate, ((float)count / (float)segments.Count));
			count++;
			segment.noiseRate = noiseRate;

			if (segment.parent != null)
			{
				segment.start = segment.parent.end;
			}
			segment.CalculateEnd();
		}
	}
}

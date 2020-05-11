using System.Collections.Generic;
using UnityEngine;

public class InverseKinematic : MonoBehaviour
{
	[SerializeField] InverseKinematicSegment m_segment = null;
	[SerializeField] [Range(1, 40)] int m_count = 5;
	[SerializeField] [Range(0.0f, 10.0f)] float m_length = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_width = 1.0f;

	[SerializeField] [Range(0.0f, 10.0f)] float m_waveTime = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_waveAmplitude = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_waveOffset = 1.0f;

	[SerializeField] Transform m_target = null;
	[SerializeField] Transform m_base = null;

	List<InverseKinematicSegment> segments { get; set; } = new List<InverseKinematicSegment>();

	void Start()
	{
		// end -> base
		InverseKinematicSegment parent = null;
		for (int i = 0; i < m_count; i++)
		{
			InverseKinematicSegment segment = Instantiate(m_segment, transform);
			segment.Initialize(parent, Vector2.zero, 0, m_length, m_width);
			segments.Add(segment);

			parent = segment;
		}
	}

	void Update()
	{
		int count = 1;
		foreach (InverseKinematicSegment segment in segments)
		{
			float time = Time.time * m_waveTime;
			float offset = ((float)count / (float)segments.Count) * m_waveOffset;
			float wave = 1.0f + ((Mathf.Sin(time + offset) + 1) * 0.5f);
			count++;

			segment.length = m_length;
			segment.width = m_width * wave * m_waveAmplitude;

			Vector2 target = (segment.parent) ? segment.parent.start : (Vector2)m_target.position;
			segment.Follow(target);
		}

		if (m_base)
		{
			// set base position
			segments[segments.Count - 1].start = m_base.transform.position;
			segments[segments.Count - 1].CalculateEnd();

			// set (base + i) position -> end
			for (int i = segments.Count - 2; i >= 0; i--)
			{
				segments[i].start = segments[i + 1].end;
				segments[i].CalculateEnd();

				Debug.DrawLine(segments[i].start, segments[i].end, Color.white);
			}
		}
	}
}

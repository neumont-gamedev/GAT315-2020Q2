using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
	[SerializeField] IKSegment m_segment = null;
	[SerializeField] [Range(1, 40)] int m_count = 5;
	[SerializeField] [Range(0.0f, 10.0f)] float m_length = 2.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_width = 1.0f;

	[SerializeField] [Range(0.0f, 5.0f)] float m_pulseAmplitude = 1.0f;
	[SerializeField] [Range(0.0f, 5.0f)] float m_pulseRate = 1.0f;
	[SerializeField] [Range(0.0f, 5.0f)] float m_pulseCycle = 1.0f;

	[SerializeField] GameObject m_target = null;
	[SerializeField] GameObject m_base = null;

	List<IKSegment> m_segments = null;

	void Start()
	{
		m_segments = new List<IKSegment>();

		// head/tip -> base
		IKSegment parent = null;
		for (int i = 0; i < m_count; i++)
		{
			IKSegment segment = Instantiate<IKSegment>(m_segment, transform);
			segment.Initialize(parent, 0.0f, m_length, m_width);
			m_segments.Add(segment);

			parent = segment;
		}
	}

	void Update()
	{
		float t = Time.time * m_pulseRate;
		//float offset = 0.0f;
		foreach (IKSegment segment in m_segments)
		{
			segment.length = m_length;
			segment.width = m_width;// + (Mathf.Sin(t + offset) + 1.0f) * 0.5f * m_pulseAmplitude;
			//offset = offset + m_pulseCycle;

			Vector2 target = (segment.parent) ? segment.parent.start : (Vector2)m_target.transform.position;
			segment.Follow(target);
		}

		if (m_base)
		{
			m_segments[m_segments.Count - 1].start = m_base.transform.position;
			m_segments[m_segments.Count - 1].CalculateEnd();

			for (int i = m_segments.Count - 2; i >= 0; i--)
			{
				m_segments[i].start = m_segments[i + 1].end;
				m_segments[i].CalculateEnd();
			}
		}
	}
}

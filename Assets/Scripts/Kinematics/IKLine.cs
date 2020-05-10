using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLine : MonoBehaviour
{
	[SerializeField] [Range(1, 40)] int m_count = 5;
	[SerializeField] [Range(0.0f, 10.0f)] float m_length = 2.0f;

	[SerializeField] GameObject m_target = null;
	[SerializeField] GameObject m_base = null;

	List<IKSegmentLine> m_segments = null;

	void Start()
	{
		m_segments = new List<IKSegmentLine>();

		// head/tip -> base
		IKSegmentLine parent = null;
		for (int i = 0; i < m_count; i++)
		{
			IKSegmentLine segment = new IKSegmentLine();
			segment.Initialize(parent, 0.0f, m_length);
			m_segments.Add(segment);

			parent = segment;
		}
	}

	void Update()
	{
		foreach (IKSegmentLine segment in m_segments)
		{
			segment.length = m_length;

			Vector2 target = (segment.parent != null) ? segment.parent.start : (Vector2)m_target.transform.position;
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

		foreach (IKSegmentLine segment in m_segments)
		{
			segment.Update();
		}
	}
}

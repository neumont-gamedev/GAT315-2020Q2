using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematic : MonoBehaviour
{
	[SerializeField] InverseKinematicSegment m_segment = null;
	[SerializeField] [Range(1, 40)] int m_count = 5;
	[SerializeField] [Range(0.0f, 10.0f)] float m_length = 2.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_width = 1.0f;

	[SerializeField] Transform m_target = null;
	[SerializeField] Transform m_base = null;

	List<InverseKinematicSegment> segments { get; set; } = new List<InverseKinematicSegment>();

	void Awake()
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

	private void Update()
	{
		foreach (InverseKinematicSegment segment in segments)
		{
			segment.length = m_length;
			segment.width = m_width;

			Vector2 target = (segment.parent) ? segment.parent.start : (Vector2)m_target.position;
			segment.Follow(target);
		}

		if (m_base)
		{
			segments[segments.Count - 1].start = m_base.position;
			segments[segments.Count - 1].CalculateEnd();

			for (int i = segments.Count - 2; i >= 0; i--)
			{
				segments[i].start = segments[i + 1].end;
				segments[i].CalculateEnd();
			}
		}
	}
}

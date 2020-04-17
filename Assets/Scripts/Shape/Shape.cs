using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
	public enum eType
	{
		CIRCLE,
		BOX
	}

	public abstract eType type { get; }
	public float density { get; set; } = 1.0f;
	public Color color { get { return m_spriteRenderer.material.color; } set { m_spriteRenderer.color = value; } }

	public abstract float ComputeMass(float density);

	protected SpriteRenderer m_spriteRenderer = null;

	private void Start()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}

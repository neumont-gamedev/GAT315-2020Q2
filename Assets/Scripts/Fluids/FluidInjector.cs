using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidInjector : MonoBehaviour
{
	[SerializeField] [Range(0, 100.0f)] float m_density = 1.0f;
	[SerializeField] Fluid m_fluid = null;

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float density = (Input.GetButton("Fire1")) ? m_density : 0;
		Vector2 velocity = (Input.GetButton("Fire2")) ? new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 20.0f : Vector2.zero;

		m_fluid.Inject(ray, density, velocity);
	}
}

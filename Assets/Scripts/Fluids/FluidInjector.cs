using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidInjector : MonoBehaviour
{
	[SerializeField] [Range(0, 20)] float m_density = 1;
	[SerializeField] [Range(0, 20)] float m_velocity = 1;
	[SerializeField] Fluid m_fluid = null;

	void FixedUpdate()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float density = (Input.GetButton("Fire1")) ? m_density : 0;
		Vector2 velocity = (Input.GetButton("Fire2")) ? new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * m_velocity : Vector2.zero;

		m_fluid.Inject(ray, density, velocity);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidInjector : MonoBehaviour
{
	[SerializeField] [Range(0, 1.0f)] float m_density = 1.0f;
	[SerializeField] Fluid m_fluid = null;

	void Update()
	{
		bool apply = Input.GetButton("Fire1");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		m_fluid.Inject(ray, m_density, apply);
	}
}

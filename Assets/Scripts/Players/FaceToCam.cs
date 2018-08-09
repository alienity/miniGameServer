using UnityEngine;
using System.Collections;

public class FaceToCam : MonoBehaviour
{

	public Camera m_Camera;
	public bool x = true;
	public bool y = true;
	public bool z = true;

	void Start ()
	{
		m_Camera = Camera.allCameras [0];
	}

	void Update ()
	{
		Vector3 faceToCamVector = (m_Camera.transform.position - transform.position).normalized;
		if (!x)
			faceToCamVector.x = transform.forward.x;
		if (!y)
			faceToCamVector.y = transform.forward.y;
		if (!z)
			faceToCamVector.z = transform.forward.z;
		transform.forward = -faceToCamVector;
	}

}

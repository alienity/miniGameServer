using UnityEngine;
using System.Collections;

public class StraightToCamera : MonoBehaviour
{

	private Camera m_Camera;

	void Start ()
	{
		m_Camera = Camera.allCameras [0];
	}

	void Update ()
	{
		transform.forward = m_Camera.transform.forward;
	}

}

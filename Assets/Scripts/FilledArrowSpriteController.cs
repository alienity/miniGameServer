using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledArrowSpriteController : MonoBehaviour
{
	private Material targetMaterial = null;
	private float progress = 0.0f;
	// Use this for initialization
	private void Awake()
	{
		if (targetMaterial == null)
		{
			targetMaterial = GetComponent<SpriteRenderer>().material;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		progress = Mathf.Clamp01(progress);
		targetMaterial.SetFloat("_Cutoff",1- progress);
	}

	public void SetProgress(float progress)
	{
		this.progress = progress;
	}
}

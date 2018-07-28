using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartOnclick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button prepareButton = GetComponent<Button>();
		prepareButton.onClick.AddListener(delegate
		{
			Server.Instance.stage = Stage.StartStage;
			SceneTransformer.Instance.TransferToNextScene();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

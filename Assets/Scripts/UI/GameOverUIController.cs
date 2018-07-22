using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour {

    private DataSaveController dataSaveController;

    public List<Text> groupScores;

	// Use this for initialization
	void Start () {
        dataSaveController = DataSaveController.Instance;
        ShowScores();
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    private void ShowScores()
    {
        int groupNumbers = dataSaveController.playerNumber / 2;
        for (int i = 0; i < groupNumbers; ++i)
        {
            groupScores[i].gameObject.SetActive(true);
            groupScores[i].text = dataSaveController.scores[i].ToString();
        }
    }
    
}

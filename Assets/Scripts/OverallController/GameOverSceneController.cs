using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSceneController : MonoBehaviour {

    // 数据控制器
    private DataSaveController dataSaveController;
    // UI控制器
    private GameOverUIController gOverUIController;
    // Use this for initialization
    void Start () {
        dataSaveController = DataSaveController.Instance;
        gOverUIController = GameOverUIController.Instance;
        if (dataSaveController != null && gOverUIController != null && gOverUIController.playerNameScore!=null)
            ShowScoresAndWinnerICone();
    }

    private void ShowScoresAndWinnerICone()
    {
        int groupNumbers = dataSaveController.playerNumber / 2;
        Dictionary<string, int> nameScoreDic = new Dictionary<string, int>();
        int winnerId = 0;
        int winnerScores = 0;
        for (int i = 0; i < groupNumbers; ++i)
        {
            // TODO:替换成真实用户名
            nameScoreDic.Add("P" + i.ToString() + "," + "P" + (i + 1).ToString(), dataSaveController.scores[i]);
            if (winnerScores < dataSaveController.scores[i])
            {
                winnerScores = dataSaveController.scores[i];
                winnerId = i;
            }
        }
        List<KeyValuePair<string, int>> lst = new List<KeyValuePair<string, int>>(nameScoreDic);
        // Sort
        lst.Sort(delegate (KeyValuePair<string, int> s1, KeyValuePair<string, int> s2)
        {
            return s2.Value.CompareTo(s1.Value);
        });

        for (int i = 0; i < groupNumbers; ++i)
        {
            gOverUIController.playerNameScore[i].name.text = lst[i].Key.ToString();
            gOverUIController.playerNameScore[i].score.text = lst[i].Value.ToString() + "分";
        }

        // TODO:展示winner的ICone
        gOverUIController.winnerImage.color = dataSaveController.groupColor[winnerId];
    }

}

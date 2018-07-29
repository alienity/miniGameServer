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
        /**  测试代码
         
        gOverUIController = GameOverUIController.Instance;
        testShowScoresAndWinnerICone();
        */
        /**  屏蔽真实场景中的代码*/
        dataSaveController = DataSaveController.Instance;
        gOverUIController = GameOverUIController.Instance;
        if (dataSaveController != null && gOverUIController != null && gOverUIController.playerNameScore != null)
            ShowScoresAndWinnerICone();

    }
    
    IEnumerator CountDownToPrepareStage(int time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            --time;
        }
        //server.StopBroadCast();
        Server.Instance.stage = Stage.Prepare;
        SceneTransformer.TransferScene("ChooseRoleScene");
    }

    private void ShowScoresAndWinnerICone()
    {
        int groupNumbers = dataSaveController.playerNumber / 2;
        Dictionary<int, int> nameScoreDic = new Dictionary<int, int>();
        int winnerId = 0;
        int winnerScores = 0;
        for (int i = 0; i < groupNumbers; ++i)
        {
            // TODO:替换成真实用户名
            nameScoreDic.Add(i, dataSaveController.scores[i]);
            if (winnerScores < dataSaveController.scores[i])
            {
                winnerScores = dataSaveController.scores[i];
                winnerId = i;
            }
        }
        List<KeyValuePair<int, int>> lst = new List<KeyValuePair<int, int>>(nameScoreDic);
        // Sort
        lst.Sort(delegate (KeyValuePair<int, int> s1, KeyValuePair<int, int> s2)
        {
            return s2.Value.CompareTo(s1.Value);
        });

        gOverUIController.gameResultsDisplay(lst, groupNumbers);
    }

    // 测试代码
    private void testShowScoresAndWinnerICone()
    {
        int groupNumbers = 4;
        Dictionary<int, int> nameScoreDic = new Dictionary<int, int>();
        int winnerId = 0;
        int winnerScores = 0;
        for (int i = 0; i < groupNumbers; ++i)
        {
            // TODO:替换成真实用户名
            nameScoreDic.Add(i, Random.Range(80, 1000));
            if (winnerScores < nameScoreDic[i])
            {
                winnerScores = nameScoreDic[i];
                winnerId = i;
            }
        }
        List<KeyValuePair<int, int>> lst = new List<KeyValuePair<int, int>>(nameScoreDic);
        // Sort
        lst.Sort(delegate (KeyValuePair<int, int> s1, KeyValuePair<int, int> s2)
        {
            return s2.Value.CompareTo(s1.Value);
        });

        gOverUIController.gameResultsDisplay(lst, groupNumbers);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        
        // 将阶段设为 gameover阶段, todo 发送gameover指令后，客户端会主动断开连接，后续就发送不了信息了，所以手机需要主动计时
        NetworkServer.SendToAll(CustomMsgType.Stage, new StageTransferMsg(Stage.GameOverStage));
        DataSaveController.Instance.stage = Stage.GameOverStage;
        StartCoroutine(CountDownToPrepareStage(10));
    }
    
    IEnumerator CountDownToPrepareStage(int time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            --time;
        }
        //server.StopBroadCast();
        DataSaveController.Instance.RestartRoom();
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

        gOverUIController.gameResultsDisplay(lst, groupNumbers, dataSaveController.role2session, dataSaveController.session2name);
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

        gOverUIController.gameResultsDisplay(lst, groupNumbers, dataSaveController.role2session, dataSaveController.session2name);
    }

}

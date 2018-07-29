using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour {

    public static WorldController Instance { get; private set; }

    // UI控制器
    public GameUIController gUIController;
    // 分数控制器
    public ScoreController scoreController;
    // 组管理和命令解析
    public GroupAndCmdManager gcManager;
    // TODO:将要修改 4个出生点
    public List<Transform> bornTrans;

    // 将被传入的玩家数
    public int groupCounts;
    //// 下一个场景的名字，用于切换场景
    //public string nextSceneName;

    // 计时器，以秒计
    public float totalGameTime = 180;
    // 游戏已经进行时长
    private float totalPastGameTime = 0;
    // 游戏已经结束了
    private bool isGameOver = false;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    private void Start ()
    {
        if (gUIController == null)
            gUIController = FindObjectOfType<GameUIController>();
        if (scoreController == null)
            scoreController = FindObjectOfType<ScoreController>();
        if (gcManager == null)
            gcManager = FindObjectOfType<GroupAndCmdManager>();

        // ********************测试生成角色代码*********************
        InstanceAllGroups();
        // ********************测试生成角色代码*********************
        // 游戏初始化游戏计时
        totalPastGameTime = 0;
    }
    
    private void Update () {

        if (isGameOver) return;

        // 计数游戏时长
        if(totalPastGameTime < totalGameTime)
            totalPastGameTime += Time.deltaTime;
        else
            GameOver();

        // 检查是否死亡并重生
        gcManager.CheckDeathAndReborn(bornTrans);

        // 更新UI
        UpdateUIParams();

    }

    // 更新所有的UI界面
    public void UpdateUIParams()
    {
        for (int gId = 0; gId < groupCounts; ++gId)
        {
            float penguRemainColdingTime = gcManager.groupPlayers[gId].RemainCoolingTime(GroupPlayer.PlayerType.PENGU);
            float pigRemainColdingTime = gcManager.groupPlayers[gId].RemainCoolingTime(GroupPlayer.PlayerType.PIG);
            gUIController.UpdateColdingTime(gId, (int)GroupPlayer.PlayerType.PENGU, penguRemainColdingTime);
            gUIController.UpdateColdingTime(gId, (int)GroupPlayer.PlayerType.PIG, pigRemainColdingTime);
            scoreController.GetScore(gId);
        }
        gUIController.UpdateRemainTimes(totalGameTime - totalPastGameTime);
    }

    // 结束游戏，并终止所有角色控制和效果影响
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;


        
        gUIController.blackPanel.gameObject.SetActive(true);
        //gcManager.StopAllProcess();

        // 加载下一个场景
        //SceneManager.LoadSceneAsync(nextSceneName);
        /*
         * 游戏结束，发送消息给客户端游戏结束，并且清空数据
         */
        NetworkServer.SendToAll(CustomMsgType.Stage, new StageTransferMsg(Stage.GameOverStage));
        Server.Instance.role2connectionID.Clear();
        Server.Instance.connectionID2role.Clear();
        Server.Instance.session2connection.Clear();
        Server.Instance.connection2session.Clear();
        Server.Instance.connections.Clear();
        Server.Instance.kownSessions.Clear();
        SceneTransformer.Instance.TransferToNextScene();
    }

    // 重启游戏
    public void RestartGame()
    {
        isGameOver = false;
        totalPastGameTime = 0;
    }

    // 实例化组对象
    public void InstanceAllGroups()
    {
        groupCounts = Mathf.CeilToInt(DataSaveController.Instance.playerNumber / 2.0f);
        if (groupCounts <= 0) return;
        for (int i = 0; i < groupCounts; ++i)
        {
            gcManager.AddPlayerGroup(i, bornTrans[i], DataSaveController.Instance.groupColor[i]);
        }
        scoreController.SetGroupPlayers(gcManager.groupPlayers);
        gUIController.SetGroupInitial(groupCounts);
    }

    //// 实例化组对象
    //public void InstanceAllGroups(int gps)
    //{
    //    groupCounts = gps;
    //    for (int i = 0; i < groupCounts; ++i)
    //    {
    //        gcManager.AddPlayerGroup(i, bornTrans[i]);
    //    }
    //    scoreController.SetGroupPlayers(gcManager.groupPlayers);
    //    gUIController.SetGroupInitial(groupCounts);
    //}

    // 返回游戏剩余时长
    public float GetRemainGameTime()
    {
        float remainGameTime = totalGameTime - totalPastGameTime;
        return remainGameTime > 0 ? remainGameTime : 0;
    }

}

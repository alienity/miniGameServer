using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Instance = this;
    }
    
    private void Start ()
    {
        if (gUIController == null)
            gUIController = GameUIController.Instance;

        // ********************测试生成角色代码*********************
        InstanceAllGroups(bornTrans.Count);
        // ********************测试生成角色代码*********************
        // 游戏初始化游戏计时
        totalPastGameTime = 0;
    }
    
    void Update () {

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
            float penguRemainColdingTime = gcManager.groupPlayers[gId].penguPlayer.RemainingColdingTime();
            float pigRemainColdingTime = gcManager.groupPlayers[gId].pigPlayer.RemainingColdingTime();
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
        SceneTransformer.Instance.TransferToNextScene();
    }

    // 重启游戏
    public void RestartGame()
    {
        isGameOver = false;
        totalPastGameTime = 0;
    }

    // 实例化组对象
    public void InstanceAllGroups(int gps)
    {
        groupCounts = gps;
        for (int i = 0; i < groupCounts; ++i)
        {
            gcManager.AddPlayerGroup(i, bornTrans[i]);
        }
        scoreController.SetGroupPlayers(gcManager.groupPlayers);
        gUIController.SetGroupInitial(groupCounts);
    }

    // 返回游戏剩余时长
    public float GetRemainGameTime()
    {
        float remainGameTime = totalGameTime - totalPastGameTime;
        return remainGameTime > 0 ? remainGameTime : 0;
    }

}

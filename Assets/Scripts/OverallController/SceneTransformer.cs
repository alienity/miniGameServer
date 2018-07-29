using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneTransformer : MonoBehaviour
{

    public static SceneTransformer Instance { get; private set; }

    public bool trans = false;

    // 下一个要切换的场景名
    public string nextSceneName;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (trans)
            TransferToNextScene();
    }

    public void AskClientToChange()
    {
        string curSceneName = SceneManager.GetActiveScene().name;
        SceneTransferMsg stm = new SceneTransferMsg(curSceneName, nextSceneName);
        NetworkServer.SendToAll(CustomMsgType.ClientChange, stm);
    }

    public void TransferToNextScene()
    {
        AskClientToChange();
        FreeResource();
        SceneManager.LoadScene(nextSceneName);
        /*
         * 切换到游戏场景是将stage设置为 GammingStage，记录当前场景
         */
        if (nextSceneName == "GameScene")
        {
            Server.Instance.stage = Stage.GammingStage;
        }
    }

    // 释放没用的资源，手动GC
    private static void FreeResource()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
    }

    public static void TransferScene(string sceneName)
    {
        FreeResource();
        SceneManager.LoadScene(sceneName);
    }

}

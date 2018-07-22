using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransformer : MonoBehaviour
{

    public static SceneTransformer Instance { get; private set; }

    // 下一个要切换的场景名
    public string nextSceneName;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransferToNextScene()
    {
        FreeResource();
        SceneManager.LoadSceneAsync(nextSceneName);
    }

    // 释放没用的资源，手动GC
    private void FreeResource()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
    }

}

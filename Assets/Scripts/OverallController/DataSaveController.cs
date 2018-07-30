using System.Collections.Generic;
using UnityEngine;

public class DataSaveController : MonoBehaviour {

    public static DataSaveController Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<DataSaveController>();

            if (instance != null)
                return instance;

            GameObject dataSaveController = new GameObject("DataSaveController");
            instance = dataSaveController.AddComponent<DataSaveController>();

            return instance;
        }
    }

    protected static DataSaveController instance;

    // 记录所有要在不同使用的数据

    // *********在选人场景下**********
    // 玩家数
    public int playerNumber;
    // TODO
    public List<string> playerName;
    public List<Color> groupColor;
    // *********在游戏场景下**********
    // 每个队伍的
    public List<int> scores;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    // 清除所有的分数
    public void CLearSocres()
    {
        scores.Clear();
    }

}

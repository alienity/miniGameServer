using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class DataSaveController : MonoBehaviour {

    public static DataSaveController Instance { get; private set; }

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
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



}

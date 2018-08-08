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

    public Stage stage = Stage.Prepare;

    
    // 记录所有要在不同使用的数据
    
    // connectionId到gId和uId的映射表, roleId = gId * 2 + uId
    public Dictionary<int, int> role2connectionID = new Dictionary<int, int>();
    public Dictionary<int, int> connectionID2role = new Dictionary<int, int>();
    public Dictionary<int, int> session2connection = new Dictionary<int, int>();
    public Dictionary<int, int> connection2session = new Dictionary<int, int>();
    public Dictionary<int, int> session2role = new Dictionary<int, int>();
    public HashSet<int> sessionIsConfirmed = new HashSet<int>();
    public Dictionary<int, int> session2confirm = new Dictionary<int, int>();
    public HashSet<int> kownSessions = new HashSet<int>();
    public Dictionary<int, string> session2name = new Dictionary<int, string>();

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
    
    public void ClearData()
    {
        role2connectionID.Clear();
        connectionID2role.Clear();
        session2connection.Clear();
        session2role.Clear();
        connection2session.Clear();
        kownSessions.Clear();
        sessionIsConfirmed.Clear();
        session2name.Clear();
    }

}

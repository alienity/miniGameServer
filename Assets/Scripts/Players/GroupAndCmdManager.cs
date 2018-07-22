using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GroupAndCmdManager : MonoBehaviour {

    // 0表示在选人场景，1表示在游戏场景，2表示在结束场景
    public int curStageId = 1;
    // 所有的队伍
    public List<GroupPlayer> groupPlayers;
    // 需要实例化的玩家Group对象
    public GroupPlayer groupPlayerToInstance;
    // 收发数据模块
    public ICmdNetHandler cmdNetHandler;
    // 每一帧接受的数据
    private Queue<string> msgQueue;
    // 游戏是否结束
    private bool isGameOver = false;

    // Use this for initialization
    void Start () {
        isGameOver = false;
    }
	
    private void Update()
    {
        if (!isGameOver)
        {
            if (cmdNetHandler != null)
            {
                HandleCmd();
                SendBackCmd();
            }
            else
            {
                cmdNetHandler = Server.Instance;
            }
        }
        else
        {
            // TODO:添加切换手机端控制的代码
        }
    }

    //// 结束所有执行过程：结束组角色的控制，并使组角色不受任何状态影响
    //public void StopAllProcess()
    //{
    //    isGameOver = true;
    //}

    // 检查是否死亡，并在给定的位置随机选择地点重生
    public void CheckDeathAndReborn(List<Transform> rebornTrans)
    {
        if (!isGameOver)
        {
            for (int i = 0; i < groupPlayers.Count; ++i)
            {
                GroupPlayer gp = groupPlayers[i];
                if (!gp.IsAlive)
                {
                    int randomRebornIdx = Random.Range(0, rebornTrans.Count);
                    gp.ReBorn(rebornTrans[randomRebornIdx]);
                }
            }
        }
    }

    // 生成组对象
    public void AddPlayerGroup(int gId, Transform bornTrans)
    {
        GameObject groupPlayerInstance = groupPlayerToInstance.FirstBorn(gId, bornTrans.position);
        GroupPlayer gp = groupPlayerInstance.GetComponent<GroupPlayer>();
        AddGroupPlayerToList(gp);
    }

    // 添加生成的组对象
    private void AddGroupPlayerToList(GroupPlayer gp)
    {
        if (groupPlayers == null)
            groupPlayers = new List<GroupPlayer>();
        groupPlayers.Add(gp);
    }
    
    // 处理接收命令
    public void HandleCmd()
    {
        msgQueue = cmdNetHandler.GetCommands();
        
        foreach (string cmdStr in msgQueue)
        {
            JObject jo = JObject.Parse(cmdStr);
            int stageId = jo["stageId"].Value<int>();

            if (stageId == 0)
            {

            }
            else if (stageId == 1)
            {
                int gId = jo["gId"].Value<int>();
                int uId = jo["uId"].Value<int>();
                float dir_x = jo["direction"]["x"].Value<float>();
                float dir_y = jo["direction"]["y"].Value<float>();
                int skill = jo["skill"].Value<int>();

                Vector3 controllDir = new Vector3(dir_x, 0, dir_y);
                if (uId == (int)GroupPlayer.PlayerType.PIG)
                {
                    groupPlayers[gId].PigMove(controllDir);
                    if (skill == 1)
                        groupPlayers[gId].PigAttack();
                }
                else if (uId == (int)GroupPlayer.PlayerType.PENGU)
                {
                    groupPlayers[gId].PenguMove(controllDir);
                    if (skill == 1)
                        groupPlayers[gId].PenguAttack();
                }
            }
            else if (stageId == 2)
            {

            }
        }

        msgQueue.Clear();
    }

    // 返回控制指令到手机端
    public void SendBackCmd()
    {
        for (int gId = 0; gId < groupPlayers.Count; ++gId)
        {
            GroupPlayer gp = groupPlayers[gId];
            bool isAlive = gp.IsAlive;
            float penguRemainCoolingTime = gp.CoolingTime(GroupPlayer.PlayerType.PENGU);
            float pigRemainCoolingTime = gp.CoolingTime(GroupPlayer.PlayerType.PIG);

            // 写出企鹅的状态
            JObject penguJson = new JObject() {
                { "commMode", 1 },
                { "stageId", curStageId },
                { "gId", gId },
                { "uId", (int)GroupPlayer.PlayerType.PENGU },
                { "joystickAvailable", isAlive ? 1 : 0 },
                { "coolingTime",  penguRemainCoolingTime }
            };
            cmdNetHandler.SendCommand(penguJson.ToString());
            // 写出猪的状态
            JObject pigJson = new JObject() {
                { "commMode", 1 },
                { "stageId", curStageId },
                { "gId", gId },
                { "uId", (int)GroupPlayer.PlayerType.PIG },
                { "joystickAvailable", isAlive ? 1 : 0 },
                { "coolingTime",  pigRemainCoolingTime }
            };
            cmdNetHandler.SendCommand(pigJson.ToString());

        }
    }

}

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
    public GameControllHandler gameControllHandler;
    // 每一帧接受的数据
    private Queue<JoystickControllMsg> msgQueue;
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
            if (gameControllHandler != null)
            {
                HandleCmd();
                SendBackCmd();
            }
            else
            {
                gameControllHandler = FindObjectOfType<GameControllHandler>();
            }
        }
        else
        {
            // TODO:添加切换手机端控制的代码

        }
    }

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
    public void AddPlayerGroup(int gId, Transform bornTrans, Color groupColor)
    {
        GameObject groupPlayerInstance = groupPlayerToInstance.FirstBorn(gId, bornTrans.position, groupColor);
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
        msgQueue = gameControllHandler.GetCommands();

        foreach (JoystickControllMsg jcm in msgQueue)
        {
            int gId = jcm.gId;
            int uId = jcm.uId;
            Vector2 dir = jcm.direction;
            bool skill = jcm.skill;
            bool finish = jcm.finish;
            
            Vector3 controllDir = new Vector3(dir.x, 0, dir.y);
            if (uId == (int)GroupPlayer.PlayerType.PIG)
            {
                groupPlayers[gId].PigMove(controllDir);
                if (skill)
                    groupPlayers[gId].PigAttack();
            }
            else if (uId == (int)GroupPlayer.PlayerType.PENGU)
            {
                groupPlayers[gId].PenguMove(controllDir);
                if (skill)
                    groupPlayers[gId].PenguAttack();
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
            PlayerStateMsg penguPsm = new PlayerStateMsg();
            penguPsm.gId = gId;
            penguPsm.uId = (int)GroupPlayer.PlayerType.PENGU;
            penguPsm.joystickAvailable = isAlive;
            penguPsm.coolingTime = penguRemainCoolingTime;
            gameControllHandler.SendCommand(penguPsm);

            // 写出猪的状态
            PlayerStateMsg pigPsm = new PlayerStateMsg();
            pigPsm.gId = gId;
            pigPsm.uId = (int)GroupPlayer.PlayerType.PENGU;
            pigPsm.joystickAvailable = isAlive;
            pigPsm.coolingTime = penguRemainCoolingTime;
            gameControllHandler.SendCommand(pigPsm);

            //// 写出企鹅的状态
            //JObject penguJson = new JObject() {
            //    { "commMode", 1 },
            //    { "stageId", curStageId },
            //    { "gId", gId },
            //    { "uId", (int)GroupPlayer.PlayerType.PENGU },
            //    { "joystickAvailable", isAlive ? 1 : 0 },
            //    { "coolingTime",  penguRemainCoolingTime }
            //};
            //cmdNetHandler.SendCommand(penguJson.ToString());
            //// 写出猪的状态
            //JObject pigJson = new JObject() {
            //    { "commMode", 1 },
            //    { "stageId", curStageId },
            //    { "gId", gId },
            //    { "uId", (int)GroupPlayer.PlayerType.PIG },
            //    { "joystickAvailable", isAlive ? 1 : 0 },
            //    { "coolingTime",  pigRemainCoolingTime }
            //};
            //cmdNetHandler.SendCommand(pigJson.ToString());

        }
    }

}

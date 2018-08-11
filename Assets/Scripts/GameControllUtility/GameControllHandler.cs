using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameControllHandler : MonoBehaviour
{

//    public static GameControllHandler Instance { get; private set; }

    // 存储摇杆操作
    private Queue<JoystickMsg> jcmQueue = new Queue<JoystickMsg>();
    // 存储蓄力操作
    private Queue<ChargeSkillMsg> csmQueue = new Queue<ChargeSkillMsg>();
    private Queue<RushSkillMag> rsmQueue = new Queue<RushSkillMag>();

    private Server server;

//    private void Awake()
//    {
//        Instance = this;
//    }
    
    private void Start()
    {
        if (server == null)
            server = Server.Instance;

		NetworkServer.RegisterHandler(CustomMsgType.GroupJoystick, OnReceiveJoystick);
        NetworkServer.RegisterHandler(CustomMsgType.GroupChargeSkill, OnReceiveChargeSkill);
        NetworkServer.RegisterHandler(CustomMsgType.GroupRushSkill, OnReceiveRushSkill);

        SceneTransferMsg stageTransferMsg = new SceneTransferMsg("ChooseRoleScene", "GameScene");
        NetworkServer.SendToAll(CustomMsgType.ClientChange, stageTransferMsg);
    }

    public void OnReceiveJoystick(NetworkMessage netmsg)
    {
        JoystickMsg curControllMsg = netmsg.ReadMessage<JoystickMsg>();
        jcmQueue.Enqueue(curControllMsg);
    }
    
    public void OnReceiveChargeSkill(NetworkMessage netmsg)
    {
        ChargeSkillMsg curControllMsg = netmsg.ReadMessage<ChargeSkillMsg>();
        csmQueue.Enqueue(curControllMsg);
    }

    public void OnReceiveRushSkill(NetworkMessage netmsg)
    {
        RushSkillMag rushSkillMsg = netmsg.ReadMessage<RushSkillMag>();
        rsmQueue.Enqueue(rushSkillMsg);
    }

    //--------------------------------------------------------------------------------------------

    public void SendGroupStatus(PlayerStateMsg psm)
    {
        int gId = psm.gId;
        int uId = psm.uId;
        int selectionId = gId * 2 + uId;
        if (!DataSaveController.Instance.session2role.ContainsValue(selectionId)) // 要选择的角色已经不可用
            return;
//        int connectionId = DataSaveController.Instance.role2connectionID[selectionId];
//        NetworkServer.SendToClient(connectionId, CustomMsgType.GroupState, psm);
    }
    
    public Queue<JoystickMsg> GetJoystickQueue()
    {
        return jcmQueue;
    }

    public Queue<ChargeSkillMsg> GetChargeSkillQueue()
    {
        return csmQueue;
    }
    
    public Queue<RushSkillMag> GetRushSkillQueue()
    {
        return rsmQueue;
    }

}

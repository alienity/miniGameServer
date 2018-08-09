using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class SessionMsg : MessageBase
{
    /*
     * 通过stage 判断在哪个场景重连
     * 服务器询问客户端sessionid需要的字段
     *     askSession
     * 客户端回应sessionid需要的字段
     *     privideSessionId
     *     sessionId
     * 服务器分配sessionId, roomId需要的字段
     *     provideSessionInfo
     *     sessiondId
     *     roomId
     * 选人阶段重连需要的信息：
     *     session2role
     *     session2confirm
     *     session2name
     * 游戏阶段需要的信息：
     *     gid
     *     uid
     */
    public bool askSession;
    public bool provideSessionInfo;
    public int sessionId;
    public int roomId;
    public int stage;
    public bool provideRoleId;
    public int gid;
    public int uid;
    public string session2role;
    public string session2confirm;
    public string session2name;

    public SessionMsg()
    {
    }

    public SessionMsg(bool askSession, bool provideSessionInfo, int sessionId, int roomId, Stage stage, bool provideRoleId, int gid, int uid, Dictionary<int, int> session2role, HashSet<int> session2confirm, Dictionary<int,string> session2name)
    {
        this.askSession = askSession;
        this.provideSessionInfo = provideSessionInfo;
        this.sessionId = sessionId;
        this.roomId = roomId;
        this.stage = (int)stage;
        this.provideRoleId = provideRoleId;
        this.gid = gid;
        this.uid = uid;
        this.session2role = JsonConvert.SerializeObject(session2role);
        this.session2confirm = JsonConvert.SerializeObject(session2confirm);
        this.session2name =  JsonConvert.SerializeObject(session2name);
        
    }
    
    public Dictionary<int, int> GetSession2Role()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(session2role);
    }

    public HashSet<int> GetSession2Confirm()
    {
        return JsonConvert.DeserializeObject<HashSet<int>>(this.session2confirm);
    }
    
    public Dictionary<int, string> GetSessionToName()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, string>>(session2name);
    }
    
    public void SetSession2Role(Dictionary<int, int> session2role)
    {
        this.session2role = JsonConvert.SerializeObject(session2role);
    }

    public override string ToString()
    {
        return string.Format("AskSession: {0}, ProvideSessionInfo: {1}, SessionId: {2}, RoomId: {3}, Stage: {4}, ProvideRoleId: {5}, Gid: {6}, Uid: {7}, Session2Role: {8}, Session2Confirm: {9}, Session2Name: {10}", askSession, provideSessionInfo, sessionId, roomId, stage, provideRoleId, gid, uid, session2role, session2confirm, session2name);
    }
}
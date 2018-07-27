﻿using System.Collections.Generic;
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
     * 服务器分配sessionId需要的字段
     *     provideSessionId
     *     sessiondId
     * 选人阶段重连需要的信息：
     *     session2role
     *     confirmed
     * 游戏阶段需要的信息：
     *     gid
     *     uid
     */
    public bool askSession;
    public bool provideSessionId;
    public int sessionId;
    public int stage;
    public bool provideRoleId;
    public int gid;
    public int uid;
    public bool confirmed;
    public string session2role;

    public SessionMsg()
    {
    }

    public SessionMsg(bool askSession, bool provideSessionId, int sessionId, int stage, bool provideRoleId, int gid, int uid, bool confirmed, Dictionary<int, int> session2role)
    {
        this.askSession = askSession;
        this.provideSessionId = provideSessionId;
        this.sessionId = sessionId;
        this.stage = stage;
        this.provideRoleId = provideRoleId;
        this.gid = gid;
        this.uid = uid;
        this.confirmed = confirmed;
        this.session2role = JsonConvert.SerializeObject(session2role);
    }
    
    public Dictionary<int, int> GetSession2Role()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(session2role);
    }
}
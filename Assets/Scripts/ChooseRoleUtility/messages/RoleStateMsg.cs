using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;

// 服务器告知客户端对应的角色是否可用
public class RoleStateMsg : MessageBase
{
    public string session2role;
    public string session2confirm;
    public string session2name;

    public RoleStateMsg(Dictionary<int, int> session2role, HashSet<int> session2confirm, Dictionary<int,string> session2name)
    {
        
        this.session2role = JsonConvert.SerializeObject(session2role);
        this.session2confirm = JsonConvert.SerializeObject(session2confirm);
        this.session2name =  JsonConvert.SerializeObject(session2name);
    }

    public Dictionary<int, int> GetSessionToRole()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(session2role);
    }

    public HashSet<int> GetSesssion2Confirm()
    {
        return JsonConvert.DeserializeObject<HashSet<int>>(this.session2confirm);
    }
    
    public Dictionary<int, string> GetSessionToName()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, string>>(session2name);
    }

    
    public RoleStateMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("Session2Role: {0}, Session2Confirm: {1}", session2role, session2confirm);
    }
}
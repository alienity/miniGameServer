using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;

// 服务器告知客户端对应的角色是否可用
public class RoleStateMsg : MessageBase
{
    public string session2role;

    public RoleStateMsg(Dictionary<int, int> session2role)
    {
        
        this.session2role = JsonConvert.SerializeObject(session2role);
    }

    public Dictionary<int, int> GetSessionToRole()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(session2role);
    }
    public RoleStateMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("Connection2Role: {0}", session2role);
    }
}
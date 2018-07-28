using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;

// 服务器告知客户端对应的角色是否可用
public class RoleStateMsg : MessageBase
{
    public string connection2role;

    public RoleStateMsg(Dictionary<int, int> connection2Role)
    {
        
        connection2role = JsonConvert.SerializeObject(connection2Role);
    }

    public Dictionary<int, int> GetConnection2Role()
    {
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(connection2role);
    }
    public RoleStateMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("Connection2Role: {0}", connection2role);
    }
}
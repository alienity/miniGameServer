using UnityEngine;
using UnityEngine.Networking;

public class AdvanceControlHandler: MonoBehaviour
{
    /*
     * 接收时写回调函数，发送时不用写
     */
    public void OnReceiveAdvanceControl(NetworkMessage netmsg)
    {
    }
}
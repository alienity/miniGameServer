using UnityEngine;
using UnityEngine.Networking;

public class JoystickMsg : MessageBase
{

    public int gId;
    public int uId;
    public Vector2 direction;

    public JoystickMsg(int gId, int uId, Vector2 direction)
    {
        this.gId = gId;
        this.uId = uId;
        this.direction = direction;
    }

    public JoystickMsg() { }

}

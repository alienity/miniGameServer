using System.Collections.Generic;
using UnityEngine;

public abstract class ICmdNetHandler : MonoBehaviour
{

    public abstract Queue<string> GetCommands();

    public abstract void SendCommand(string cmd);

}

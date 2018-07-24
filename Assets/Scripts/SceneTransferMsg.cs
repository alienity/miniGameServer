using UnityEngine.Networking;

public class SceneTransferMsg : MessageBase
{
    public string curSceneName;
    public string nextSceneName;

    public SceneTransferMsg(string curSceneName, string nextSceneName)
    {
        this.curSceneName = curSceneName;
        this.nextSceneName = nextSceneName;
    }

    public SceneTransferMsg()
    {
    }
}
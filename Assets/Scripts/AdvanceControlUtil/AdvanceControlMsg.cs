using UnityEngine.Networking;

public enum AdvanceControlType
{
    Viberate
}
public class AdvanceControlMsg: MessageBase
{
    public AdvanceControlType type;
    public float duration;
    public float interval;

    public AdvanceControlMsg(AdvanceControlType type, float duration, float interval)
    {
        this.type = type;
        this.duration = duration;
        this.interval = interval;
    }

    public AdvanceControlMsg()
    {
    }
}

﻿using UnityEngine.Networking;

public enum AdvanceControlType
{
    Viberate
}
public class AdvanceControlMsg: MessageBase
{
    public AdvanceControlType type; 
    
}

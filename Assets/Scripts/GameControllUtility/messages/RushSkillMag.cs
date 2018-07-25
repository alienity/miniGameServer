using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RushSkillMag : MessageBase
{

    public int gId;
    public int uId;
    public bool skill;

    public RushSkillMag(int gId, int uId, bool skill)
    {
        this.gId = gId;
        this.uId = uId;
        this.skill = skill;
    }

    public RushSkillMag() { }

}

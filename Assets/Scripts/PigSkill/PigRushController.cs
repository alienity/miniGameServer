using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigRushController : PigSkillController {

    // 维持猪的加速度给的力
    public float pigRushForce;
    // 发动后过去时间
    private float pigPastTime;
    // 技能持续时间
    public float pigRushTime;
    // 控制组的无敌时间
    private GroupPlayer gpPlayer;

    // Use this for initialization
    void Start () {
        remainColdingTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        remainColdingTime -= Time.deltaTime;
        pigPastTime += Time.deltaTime;
    }

    // 持续效用的技能
    public override void ContinueUpdate(Vector3 pigCurDirection)
    {
        if (pigPastTime < pigRushTime)
        {
            pigRd.AddForce(pigCurDirection * pigRushForce);
            pigPlayer.IsCrazy = true;
            Debug.Log("冲刺技能更新中，变无敌");
        }
        else
        {
            pigPlayer.RemoveContinueSkills(ContinueUpdate);
            pigPlayer.IsCrazy = false;
            Debug.Log("冲刺技能结束，无敌取消");
        }
    }

    public override void UseSkill(PigPlayer pigPlayer)
    {
        if(AvailableNow() == 1)
        {
            this.pigPlayer = pigPlayer;
            pigRd = pigPlayer.groupRd;
            pigPlayer.AddContinueSkills(ContinueUpdate);
            pigPastTime = 0;
            remainColdingTime = maxColdingTime;
        }
    }
    public override int AvailableNow()
    {
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSpeedupRoadController : PigSkillController{

    // 加速滑道的猪的加速度给的力
    public float pigSlipForce;
    // 技能持续时间
    public float speedupTime = 2.5f;
    // 发动后过去时间
    private float speedupPastTime;

    // Use this for initialization
    void Start () {
        remainColdingTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        remainColdingTime -= Time.deltaTime;
        speedupPastTime += Time.deltaTime;
    }

    // 持续效用的技能
    public override void ContinueUpdate(Transform pigCurTransform)
    {
        if (speedupPastTime < speedupTime)
        {
            pigRd.AddForce(pigCurTransform.forward * pigSlipForce);
            Debug.Log("加速滑道更新中");
        }
        else
        {
            pigPlayer.RemoveContinueSkills(ContinueUpdate);
            Debug.Log("加速滑道结束");
        }
    }

    public override void UseSkill(PigPlayer pigPlayer)
    {
        if (AvailableNow() == 1)
        {
            this.pigPlayer = pigPlayer;
            pigRd = pigPlayer.groupRd;
            pigPlayer.AddContinueSkills(ContinueUpdate);
            speedupPastTime = 0;
            remainColdingTime = maxColdingTime;
            // TODO 加滑行特效
        }
    }

    public override int AvailableNow()
    {
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }
}

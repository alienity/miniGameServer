using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigFlashController : PigSkillController {
    
    // 闪现距离
    public float flashDistance;
    // 技能剩余次数
    public int remainNums;

    // Use this for initialization
    void Start () {
        remainColdingTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        remainColdingTime -= Time.deltaTime;
        if (RemainNums() == 0)
            Destroy(gameObject);
    }

    public override void UseSkill(PigPlayer pigPlayer)
    {
        if (0 == AvailableNow()) return;

        Transform groupTrans = pigPlayer.groupTrans;
        Vector3 pigArrowDir = pigPlayer.pigCurDirection;
        Vector3 showPlace = groupTrans.position + pigArrowDir * flashDistance;
        groupTrans.position = showPlace;

        remainNums -= 1;
        remainColdingTime = maxColdingTime;
    }

    public override int RemainNums()
    {
        return remainNums;
    }

    public override int AvailableNow()
    {
        if (remainNums > 0 && remainColdingTime < 0)
            return 1;
        return 0;
    }
    
}

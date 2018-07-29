﻿using System.Collections;
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
    public float invulnerableTime;
    // 猪的放屁效果
    public ParticleSystem fartParticleEffects;

    // 放屁效果实例化对象
    private ParticleSystem fartInstance;

    // Use this for initialization
    void Start () {
        remainColdingTime = 0;
        InitialParticles();
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
            //pigPlayer.IsCrazy = true;
            Debug.Log("冲刺技能更新中，变无敌");
        }
        else
        {
            pigPlayer.RemoveContinueSkills(ContinueUpdate);
            //pigPlayer.IsCrazy = false;
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

            fartInstance.Emit(10);
            //fartInstance.Simulate(3f);

            StartCoroutine(Invulnerator());
        }
    }
    public override int AvailableNow()
    {
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }

    IEnumerator Invulnerator()
    {
        this.pigPlayer.IsCrazy = true;
        yield return new WaitForSeconds(invulnerableTime);
        this.pigPlayer.IsCrazy = false;
    }

    private void InitialParticles()
    {
        fartInstance = Instantiate(fartParticleEffects) as ParticleSystem;
        fartInstance.transform.parent = transform;
        fartInstance.transform.position = transform.position + Vector3.up + Vector3.left * 1.2f;
        fartInstance.transform.rotation = Quaternion.Inverse(transform.rotation);
    }

}

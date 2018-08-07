using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSelfRescueController : PigSkillController {

    // 猪自救时向上飞行的初始速度
    public float SelfRescueSpeed = 2f;
    // 猪的放屁效果
    public ParticleSystem fartParticleEffects;
    // 放屁效果实例化对象
    private ParticleSystem fartInstance;

    // Use this for initialization
    void Start()
    {
        remainColdingTime = 0;
        InitialParticles();
    }

    // Update is called once per frame
    void Update()
    {
        remainColdingTime -= Time.deltaTime;
    }

    public override void UseSkill(PigPlayer pigPlayer)
    {
        if (AvailableNow() == 1)
        {
            this.pigPlayer = pigPlayer;
            pigRd = pigPlayer.groupRd;
            PigPlayerSurvival(SelfRescueSpeed);
            remainColdingTime = maxColdingTime;

            fartInstance.Play();
            //fartInstance.Emit(10);
        }
    }

    // 发动自救
    public void PigPlayerSurvival(float SelfRescueSpeed)
    {
        Debug.Log("pig Survival jump");

        pigRd.velocity = new Vector3(0f, SelfRescueSpeed, 0f);
    }

    public override int AvailableNow()
    {
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }
    private void InitialParticles()
    {
        fartInstance = Instantiate(fartParticleEffects, transform) as ParticleSystem;
        fartInstance.transform.position = transform.position;
        fartInstance.transform.rotation = transform.rotation;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallController : ShotBallController
{
    
    // Ball的冷却时间
    public float snowBallColdingTime;
    // Use this for initialization
    void Start () {
        if (ball == null)
            ball = FindObjectOfType<SnowBall>();
        remainColdingTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(remainColdingTime > 0)
            remainColdingTime -= Time.deltaTime;
    }

    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        if (0 == AvailableNow()) return;
        ball.SpawnBall(ownerId, position, rotation);
        remainColdingTime = snowBallColdingTime;
    }

    // 剩余的能用的ball的数量
    public override int RemainNums()
    {
        return 1;
    }
    
    // 当前是否可以使用Ball
    public override int AvailableNow()
    {
        //Debug.Log(remainColdingTime);
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }
    
}

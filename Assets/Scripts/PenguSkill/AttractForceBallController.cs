using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractForceBallController : ShotBallController {

    // 初始的数量
    public int totalBallNums = 5;
    // 剩余的数量
    private int remainBallNums;

    // Use this for initialization
    void Start () {
        if (ball == null)
            ball = FindObjectOfType<AttractiveForceBall>();
        ReSet();
    }
	
	// Update is called once per frame
	private void Update () {
        if (remainColdingTime > 0)
            remainColdingTime -= Time.deltaTime;
        if (RemainNums() == 0)
            Destroy(gameObject);
    }

    public void ReSet()
    {
        remainBallNums = totalBallNums;
    }

    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        if (0 == AvailableNow()) return;
        ball.SpawnBall(ownerId, position, rotation, 0);
        remainBallNums -= 1;
        remainColdingTime = maxColdingTime;
        this.ResetCharge();
    }

    // 剩余的能用的ball的数量
    public override int RemainNums()
    {
        return remainBallNums;
    }

    // 当前是否可以使用Ball
    public override int AvailableNow()
    {
        if (remainBallNums > 0 && remainColdingTime < 0)
            return 1;
        return 0;
    }

}

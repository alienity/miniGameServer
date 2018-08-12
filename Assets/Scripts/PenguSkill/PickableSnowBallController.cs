using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableSnowBallController : SnowBallController
{

    // 剩余可用的ball的数量
    public int remainBallNums = 5;
    
    private Text CountRemainingText;

    public void SetCountRemainingText(Text t)
    {
        if (t != null)
        {
            CountRemainingText = t;
            Color temp = CountRemainingText.color;
            temp.a = 1;
            CountRemainingText.color = temp;
            CountRemainingText.text = remainBallNums.ToString();
        }
        else
        {
            Debug.LogError("CountRemainingText is null");
        }
    }
    
    void Start()
    {
        if (ball == null)
            ball = FindObjectOfType<SnowBall>();
        remainColdingTime = 0;
    }

    void Update()
    {
        // 没有开始蓄力，就是释放结束了
        if (this.attackFinished)
        {
            if (remainColdingTime > 0)
                remainColdingTime -= Time.deltaTime;
        }
    }

    // 充能结束后释放
    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation, Color ballColor)
    {
        if (0 == AvailableNow()) return;
        float chargedPastTime = this.chargeCurrentTime - this.chargeStartTime;
        SnowBall snowBall = Instantiate(ball, position, rotation) as SnowBall;
        snowBall.InitiateBall(ownerId, Mathf.Clamp(chargedPastTime, 0, maxChargeTime), ballColor, ShotBall.BallType.SnowBall);
        remainColdingTime = maxColdingTime;
        remainBallNums -= 1;

        if (CountRemainingText != null)
        {
            CountRemainingText.text = remainBallNums.ToString();
            if (remainBallNums <= 0)
            {
                Color temp = CountRemainingText.color;
                temp.a = 0;
                CountRemainingText.color = temp;
            }
        }
        ResetCharge();
    }

    // 剩余的能用的ball的数量
    public override int RemainNums()
    {
        return remainBallNums;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableFireBallController : ShotBallController
{
    // 球类型
    public ShotBall.BallType ballType;
    // 剩余可用的ball的数量
    public int remainBallNums = 5;
    
    // 火球计数器
    private Text CountRemainingText;
    // 火球图标
    private Image fireBallIcon;
    
    public void SetCountRemainingText(Text t, Image fbIcon)
    {
        if (t != null)
        {
            CountRemainingText = t;
            Color temp = CountRemainingText.color;
            temp.a = 1;
            CountRemainingText.color = temp;
            CountRemainingText.text = remainBallNums.ToString();

            this.fireBallIcon = fbIcon;
            Color fireBallIconColor = fireBallIcon.color;
            fireBallIconColor.a = 1;
            fireBallIcon.color = fireBallIconColor;
        }
        else
        {
            Debug.LogError("CountRemainingText is null");
        }
    }
    
    protected void Start()
    {
        if (ball == null)
            ball = FindObjectOfType<FireBall>();
        remainColdingTime = 0;
    }

    protected void Update()
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
        FireBall shotBall = Instantiate(ball, position, rotation) as FireBall;
        shotBall.InitiateBall(ownerId, Mathf.Clamp(chargedPastTime, 0, maxChargeTime), ballColor, ShotBall.BallType.FireBall);
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

                Color fireBallIconColor = fireBallIcon.color;
                fireBallIconColor.a = 0;
                fireBallIcon.color = fireBallIconColor;
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

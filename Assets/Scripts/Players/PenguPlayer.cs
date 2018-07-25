using UnityEngine;
using System.Collections;

public class PenguPlayer : MonoBehaviour
{

    public int gId;
    private int uId = 1;
    public int playerId;

    // 企鹅攻击指向
    public Vector3 penguCurDirection;
    // 企鹅转向速率
    public float penguRotateSpeed;
    // 企鹅原始默认投掷控制器
    public ShotBallController snowBallController;
    // 且新捡到的投掷物品
    public ShotBallController curBallController;
    //// 蓄力时长
    //public float maxChargeTime;

    //// 记录开始蓄力
    //private float chargeStartTime = -1;
    //// 记录蓄力到现在，当前时刻
    //private float chargeCurrentTime = -1;
    // 自有对象
    private Transform mTrans;

    // Use this for initialization
    void Start()
    {
        mTrans = GetComponent<Transform>();
        Reset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 修改当前朝向
        mTrans.rotation = Quaternion.LookRotation(penguCurDirection);


        // ********************测试代码*******************
        /*
        Vector3 m_newDir = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_newDir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_newDir += Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_newDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_newDir += Vector3.right;
        }
        SetArrowDirection(m_newDir.normalized);

        if (Input.GetKeyDown(KeyCode.L))
        {
            PenguPlayerAttack();
        }
        */
        // ********************测试代码*******************

    }

    // 重生后重置参数
    public void Reset()
    {
        if (snowBallController == null)
            snowBallController = GetComponent<SnowBallController>();
        //curBallController = snowBallController;
    }

    // 设置组ID
    public void SetId(int gId, int pId)
    {
        this.gId = gId;
        this.playerId = pId;
    }

    // 设置指向
    public void SetArrowDirection(Vector3 dir)
    {
        if (dir.magnitude == 0) return;
        penguCurDirection = dir;
    }

    // 捡物品
    public void CatchItem(ShotBallController shotController)
    {
        curBallController = shotController;
    }
    
    // 检查SkillController并做替换
    public void CheckSkillController()
    {
        if ((curBallController != null && curBallController.RemainNums() == 0)
            || (curBallController == null))
        {
            curBallController = snowBallController;
            //maxChargeTime = curBallController.MaxChargeTime();
        }
    }
    /*
    // 开始蓄力
    private void PenguPlayerStartCharge(float startTime)
    {
        chargeStartTime = startTime;
        chargeCurrentTime = startTime;
    }

    // 蓄力进行中
    private void PenguPlayerChargin(float curTime)
    {
        chargeCurrentTime = curTime;
    }

    // 结束蓄力
    private void PenguPlayerFinishCharge()
    {
        chargeStartTime = -1;

    }
    */
    // 根据时间处理蓄力技能
    public void HandleChargeSkill(float chargeStartTime, float chargeCurrentTime, bool chargeReturn)
    {
        // 测试蓄力
        curBallController.HandleChargeAttack(chargeStartTime, chargeCurrentTime);
        if (chargeReturn) PenguPlayerAttack();
    }

    // 发射雪球
    public void PenguPlayerAttack()
    {
        Vector3 ballBirthPlace = mTrans.position + mTrans.forward * 1.2f;
        CheckSkillController();
        if (curBallController.AvailableNow() == 1)
            curBallController.UseBall(gId, ballBirthPlace, mTrans.rotation);
    }

    // 返回技能剩余冷却时间，同步到手机端
    public float RemainingColdingTime()
    {
        CheckSkillController();
        float remainColdingTime = curBallController.RemainColdingTime();
        if (remainColdingTime < 0)
            return 0;
        else
            return remainColdingTime;
    }

}

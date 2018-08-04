using UnityEngine;
using System.Collections;

public class PenguPlayer : MonoBehaviour
{
    private AudioSource selfAudioSource;

    public int gId { get; set; }
    private const int uId = 1;

    // 企鹅攻击指向
    public Vector3 penguCurDirection;
    // 企鹅转向速率
    public float penguRotateSpeed;
    // 企鹅原始默认投掷控制器
    public ShotBallController snowBallController;
    // 且新捡到的投掷物品
    public ShotBallController curBallController;
    // 发雪球的音效
    public AudioClip throwSnowBall;

    // 动画控制器
    private Animator animator;
    private int attackAnimId;

    // 企鹅箭头
    public SpriteRenderer arrowRender;
    // 箭头最长距离
    public float arrowMaxRatio = 3f;
    // 箭头最短距离
    public float arrowMinRatio = 1f;

    // 雪球出生前向偏移
    public float birthForward = 0.02f;

    // 自身transform
    private Transform mTrans;

    // 猪眩晕啥也不能干
    public bool IsSturn { get; set; }

    // 死亡
    public bool IsDie { get; set; }

    // Use this for initialization
    void Start()
    {
        if (selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();

        //暂时只加入一种声音
        selfAudioSource.clip = throwSnowBall;

        mTrans = GetComponent<Transform>();

        animator = GetComponent<Animator>();
        attackAnimId = Animator.StringToHash("shot");

        CheckSkillController();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDie || IsSturn) return;

        // 修改当前朝向
        /*
        if (penguCurDirection.magnitude != 0)
            mTrans.rotation = Quaternion.LookRotation(penguCurDirection);
        */

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

    public void Reset()
    {

    }

    // 设置指向
    public void SetArrowDirection(Vector3 dir)
    {
        if (dir.magnitude == 0 || IsSturn) return;
        penguCurDirection = dir;
        mTrans.rotation = Quaternion.LookRotation(penguCurDirection);
    }

    // 捡物品
    public void CatchItem(ShotBallController shotController)
    {
        curBallController = shotController;
    }
    
    // 检查SkillController并做替换
    public void CheckSkillController()
    {
        if ((curBallController != null && curBallController.RemainNums() == 0) || (curBallController == null))
        {
            if(curBallController != null)
                DestroyImmediate(curBallController);
            curBallController = Instantiate(snowBallController, mTrans);
        }
    }

    // 设置箭头颜色
    public void SetArrowColor(Color arrowColor)
    {
        arrowRender.color = arrowColor;
    }

    // 修改箭头长度
    public void SetArrowLen(float arrowRatio)
    {
        Vector2 arrowSize = arrowRender.size;
        arrowSize.y = Mathf.Lerp(arrowMinRatio, arrowMaxRatio, arrowRatio);
        arrowRender.size = arrowSize;
    }

    // 根据时间处理蓄力技能
    public void HandleChargeSkill(string processId, float currrentTime, int touchId)
    {

        float ratio = curBallController.HandleChargeAttack(processId, currrentTime, touchId);
        SetArrowLen(ratio);
        PenguPlayerAttack();
    }

    // 发射雪球
    public void PenguPlayerAttack()
    {
        if (curBallController.AvailableNow() == 1)
        {
            if (curBallController.chargeFinished)
            {
                selfAudioSource.Play();
                animator.SetTrigger(attackAnimId);
                Vector3 ballBirthPlace = mTrans.position + mTrans.forward * birthForward;
                curBallController.UseBall(gId, ballBirthPlace, mTrans.rotation);
                SetArrowLen(0);
            }
        }
        CheckSkillController();
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

    // 返回技能的最大冷却时间
    public float MaxColdingTime()
    {
        CheckSkillController();
        return curBallController.MaxColdingTime();
    }

}

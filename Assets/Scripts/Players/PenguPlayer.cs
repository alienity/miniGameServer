using UnityEngine;
using System.Collections;

public class PenguPlayer : MonoBehaviour
{
    private AudioSource selfAudioSource;

    public int gId { get; set; }
    private const int uId = 1;

    // 企鹅攻击指向
    [HideInInspector] public Vector3 penguCurDirection = Vector3.forward;
    // 企鹅转向速率
    public float penguRotateSpeed;
    // 企鹅原始默认投掷控制器
    public SnowBallController snowBallController;
    private SnowBallController defaultSnowBallController;
    // 且新捡到的投掷物品
    public ShotBallController curBallController;
    // 发雪球的音效
    public AudioClip throwSnowBall;

    // 动画控制器
    private Animator animator;
    private int attackAnimId;
    
    // cd箭头控制器
    public FilledArrowSpriteController filledArrowSpriteController;

    // 雪球出生前向偏移
    public Transform attackTrans;
    // 旋转速度
    public float rotateSpeed;

    // 猪眩晕啥也不能干
    public bool IsSturn { get; set; }
    // 死亡
    public bool IsDie { get; set; }
    
    
    // 企鹅对应队伍的颜色，这个颜色用于箭头颜色和雪球颜色
    private Color groupColor = Color.black;
    // Use this for initialization
    void Start()
    {
        if (selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();

        if (filledArrowSpriteController == null)
            filledArrowSpriteController = GetComponentInChildren<FilledArrowSpriteController>();

        //暂时只加入一种声音
        selfAudioSource.clip = throwSnowBall;
        
        // 动画播放
        animator = GetComponent<Animator>();
        attackAnimId = Animator.StringToHash("shot");

        CheckSkillController();
    }

    private void Update()
    {
        if (IsDie || IsSturn) return;

        // 修改当前朝向
        /*
        if (penguCurDirection.magnitude != 0)
            mTrans.rotation = Quaternion.LookRotation(penguCurDirection);
        */

        // 设置cd进度
        filledArrowSpriteController.SetProgress(1 - RemainingColdingTime() / MaxColdingTime());

        // ********************测试代码*******************
        /**/
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
            Debug.Log("攻击键");
            float tm = Time.time;
            HandleChargeSkill(tm.ToString(), Time.time, 0);
            HandleChargeSkill(tm.ToString(), Time.time, 1);

            //PenguPlayerAttack();
        }
        
        // ********************测试代码*******************
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 修改当前朝向
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(penguCurDirection), Time.deltaTime * rotateSpeed);
        //mTrans.rotation = Quaternion.LookRotation(penguCurDirection);

        if (IsDie || IsSturn) return;
        
    }

    public void Reset()
    {

    }

    // 设置指向
    public void SetArrowDirection(Vector3 dir)
    {
        if (dir.magnitude == 0 || IsSturn) return;
        penguCurDirection = dir;
        //mTrans.rotation = Quaternion.LookRotation(penguCurDirection);
    }

    // 捡物品
    public void CatchItem(ShotBallController shotController)
    {

        if (curBallController != null)
            Destroy(curBallController.gameObject);

        curBallController = shotController;
        curBallController.transform.parent = transform;
        curBallController.transform.position = transform.position;
        curBallController.transform.rotation = transform.rotation;
    }
    
    // 检查SkillController，如果没有雪球，则使用默认雪球
    public void CheckSkillController()
    {
        if(defaultSnowBallController == null)
        {
            defaultSnowBallController = Instantiate(snowBallController, transform);
            defaultSnowBallController.transform.position = transform.position;
            defaultSnowBallController.transform.rotation = transform.rotation;
        }

        if ((curBallController != null && curBallController.RemainNums() == 0) || (curBallController == null))
        {
            if(curBallController != null)
                DestroyImmediate(curBallController.gameObject);
            curBallController = defaultSnowBallController;
        }
    }
    
    // 设置箭头颜色
    public void SetArrowAndSnowballColor(Color color)
    {
        //arrowBoundRender.color = arrowColor;
        //filledArrowRender.color = arrowColor;
        //filledArrowRender.material.color = arrowColor;
        filledArrowSpriteController.SetArrowColor(color);
        groupColor = color;
    }
    /*
    // 修改箭头长度
    public void SetArrowLen(float arrowRatio)
    {
        Vector2 arrowSize = arrowBoundRender.size;
        arrowSize.y = Mathf.Lerp(arrowMinRatio, arrowMaxRatio, arrowRatio);
        arrowBoundRender.size = arrowSize;
        filledArrowRender.size = arrowSize;
        Debug.Log("filledArrowRender.size" + filledArrowRender.size + " arrowSize" + arrowSize + " arrowBoundRender.size: " + arrowBoundRender.size );
    }
    */
    // 根据时间处理蓄力技能
    public void HandleChargeSkill(string processId, float currrentTime, int touchId)
    {
        Debug.Log(touchId);
        float ratio = curBallController.HandleChargeAttack(processId, currrentTime, touchId);
        //SetArrowLen(ratio);
        filledArrowSpriteController.SetArrowLen(ratio);
        if(touchId == 1)
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
                curBallController.UseBall(gId, attackTrans.position, transform.rotation, groupColor);
                //SetArrowLen(0);
                filledArrowSpriteController.SetArrowLen(0);
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

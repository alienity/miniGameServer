using UnityEngine;
using System.Collections;

public class PigPlayer : MonoBehaviour
{
    public int gId { get; set; }
    private const int uId = 1;

    // 更新函数
    public delegate void ContinueSkill(Transform pigCurTransform);
    // 持续更新函数
    private ContinueSkill continueSkills;

    // 队伍Transform，使用闪现等要直接位移的技能时需要用到
    public Transform groupTrans;
    // pig的Transform，改变当前箭头朝向
    private Transform mTrans;
    // 要移动的对象
    public Rigidbody groupRd;
    
    // 维持猪的加速度给的力
    public float accForce;
    // 猪箭头指向
    public Vector3 pigCurDirection = Vector3.forward;
    // 猪移动向量
    public Vector3 pigMoveDirection;
    // 猪正常移动速度
    public float pigNormalSpeed;

    // 猪冲撞时能冲撞走球
    public bool IsCrazy { get; set; }
    // 猪眩晕啥也不能干
    public bool IsSturn { get; set; }
    // 猪位于悬崖边的自救区
    private bool isSurivalSkillNow = false;

    // 猪的动画组件
    private Animator animator;
    private int runSpeedId;

    //wwq 音源
    private AudioSource selfAudioSource;
    // 音乐片段
    public AudioClip runingClip;
    public AudioClip rushClip;
    // 声音播放速度
    public float maxAudioRatio;

    // 猪的阴影
    public GameObject shadowObj;

    // 猪默认技能
    public PigSkillController pigRushController;
    // 猪自救技能
    public PigSkillController pigSelfRescueController;
    // 猪当前技能
    public PigSkillController curSkillController;

    // cd箭头控制器
    public FilledArrowSpriteController fillArrowSpriteController;

    // 保存猪常用的两个技能
    private PigSkillController pigRushControllerInstance;
    private PigSkillController pigSelfRescueControllerInstance;

    // 用于技能切换时暂存技能

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        runSpeedId = Animator.StringToHash("speed");

        mTrans = GetComponent<Transform>();
        if (groupTrans == null)
            groupTrans = GetComponentInParent<Transform>();
        if(groupRd == null)
            groupRd = GetComponentInParent<Rigidbody>();
        if(selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();

        if (fillArrowSpriteController == null)
            fillArrowSpriteController = GetComponentInChildren<FilledArrowSpriteController>();

        selfAudioSource.clip = runingClip;

        CheckSkillController();
    }

    private void Update()
    {
        // 设置降下来的时候阴影在地面上
        RaycastHit hit;
        if (Physics.Raycast(groupTrans.position, Vector3.down, out hit))
        {
            shadowObj.transform.position = hit.point + 0.1f * Vector3.up;
        }
        // 根据cd填充箭头
        fillArrowSpriteController.SetProgress(1 - RemainingColdingTime() / MaxColdingTime());

        // ********************测试代码*******************
        /**/
        Vector3 m_newDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            m_newDir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_newDir += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_newDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_newDir += Vector3.right;
        }
        SetDirection(m_newDir.normalized);

        if (Input.GetKeyDown(KeyCode.J))
        {
            PigPlayerAttack();
        }

        // ********************测试代码*******************

    }

    private void FixedUpdate()
    {
        // 修改箭头指向
        mTrans.rotation = Quaternion.LookRotation(pigCurDirection);
        
        // 根据移动速度更改音乐播放
        Vector2 horizontalVel = new Vector2(groupRd.velocity.x, groupRd.velocity.z);
        // 根据速度调整动画
        animator.SetFloat(runSpeedId, Mathf.Lerp(0.01f, pigNormalSpeed, horizontalVel.magnitude / pigNormalSpeed));
        // 根据速度调整音效
        if (horizontalVel.magnitude > 0.02)
        {
            // 播放音效
            if (!selfAudioSource.isPlaying)
            {
                if (selfAudioSource.clip != runingClip)
                    selfAudioSource.clip = runingClip;
                selfAudioSource.Play();
                selfAudioSource.volume = Mathf.Lerp(0, 0.3f, horizontalVel.magnitude / pigNormalSpeed);
                selfAudioSource.pitch = Mathf.Lerp(1f, maxAudioRatio + 1f, horizontalVel.magnitude / pigNormalSpeed);
            }
        }
        else
        {
            // 停止移动动画播放
            if (!selfAudioSource.isPlaying)
                selfAudioSource.Stop();
        }
        
        //移动更新，加速到最大速度后匀速运动
        if (pigMoveDirection.magnitude != 0)
        {
            if (groupRd.velocity.magnitude < 0.01)
            {
                groupRd.AddForce(pigMoveDirection * (groupRd.drag + accForce));
            }
            else
            {
                Vector3 velocityDirection = groupRd.velocity.normalized;
                Vector3 velocityCrossDirection =
                    Vector3.Cross(Vector3.Cross(velocityDirection, pigMoveDirection), velocityDirection);
                float cosDegree = Vector3.Dot(velocityDirection, pigMoveDirection);
                float sinDegree = Mathf.Sqrt(1 - cosDegree * cosDegree);

                // 加速
                if (groupRd.velocity.magnitude < pigNormalSpeed)
                    groupRd.AddForce(velocityDirection * (groupRd.drag + accForce * cosDegree));
                else
                    groupRd.AddForce(velocityDirection * groupRd.drag);
                // 偏移
                Vector3 biasForce = velocityCrossDirection * (groupRd.drag + accForce * sinDegree);
                if(biasForce.magnitude != 0)
                    groupRd.AddForce(biasForce);
            }
        }
        
        // 更新持续技能
        if (continueSkills != null)
            continueSkills(mTrans);

    }

    // 设置箭头颜色
    public void SetArrowColor(Color arrowColor)
    {
        //arrowBoundRender.color = arrowColor;
        //filledArrowRender.color = arrowColor;
        //filledArrowRender.material.color = arrowColor;
        fillArrowSpriteController.SetArrowColor(arrowColor);
    }

    public void StopSkillNow()
    {
        PigRushController csc = curSkillController as PigRushController;
        csc.StopSkill();
    }

    public void Reset()
    {
        groupRd.velocity = Vector3.zero;
        //pigMoveDirection = Vector3.zero;
        continueSkills = null;
        selfAudioSource.Stop();
    }

    // 捡物品
    public void CatchItem(PigSkillController pigSkillController)
    {
        curSkillController = pigSkillController;
        curSkillController.transform.parent = mTrans;
        curSkillController.transform.position = mTrans.position;
        curSkillController.transform.rotation = mTrans.rotation;
    }

    // 设置移动方向
    public void SetDirection(Vector3 dir)
    {
        if (IsSturn) return;

        pigMoveDirection = dir;
        if (pigMoveDirection.magnitude == 0) return;
        pigCurDirection = pigMoveDirection.normalized;
    }

    // 发动冲刺
    public void PigPlayerAttack()
    {
        if (IsSturn || IsSturn) return;

        Debug.Log("pig Attack");
        CheckSkillController();

        if (curSkillController.AvailableNow() == 1)
        {
            if (selfAudioSource.clip != rushClip)
            {
                selfAudioSource.volume = 1;
                selfAudioSource.pitch = 1;
                selfAudioSource.PlayOneShot(rushClip);
            }
            curSkillController.UseSkill(this);
        }
    }

    // 添加持续更新的技能
    public void AddContinueSkills(ContinueSkill cs)
    {
        continueSkills += cs;
    }
    // 删除持续更新的技能
    public void RemoveContinueSkills(ContinueSkill cs)
    {
        continueSkills -= cs;
    }

    // 受到攻击有相应的加速加成
    public void ReceiveSuddenSpeed(Vector3 suddenSpeed)
    {
        groupRd.velocity += suddenSpeed;
    }

    // 猪受到力的作用
    public void ReceiveForce(Vector3 addedForce)
    {
        groupRd.AddForce(addedForce);
    }

    // 判断在悬崖上，然后更换技能
    public bool IsSurivalSkillNow
    {
        get { return isSurivalSkillNow; }
        set
        {
            if (value == true)
            {
                curSkillController = pigSelfRescueControllerInstance;
            }
            else
            {
                curSkillController = pigRushControllerInstance;
            }
        }
    }

    // 检查SkillController并做替换
    public void CheckSkillController()
    {
        if(pigRushControllerInstance == null)
        {
            pigRushControllerInstance = Instantiate(pigRushController, mTrans);
            pigRushControllerInstance.transform.position = mTrans.position;
            pigRushControllerInstance.transform.rotation = mTrans.rotation;
        }
        if(pigSelfRescueControllerInstance == null)
        {
            pigSelfRescueControllerInstance = Instantiate(pigSelfRescueController, mTrans);
            pigSelfRescueControllerInstance.transform.position = mTrans.position;
            pigSelfRescueControllerInstance.transform.rotation = mTrans.rotation;
        }

        if ((curSkillController != null && curSkillController.RemainNums() == 0) || (curSkillController == null))
        {
            if(curSkillController != null)
                DestroyImmediate(curSkillController.gameObject);
            curSkillController = pigRushControllerInstance;
        }
    }
    
    // 返回技能剩余冷却时间，同步到手机端
    public float RemainingColdingTime()
    {
        CheckSkillController();

        float remainColdingTime = curSkillController.RemainColdingTime();
        if (remainColdingTime < 0)
            return 0;
        else
            return remainColdingTime;
    }

    // 返回技能的最大冷却时间
    public float MaxColdingTime()
    {
        CheckSkillController();

        return curSkillController.MaxColdingTime();
    }

}

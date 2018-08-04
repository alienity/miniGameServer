using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GroupPlayer : MonoBehaviour
{
    // wwq 音源
    private AudioSource selfAudioSource;

    //wwq slider
    public Slider pigSlider, penguSlider;
    //wwq effect
    public ParticleSystem dieEffect;
    // 死亡爆炸幅度
    public float exploseStrenth = 12f;
    // 重生光环
    public GameObject rebornCircleEffect;
    private GameObject rebornCircleEffectInstance;
    // 眩晕对象
    public GameObject stunEffect;

    public enum PlayerType
    {
        PENGU,  // 企鹅玩家
        PIG     // 猪玩家
    }

    // 组ID
    public int gId;
    // 活着
    private bool isAlive = true;
    // 无敌
    private bool isInvincible = false;
    // 晕眩
    private bool isSturn = false;

    // 猪对企鹅的限制角度
    public float pigLockPenguDegree = 180;

    // 死亡后重生等待时间
    [SerializeField] private float rebornDelay = 0;
    //// 重生无敌时间
    //public float rebornRestTime = 3f;

    // 组的颜色
    private Color groupColor;
    // 队伍Trans
    public Transform groupTrans;
    
    // 被某个组攻击到，该组的ID
    [SerializeField] private int attackerId = -1;
    // 重置攻击者倒计时时长
    [SerializeField] private float countdownTime = 30;
    // 重置计时
    private float countdownPast = 0;

    // 猪无敌
    [HideInInspector]
    public bool isInvulnerable { get; private set; }

    // 分数控制器
    private ScoreController scoreController;
    // 本局获得的分数
    [SerializeField] private int totalScore = 0;
    // 自杀身亡给所有的平均加分
    [SerializeField] private int suicideScore = 5;
    // 被杀身亡给凶手的加分
    [SerializeField] private int toKillerScore = 15;

    // 组内玩家
    [SerializeField] private PigPlayer pigPlayer;
    [SerializeField] private PenguPlayer penguPlayer;

    // 被撞击音效
    public AudioClip hittedAudio;
    public AudioClip rebornAudio;
    public bool IsAlive { get { return isAlive; } }
    public bool IsInvincible { get { return isInvincible; } }
    

    private void Start()
    {
        groupTrans = GetComponent<Transform>();

        if(selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();
        if (scoreController == null)
            scoreController = FindObjectOfType<ScoreController>();
        pigSlider.maxValue = pigSlider.value = pigPlayer.MaxColdingTime();
        penguSlider.maxValue =penguSlider.value = penguPlayer.MaxColdingTime();

        pigPlayer.gId = gId;
        penguPlayer.gId = gId;
        penguPlayer.SetArrowColor(groupColor);
    }

    private void Update()
    {
        if (!isAlive) return;

        // 最后攻击者倒计时
        if (countdownPast > 0)
            countdownPast -= Time.deltaTime;
        else
            attackerId = -1;
        
        // 冷却倒计时
        pigSlider.value = pigSlider.maxValue - pigPlayer.RemainingColdingTime();
        penguSlider.value = penguSlider.maxValue - penguPlayer.RemainingColdingTime();
        //Debug.Log(penguSlider.value);

        // 当猪冲撞的时候，队伍是无敌的
        isInvulnerable = pigPlayer.IsCrazy;

        // 眩晕倒计时
        pigPlayer.IsSturn = isSturn;
        penguPlayer.IsSturn = isSturn;

        stunEffect.SetActive(isSturn);

    }

    // 玩家相互碰撞后记录碰撞体是谁的
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        GroupPlayer otherGroup = collision.gameObject.GetComponent<GroupPlayer>();
    //        attackerId = otherGroup.gId;
    //        countdownPast = countdownTime;

    //        Debug.Log("两只猪相撞");

    //        if (pigPlayer.IsCrazy && !isSturn)
    //        {
    //            if (otherGroup.pigPlayer.curSkillController is PigRushController)
    //            {
    //                PigRushController pigCurSkill = otherGroup.pigPlayer.curSkillController as PigRushController;
    //                float sturnDuring = pigCurSkill.sturnDuring;
    //                StartCoroutine(SturnPlayer(sturnDuring, otherGroup));
    //                pigPlayer.StopSkillNow();
    //            }
    //        }
    //    }
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GroupPlayer otherGroup = collision.gameObject.GetComponent<GroupPlayer>();
            attackerId = otherGroup.gId;
            countdownPast = countdownTime;
            
            if (pigPlayer.IsCrazy)
            {
                if (otherGroup.pigPlayer.curSkillController is PigRushController)
                {
                    PigRushController pigCurSkill = otherGroup.pigPlayer.curSkillController as PigRushController;
                    float sturnDuring = pigCurSkill.sturnDuring;
                    StartCoroutine(SturnPlayer(sturnDuring, otherGroup));
                    pigPlayer.StopSkillNow();
                }
            }
        }
    }

    IEnumerator SturnPlayer(float during, GroupPlayer gp)
    {
        gp.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gp.isSturn = true;
        yield return new WaitForSeconds(during);
        gp.isSturn = false;
    }

    // 被攻击打中后设置攻击者是谁
    public void SetAttacker(int atkId)
    {
        attackerId = atkId;
        countdownPast = countdownTime;
    }

    // 猪无敌的时候反弹打到身上的攻击，该方法是Ball来调用的
    public void ReflectAttack(Transform atTrans)
    {
        Vector3 symmetryAxis = transform.position - atTrans.position;
        Vector3 forward = atTrans.forward;

        Vector3 newVector3 = -2 * (Vector3.Dot(forward, symmetryAxis)) / (Vector3.Dot(forward, forward)) * forward - symmetryAxis;
        newVector3.y = 0;
        Debug.Log(newVector3.ToString());
        atTrans.forward = newVector3;
    }

    // 队伍死亡
    public void Die()
    {
        isAlive = false;

        DieEffect();
        
        pigPlayer.Reset();
        
        SendViberateToGroup();
        
        Debug.Assert(scoreController != null);

        if (attackerId != -1)
        {
            if(attackerId != gId)
                scoreController.IncreaseScoreForPlayer(toKillerScore, attackerId);
        }
        else
            scoreController.IncreaseScoreForAll(suicideScore, gId);
    }


    // 首次出生
    public GroupPlayer FirstBorn(int gId, Vector3 bornPos, Color groupColor)
    {
        this.gId = gId;
        this.groupColor = groupColor;

        GroupPlayer group = Instantiate(this, bornPos, Quaternion.identity);
        group.gId = gId;
        group.groupColor = groupColor;

        return group;
    }

    // 设置ScoreController
    public void SetScoreController(ScoreController sc)
    {
        scoreController = sc;
    }

    //// 计时无敌时间的协程
    //IEnumerator InvincibleCoroutine()
    //{
    //    isInvincible = true;
    //    yield return new WaitForSeconds(rebornRestTime);
    //    isInvincible = false;
    //}

    // 在指定位置重生
    public IEnumerator ReBorn(Transform reBirthTrans)
    {
        if (isAlive != true)
        {
            //isInvincible = true;
            
            yield return new WaitForSeconds(rebornDelay);

            pigPlayer.Reset();
            
            groupTrans.position = reBirthTrans.position;

            selfAudioSource.clip = rebornAudio;
            selfAudioSource.Play();
            Debug.Log("Play");
            
            StartCoroutine(LightCircleDown());

            isAlive = true;
        }
    }
    void DieEffect()
    {
        ParticleSystem curEffect;
        curEffect = Instantiate(dieEffect) as  ParticleSystem;
        curEffect.transform.localScale = Vector3.one * exploseStrenth;
        curEffect.transform.position = gameObject.transform.position;
        Destroy(curEffect.gameObject, 1);
    }


    IEnumerator LightCircleDown()
    {
        if (rebornCircleEffectInstance == null)
            rebornCircleEffectInstance = Instantiate(rebornCircleEffect);
        
        rebornCircleEffectInstance.SetActive(true);

        float planeHight = 6f;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Plane")
            {
                planeHight = hit.point.y;
                break;
            }
        }
        
        while (transform.position.y > planeHight + 1)
        {
            Vector3 curPos = transform.position;
            rebornCircleEffectInstance.transform.position = new Vector3(curPos.x, planeHight, curPos.z);
            yield return new WaitForEndOfFrame();
        }

        rebornCircleEffectInstance.SetActive(false);
    }

    // 捡去物品并分配到各个角色，捡取成功返回true
    public bool CatchItem(SkillItem skillItem)
    {
        if (!isAlive) return false;
        
        if(skillItem.playerType == PlayerType.PIG)
        {
            pigPlayer.CatchItem(skillItem.GetPigSkillController());
        }
        else if(skillItem.playerType == PlayerType.PENGU)
        {
            penguPlayer.CatchItem(skillItem.GetPenguSkillController());
        }
        return true;
    }
    
    // 猪的控制
    public void PigMove(Vector3 dir)
    {
        if (!isAlive || isSturn) return;

        pigPlayer.SetDirection(dir.normalized);
    }
    
    // 猪受到雪球等的攻击后被击退
    public void EffectSpeedMovement(Vector3 addedSpeed)
    {
        if (!isAlive) return;
        if (selfAudioSource.clip != hittedAudio)
            selfAudioSource.clip = hittedAudio;
        selfAudioSource.Play();
        pigPlayer.ReceiveSuddenSpeed(addedSpeed);
        SendViberateToGroup();
    }

    // 受力是一个持续的过程，必须外面的方法持续调用该方法
    public void EffectForce(Vector3 addedForce)
    {
        if (!isAlive) return;

        pigPlayer.ReceiveForce(addedForce);
    }

    // 猪发动技能
    public void PigAttack()
    {
        if (!isAlive || isSturn) return;

        pigPlayer.PigPlayerAttack();
    }
    
    // 企鹅的控制
    public void PenguMove(Vector3 dir)
    {
        if (!isAlive || isSturn) return;
        float dot = Vector3.Dot(pigPlayer.pigCurDirection, dir.normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Debug.Log(angle);
        //
        if(angle < pigLockPenguDegree/2) penguPlayer.SetArrowDirection(dir.normalized);
    }
    
    // 企鹅蓄力攻击
    public void PenguChargeAttack(string processId, float currrentTime, int touchId)
    {
        if (!isAlive || isSturn) return;
    
        penguPlayer.HandleChargeSkill(processId, currrentTime, touchId);
    }
    
    // 获取猪和企鹅的数据用于返回到控制界面
    public bool IsJoysticjAvai()
    {
        if (isAlive)
            return false;
        return true;
    }

    // 返回冷却时间
    public float RemainCoolingTime(PlayerType playerType)
    {
        if(playerType == PlayerType.PENGU)
        {
            return penguPlayer.RemainingColdingTime();
        }
        else if (playerType == PlayerType.PIG)
        {
            return pigPlayer.RemainingColdingTime();
        }
        return 0;
    }

    // 返回最大冷却时间
    public float MaxCoolingTime(PlayerType playerType)
    {
        if (playerType == PlayerType.PENGU)
        {
            return penguPlayer.MaxColdingTime();
        }
        else if (playerType == PlayerType.PIG)
        {
            return pigPlayer.MaxColdingTime();
        }
        return 0;
    }

    // 返回当前获得的分数
    public int TotalScore()
    {
        return totalScore;
    }

    // 增加分数
    public void IncreaseScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
    }
    
    
    // 向猪和企鹅发送震动信息
    private void SendViberateToGroup()
    {
        int roleId = gId * 2;
        for (int i = 0; i < 2; i++)
        {
            if (Server.Instance.role2connectionID.ContainsKey(roleId + i))
            {
                NetworkServer.SendToClient(Server.Instance.role2connectionID[roleId + i], CustomMsgType.AdvanceControl,
                    new AdvanceControlMsg(AdvanceControlType.Viberate));
                Debug.Log("振动");
            }
        }
    }


}

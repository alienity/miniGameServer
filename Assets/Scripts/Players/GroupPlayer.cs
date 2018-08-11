using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GroupPlayer : MonoBehaviour
{

    public enum PlayerType
    {
        PENGU,  // 企鹅玩家
        PIG     // 猪玩家
    }

    // wwq 音源
    private AudioSource selfAudioSource;
    //wwq effect
    public ParticleSystem dieEffect;
    public ParticleSystem HitEffect;
    
    // 死亡爆炸幅度
    public float exploseStrenth = 12f;
    // 重生光环
    public GameObject rebornCircleEffect;
    private GameObject rebornCircleEffectInstance;
    // 眩晕对象
    public GameObject stunEffect;
    // 增加分数时会出现的字
    public Text addScore;
    public Text playerName;

    // 组ID
    public int gId;
    // 活着
    private bool isAlive = true;
    // 无敌
    private bool isInvincible = false;
    // 晕眩
    private bool isSturn = false;
    
    // 死亡后重生等待时间
    [SerializeField] private float rebornDelay = 0;

    // 组的颜色
    private Color groupColor = Color.black;
    
    // 被某个组攻击到，该组的ID
    private int attackerId = -1;
    // 重置攻击者倒计时时长
    public float countdownTime = 30;
    // 重置计时
    public float countdownPast = 0;
    // 反弹强度
    public float springStrength = 6f;

    // 猪无敌
    [HideInInspector] public bool isInvulnerable { get; private set; }

    // 分数控制器
    private ScoreController scoreController;
    // 本局获得的分数
    [SerializeField] private int totalScore = 0;
    // 自杀身亡给所有的平均加分
    [SerializeField] private int suicideScore = 5;
    // 被杀身亡给凶手的加分
    [SerializeField] private int toKillerScore = 15;

    // 组内玩家
    [SerializeField] public PigPlayer pigPlayer;
    [SerializeField] private PenguPlayer penguPlayer;

    // 队伍Trans
    public Transform groupTrans;

    public float scorePopupScale = 0.02f;
    public float popupDuration = 0.3f;
    public Text remainBallText;

    // 被撞击音效
    public AudioClip hittedAudio;
    public AudioClip rebornAudio;
    public bool IsAlive { get { return isAlive; } }
    public bool IsInvincible { get { return isInvincible; } }
    // 队伍在悬崖上
    public bool IsSurivalSkillNow
    {
        get { return pigPlayer.IsSurivalSkillNow; }
        set { pigPlayer.IsSurivalSkillNow = value; }
    }
    
    private void Start()
    {
        groupTrans = GetComponent<Transform>();

        if(selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();
        if (scoreController == null)
            scoreController = FindObjectOfType<ScoreController>();
        pigPlayer.gId = gId;
        penguPlayer.gId = gId;
       

        penguPlayer.SetArrowAndSnowballColor(groupColor);
        pigPlayer.SetArrowColor(groupColor);
        
        SetUpPlayerInfo();
       
    }

    private void SetUpPlayerInfo()
    {
        playerName.color = groupColor;

        string penguName = getRoleName(gId * 2);
        string pigName = getRoleName(gId * 2 + 1);
        playerName.text = ConcateName(penguName, pigName);
        Color transColor = addScore.color;
        transColor.a = 0;
        addScore.color = transColor;

        Color remainballColor = remainBallText.color;
        remainballColor.a = 0;
        remainBallText.color = remainballColor;
    }

    private string ConcateName(string first, string second)
    {
        if (first == null) first = "";
        if (second == null) second = "";
        if (first.Length > 6)
        {
            first = first.Substring(0, 6);
        }

        if (second.Length > 6)
        {
            second = second.Substring(second.Length - 6, 6);
        }

        string first_part = first.Substring(0, first.Length / 2);
        string second_part = second.Substring(second.Length / 2, second.Length/2);
        return String.Format("{0}.{1}", first_part, second_part);
    }
    
    /*
     * 辅助函数，只是用来查找角色的名字
     */
    private string getRoleName(int roleId)
    {
        if (!DataSaveController.Instance.role2session.ContainsKey(roleId))
        {
            return "";
        }

        int sessionId = DataSaveController.Instance.role2session[roleId];
        return DataSaveController.Instance.session2name[sessionId];
    }

    private void Update()
    {
        if (!isAlive) return;

        // 最后攻击者倒计时
        if (countdownPast > 0)
            countdownPast -= Time.deltaTime;
        else
            attackerId = -1;

        // 当猪冲撞的时候，队伍是无敌的
        isInvulnerable = pigPlayer.IsCrazy;

        // 眩晕倒计时
        pigPlayer.IsSturn = isSturn;
        penguPlayer.IsSturn = isSturn;

        stunEffect.SetActive(isSturn);
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GroupPlayer otherGroup = collision.gameObject.GetComponent<GroupPlayer>();
            attackerId = otherGroup.gId;
            countdownPast = countdownTime;
            
            if (pigPlayer.IsCrazy)
            {
                SendViberationToPig();
                if (otherGroup.pigPlayer.curSkillController is PigRushController)
                {
                    otherGroup.SendViberateToGroup(0.5f);

                    Vector3 curToTargetDirection = (otherGroup.transform.position - transform.position).normalized;

                    GetComponent<Rigidbody>().velocity += -1 * curToTargetDirection * springStrength;
                    otherGroup.GetComponent<Rigidbody>().velocity += curToTargetDirection * springStrength;

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
        gp.isSturn = true;
        //wwq
        Destroy(Instantiate(HitEffect,transform) as ParticleSystem, 2);
        
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
        isSturn = false;

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
            if (skillItem.GetPenguSkillController() is PickableSnowBallController)
            {
                PickableSnowBallController pickableSnowBallController = (PickableSnowBallController) skillItem.GetPenguSkillController();
                pickableSnowBallController.SetCountRemainingText(remainBallText);
                penguPlayer.CatchItem(skillItem.GetPenguSkillController());
            }
            if (skillItem.GetPenguSkillController() is PickableFireBallController)
            {
                PickableFireBallController pickableSnowBallController = (PickableFireBallController)skillItem.GetPenguSkillController();
                pickableSnowBallController.SetCountRemainingText(remainBallText);
                penguPlayer.CatchItem(skillItem.GetPenguSkillController());
            }
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

    public void PigAttack()
    {
        if (!isAlive || isSturn) return;

        pigPlayer.PigPlayerAttack();
    }
    // 猪在加速道路上的速度变化
    public void PigSpeedupRoad()
    {
        if (!isAlive) return;

        pigPlayer.PigPlayerSpeedupRoad();
    }
    // 企鹅的控制
    public void PenguMove(Vector3 dir)
    {
        if (!isAlive || isSturn) return;
        float dot = Vector3.Dot(pigPlayer.pigCurDirection, dir.normalized);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //        Debug.Log(angle);
        //
        //if (angle < pigLockPenguDegree / 2) penguPlayer.SetArrowDirection(dir.normalized);
        penguPlayer.SetArrowDirection(dir.normalized);
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
        PopupScore(scoreToAdd);

    }
    private void PopupScore(int score)
    {
        addScore.text = score.ToString();
        RectTransform rectTransform = addScore.GetComponent<RectTransform>();
        Vector3 oldScale = rectTransform.localScale;
       
        Color tempColor = addScore.color;
        tempColor.a = 1;
        addScore.color = tempColor;
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(scorePopupScale, popupDuration)).OnComplete(delegate
        {
            tempColor.a = 0;
            addScore.color = tempColor;
            rectTransform.localScale = oldScale;
        });
    }
    
    // 向猪和企鹅发送震动信息
    public void SendViberateToGroup(float duration=0.05f, float interval=0.05f)
    {
        int roleId = gId * 2;
        for (int i = 0; i < 2; i++)
        {
            SendViberationToRole(roleId + i, duration, interval);
        }
    }

    public void SendViberationToPengu(float duation=0.5f, float interval=0.5f)
    {
        SendViberationToRole(gId*2, duation, interval);
    }

    public void SendViberationToPig(float duration = 0.5f, float interval=0.5f)
    {
        SendViberationToRole(gId*2+1, duration, interval);
    }

    public void SendViberationToRole(int roleId, float duration, float interval)
    {
        if (DataSaveController.Instance.role2session.ContainsKey(roleId))
        {
            int sessionId = DataSaveController.Instance.role2session[roleId];
            int connectionId = DataSaveController.Instance.session2connection[sessionId];
            NetworkServer.SendToClient(connectionId, CustomMsgType.AdvanceControl,
                new AdvanceControlMsg(AdvanceControlType.Viberate, duration, interval ));
            Debug.Log("振动");
        }
    }


}

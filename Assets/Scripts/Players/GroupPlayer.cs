using UnityEngine;
using System.Collections;

public class GroupPlayer : MonoBehaviour
{
    // wwq 音源
    private AudioSource selfAudioSource;

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

    // 死亡后重生等待时间
    public float rebornDelay = 0;
    //// 重生无敌时间
    //public float rebornRestTime = 3f;

    // 组的颜色
    private Color groupColor;
    // 队伍Trans
    public Transform groupTrans;
    // 重生时设置地点
    public Transform bornTrans;

    // 被某个组攻击到，该组的ID
    public int attackerId = -1;
    // 重置攻击者倒计时时长
    public float countdownTime = 30;
    // 重置计时
    private float countdownPast = 0;

    // 猪无敌
    [HideInInspector]
    public bool isInvulnerable = false;

    // 分数控制器
    private ScoreController scoreController;
    // 本局获得的分数
    public int totalScore = 0;
    // 自杀身亡给所有的平均加分
    public int suicideScore = 5;
    // 被杀身亡给凶手的加分
    public int toKillerScore = 15;

    // 组内玩家
    public PigPlayer pigPlayer;
    public PenguPlayer penguPlayer;

    // 被撞击音效
    public AudioClip hittedAudio;

    public bool IsAlive { get { return isAlive; } }
    public bool IsInvincible { get { return isInvincible; } }

    private void Start()
    {
        groupTrans = GetComponent<Transform>();

        if(selfAudioSource == null)
            selfAudioSource = GetComponent<AudioSource>();

        pigPlayer.gId = gId;
        penguPlayer.gId = gId;
        penguPlayer.SetArrowColor(groupColor);
    }

    private void Update()
    {
        if (!isAlive) return;

        if (countdownPast > 0)
            countdownPast -= Time.deltaTime;
        else
            attackerId = -1;

        // 当猪冲撞的时候，队伍是无敌的
        isInvulnerable = pigPlayer.IsCrazy;
    }

    // 玩家相互碰撞后记录碰撞体是谁的
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GroupPlayer attackGroup = collision.gameObject.GetComponent<GroupPlayer>();
            attackerId = attackGroup.gId;
            countdownPast = countdownTime;
        }
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
        pigPlayer.Reset();

        Debug.Assert(scoreController != null);

        if (attackerId != -1)
            scoreController.IncreaseScoreForPlayer(toKillerScore, attackerId);
        else
            scoreController.IncreaseScoreForAll(suicideScore);
    }

    // 首次出生
    public GameObject FirstBorn(int gId, Vector3 bornPos, Color groupColor)
    {
        this.gId = gId;
        this.groupColor = groupColor;
        GameObject group = Instantiate(gameObject, bornPos, Quaternion.identity);
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
        //if (isAlive == true) return;
        //isAlive = true;
        //groupTrans.position = reBirthTrans.position;
        //StartCoroutine(InvincibleCoroutine());
        if (isAlive != true /*&& !isInvincible*/)
        {
            //isInvincible = true;
            
            yield return new WaitForSeconds(rebornDelay);

            pigPlayer.Reset();

            groupTrans.position = reBirthTrans.position;
            
            //yield return new WaitForSeconds(rebornRestTime);
            isAlive = true;
            //isInvincible = false;

            //isInvincible = true;
            //yield return new WaitForSeconds(rebornRestTime);
            //isInvincible = false;
        }
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
        if (!isAlive) return;

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
        if (!isAlive) return;

        pigPlayer.PigPlayerAttack();
    }
    
    // 企鹅的控制
    public void PenguMove(Vector3 dir)
    {
        if (!isAlive) return;

        penguPlayer.SetArrowDirection(dir.normalized);
    }

    // 企鹅蓄力攻击
    public void PenguChargeAttack(float chargeStartTime, float chargeCurrentTime, bool chargeReturn)
    {
        if (!isAlive) return;
    
        penguPlayer.HandleChargeSkill(chargeStartTime, chargeCurrentTime, chargeReturn);
    }
    
    // 获取猪和企鹅的数据用于返回到控制界面
    public bool IsJoysticjAvai()
    {
        if (isAlive)
            return false;
        return true;
    }

    // 返回冷却时间
    public float CoolingTime(PlayerType playerType)
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

}
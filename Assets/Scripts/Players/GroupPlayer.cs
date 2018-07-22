using UnityEngine;
using System.Collections;

public class GroupPlayer : MonoBehaviour
{
    
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

    //// 死亡后重生等待时间
    //public float reboenDelay = 2f;
    // 重生无敌时间
    public float rebornRestTime = 3f;

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

    public bool IsAlive { get { return isAlive; } }
    public bool IsInvincible { get { return isInvincible; } }

    void Start()
    {
        groupTrans = GetComponent<Transform>();

        pigPlayer.SetId(gId, 1);
        penguPlayer.SetId(gId, 0);
    }

    void Update()
    {
        if (!isAlive) return;

        if (countdownPast > 0)
            countdownPast -= Time.deltaTime;
        else
            attackerId = -1;

    }

    // 玩家相互碰撞后记录碰撞体是谁的
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
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

    // 队伍死亡
    public void Die()
    {
        isAlive = false;
        pigPlayer.StopNow();

        Debug.Assert(scoreController != null);

        if (attackerId != -1)
            scoreController.IncreaseScoreForPlayer(toKillerScore, attackerId);
        else
            scoreController.IncreaseScoreForAll(suicideScore);
    }

    // 首次出生
    public GameObject FirstBorn(int gId, Vector3 bornPos)
    {
        this.gId = gId;
        return Instantiate(gameObject, bornPos, Quaternion.identity);
    }

    // 设置ScoreController
    public void SetScoreController(ScoreController sc)
    {
        scoreController = sc;
    }

    // 计时无敌时间的协程
    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(rebornRestTime);
        isInvincible = false;
    }

    // 在指定位置重生
    public void ReBorn(Transform reBirthTrans)
    {
        if (isAlive == true) return;
        isAlive = true;
        groupTrans.position = reBirthTrans.position;
        StartCoroutine(InvincibleCoroutine());
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

    public void EffectSpeedMovement(Vector3 addedSpeed)
    {
        if (!isAlive) return;

        pigPlayer.ReceiveSuddenSpeed(addedSpeed);
    }

    // 受力是一个持续的过程，必须外面的方法持续调用该方法
    public void EffectForce(Vector3 addedForce)
    {
        if (!isAlive) return;

        pigPlayer.ReceiveForce(addedForce);
    }

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

    public void PenguAttack()
    {
        if (!isAlive) return;

        penguPlayer.PenguPlayerAttack();
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

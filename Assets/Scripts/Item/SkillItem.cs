using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class SkillItem : MonoBehaviour {

    // TODO : 后面重构代码

    // 手动根据玩家类别来获取技能物品
    public GroupPlayer.PlayerType playerType;
    // 要实例化的控制对象
    public GameObject skillController;
    private GameObject skillControllerInstance;
    // 现在是否有玩家正在占取这个道具
    private bool beingCaptured;
    // 正在获取道具的玩家是谁
    private GroupPlayer capturingGroupPlayer;

    // 被拾取后的回调函数
    public System.Action noticeGenerator;

	// Use this for initialization
	void Start () {
        if (skillController == null)
            Debug.Log("放置物品");
        if (skillControllerInstance == null)
            skillControllerInstance = Instantiate(skillController, transform);
    }

    // Todo 如果有多个玩家同时站在道具的占领区，可能会出现问题
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(!beingCaptured && other.tag == "Player")
        {
            beingCaptured = true;
            GroupPlayer groupPlayer = other.GetComponent<GroupPlayer>();
            capturingGroupPlayer = groupPlayer;
            if (groupPlayer.CatchItem(this))
            {
                if (noticeGenerator != null)
                {
                    noticeGenerator();
                    noticeGenerator = null;
                }
                Destroy(gameObject);
            }
        }
    }

    public PigSkillController GetPigSkillController()
    {
        return skillControllerInstance.GetComponent<PigSkillController>();
    }

    public ShotBallController GetPenguSkillController()
    {
        return skillControllerInstance.GetComponent<ShotBallController>();
    }

}

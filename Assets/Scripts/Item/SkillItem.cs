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
    
    // 现在是否有玩家正在占取这个道具
    private bool beingCaptured;
    // 正在获取道具的玩家是谁
    private GroupPlayer capturingGroupPlayer;

	// Use this for initialization
	void Start () {
        if (skillController == null)
            Debug.Log("放置物品");
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
                Destroy(gameObject);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    /*
    //     * 只要当正在占领的玩家处在占领道具的区域内，就对占领时间进行计时
    //     * 到达占领时间后让玩家取得该道具，如果玩家不能取得该道具，则重置道具状态
    //     */
    //    if (beingCaptured)
    //    {
    //        capturingTime += Time.deltaTime;
    //        if (capturingTime > timeToCapture)
    //        {
    //            if (capturingGroupPlayer.CatchItem(this))
    //            {
    //                Destroy(gameObject);
    //            }
    //            else
    //            {
    //                beingCaptured = false;
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (beingCaptured)
    //    {
    //        beingCaptured = false;
    //        capturingTime = 0.0f;
    //        capturingGroupPlayer = null;
    //    }

    //}

    public PigSkillController GetPigSkillController()
    {
        return Instantiate(skillController.GetComponent<PigSkillController>());
    }

    public ShotBallController GetPenguSkillController()
    {
        return Instantiate(skillController.GetComponent<ShotBallController>());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour {

    // TODO : 后面重构代码

    // 手动根据玩家类别来获取技能物品
    public GroupPlayer.PlayerType playerType;
    // 要实例化的控制对象
    public GameObject skillController;

	// Use this for initialization
	void Start () {
        if (skillController == null)
            Debug.Log("放置物品");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.tag == "Player")
        {
            GroupPlayer groupPlayer = other.GetComponent<GroupPlayer>();
            if (groupPlayer.CatchItem(this))
                Destroy(gameObject);
        }
    }
    
    public PigSkillController GetPigSkillController()
    {
        return Instantiate(skillController.GetComponent<PigSkillController>());
    }

    public ShotBallController GetPenguSkillController()
    {
        return Instantiate(skillController.GetComponent<ShotBallController>());
    }

}

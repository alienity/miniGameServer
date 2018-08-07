using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomMap : MonoBehaviour {

	private int[] pos;
    public SkillItem skillItem;
    public int cloneNum;
    private int clonedNum;
    private System.Random rand;
    public List<Transform> reBorns;
    public float reBornCD;
    private float reTimes;

    void Start () {
        reTimes = -1;
        pos = new int[10];
        clonedNum = 0;
        for (int i = 0; i < 9; i++)
            pos[i] = i;
        rand = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
        if (reTimes < 0)
        {
            if (clonedNum < cloneNum)
            {
                int rpos = rand.Next(9 - clonedNum);
                int x = pos[rpos];
                setMap(x);
                clonedNum++;

                pos[rpos] += pos[9 - clonedNum];
                pos[9 - clonedNum] = pos[rpos] - pos[9 - clonedNum];
                pos[rpos] = pos[rpos] - pos[9 - clonedNum];
                //x;
            }
        }
        else reTimes -= Time.deltaTime;
	}

    // 
    private void setMap(int idx)
    {
        GameObject curSkillItemObj = Instantiate(skillItem.gameObject, reBorns[idx]);
        curSkillItemObj.transform.position = reBorns[idx].transform.position;
        curSkillItemObj.transform.rotation = reBorns[idx].transform.rotation;
        curSkillItemObj.GetComponent<SkillItem>().noticeGenerator = getObject;
    }

    // 物品被拾取后的回调函数
    public void getObject()
    {
        clonedNum--;
        reTimes = reBornCD;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomMap : MonoBehaviour {

	private int[] pos;
    public GameObject cloneObject;
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
            int len = reBorns.Count;
            if (clonedNum < cloneNum)
            {
                int rpos = rand.Next(len - clonedNum);
                int x = pos[rpos];
                setMap(x);
                clonedNum++;

                pos[rpos] += pos[len - clonedNum];
                pos[len - clonedNum] = pos[rpos] - pos[len - clonedNum];
                pos[rpos] = pos[rpos] - pos[len - clonedNum];
                //x;
            }
        }
        else reTimes -= Time.deltaTime;
	}
    private void setMap(int idx)
    {
        Instantiate(cloneObject,reBorns[idx]);
        
    }
    public void getObject()
    {
        clonedNum--;
        reTimes = reBornCD;
    }
}

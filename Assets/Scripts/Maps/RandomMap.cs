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
    void Start () {
        pos = new int[10];
        clonedNum = 0;
        for (int i = 0; i < 9; i++)
            pos[i] = i;
        rand = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
        if (clonedNum <cloneNum)
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
    private void setMap(int idx)
    {
        Instantiate(cloneObject,new Vector3(idx / 3 + 2.5f, 0, idx % 3 + 2.5f), Quaternion.identity);
    }
    public void getObject()
    {
        clonedNum--;
    }
}

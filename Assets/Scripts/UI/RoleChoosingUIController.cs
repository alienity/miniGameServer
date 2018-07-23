using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RoleChoosingUIController : MonoBehaviour {
    
    //Make sure to attach these Buttons in the Inspector
    public Button P1Pengu;
    public Button P1Pig;
    public Button P2Pengu;
    public Button P2Pig;
    public Button P3Pengu;
    public Button P3Pig;
    public Button P4Pengu;
    public Button P4Pig;

    private Button[] buttons;
    
    private void Start()
    {
        InitArray();
    }

    private void InitArray()
    {
        buttons = new Button[8];
        buttons[0] = P1Pengu;
        buttons[1] = P1Pig;
        buttons[2] = P2Pengu;
        buttons[3] = P2Pig;
        buttons[4] = P3Pengu;
        buttons[5] = P3Pig;
        buttons[6] = P4Pengu;
        buttons[7] = P4Pig;

        foreach (Button button in buttons)
        {
            button.image.color = Color.white;
        }

    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleAvailable(int gid, int uid)
    {
        Debug.Log(gid+ " " + uid + " available");

        buttons[gid*2 + uid].interactable = true;
        buttons[gid * 2 + uid].image.color = DataSaveController.Instance.groupColor[gid];
    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleSelected(int gid, int uid)
    {
        Debug.Log(gid + " " + uid + " selected");
        buttons[gid*2 + uid].interactable = false;
    }
    
}

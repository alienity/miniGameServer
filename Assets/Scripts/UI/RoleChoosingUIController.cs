using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RoleChoosingUIController : MonoBehaviour {

    // 两个Canvas
    [Header("Canvas")]
    public GameObject StartCanvas;
    public GameObject ChooseRoleCanvas;

    // 当前连入Server的Client人数滑动进度条 
    [Header("ProcessBar")]
    public Image BlueProgressBar;
    // 进度显示分数（1/8）
    public Text FractionalText;
    // ChooseRoleScene的Button
    public Button P1Pengu;
    public Button P1Pig;
    public Button P2Pengu;
    public Button P2Pig;
    public Button P3Pengu;
    public Button P3Pig;
    public Button P4Pengu;
    public Button P4Pig;
    private Button[] buttons;
    // 进入GameScene前的倒计时
    public Text CountDown;
    
    
    private void Start()
    {
        InitArray();
        CountDown.gameObject.SetActive(false);

        // 初始场景切换
        StartCanvas.SetActive(true);
        ChooseRoleCanvas.SetActive(false);
    }

    // StartCanvas进度条显示
    public void ProgressBarPlay(int numerator, int denominator)
    {
        BlueProgressBar.fillAmount = (float)numerator / (float)denominator;
        FractionalText.text = numerator.ToString() + " / " + denominator.ToString();
    }
    // 切换Canvas
    public void changeCanvas()
    {
        // 切换至选人界面
        StartCanvas.SetActive(false);
        ChooseRoleCanvas.SetActive(true);
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
        //buttons[gid * 2 + uid].image.color = DataSaveController.Instance.groupColor[gid];
        buttons[gid * 2 + uid].image.color = Color.white;
       
    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleSelected(int gid, int uid)
    {
        Debug.Log(gid + " " + uid + " selected");
        //buttons[gid*2 + uid].interactable = false;
        buttons[gid * 2 + uid].image.color = DataSaveController.Instance.groupColor[gid];
    }

    public void CountDownTextSetActive()
    {
        CountDown.gameObject.SetActive(true);
    }

    public void CountDownPlay(int time)
    {
        CountDown.text = time.ToString();
    }
}

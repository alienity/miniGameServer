using System.Collections;
using System.Collections.Generic;
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
    public Sprite PenguHeadAltern;
    public Sprite PenguHeadSelect;
    public Sprite PenguHeadLock;
    public Sprite PigHeadAltern;
    public Sprite PigHeadSelect;
    public Sprite PigHeadLock;
    private Button[] buttons;
    // 进入GameScene前的倒计时
    public Text CountDown;
    
    // 改名字的部分
    public List<Text> playerNames;
    private List<string> defaultNames = new List<string>()
    {
        "唐吉诃鹅 - P1",
        "罗齐南猪 - P1",
        "唐吉诃鹅 - P2",
        "罗齐南猪 - P2",
        "唐吉诃鹅 - P3",
        "罗齐南猪 - P3",
        "唐吉诃鹅 - P4",
        "罗齐南猪 - P4",
    };
    
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

        for(int i = 0; i < 4; ++i )
        {
            //button.image.color = Color.white;D:\github_MiniGame\miniGameServer\Assets\Resource\Textures\ChooseRoleSceneICon
            buttons[2 * i].GetComponent<Image>().sprite = PenguHeadAltern;
            buttons[2 * i + 1].GetComponent<Image>().sprite = PigHeadAltern;
        }

    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleAvailable(int gid, int uid)
    {

        buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadAltern : PigHeadAltern;
        //buttons[gid*2 + uid].interactable = true;
        //buttons[gid * 2 + uid].image.color = DataSaveController.Instance.groupColor[gid];
        //buttons[gid * 2 + uid].image.color = Color.white;

    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleSelected(int gid, int uid)
    {
        //buttons[gid*2 + uid].interactable = false;
        //buttons[gid * 2 + uid].image.color = DataSaveController.Instance.groupColor[gid];
        buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadSelect : PigHeadSelect;
    }

    public void SetButtonRoleLocked(int gid, int uid)
    {
        buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadLock : PigHeadLock;
    }

    public void CountDownTextSetActive()
    {
        CountDown.gameObject.SetActive(true);
    }

    public void CountDownPlay(int time)
    {
        CountDown.text = time.ToString();
    }

    public void SetRoleNames(Dictionary<int, string> role2Name)
    {
        for (int i = 0; i < 8; i++)
        {
            playerNames[i].text = role2Name.ContainsKey(i) ? role2Name[i] : defaultNames[i];
        }
    }
}

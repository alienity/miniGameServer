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

    [System.Serializable]
    public struct PlayerIcone
    {
        public Image HeadSelect;
        public Image ConfirmMaskImage;
        public Text NameText;
        public Image ConfirmFrameImage;
        public Image ChooseFrameImage;
        public Image CrownImage;
        public Button button;
    }
    public List<PlayerIcone> playerIcone = new List<PlayerIcone>();
    private Button[] buttons;
    // 进入GameScene前的倒计时
    public Text CountDown;
    public AudioClip CountDownAudio;
    public AudioClip CountDownOverAudio;
    // Title 请选择你的英雄和战队
    public Text Title; 
    
    // 改名字的部分
    public List<Text> playerNames;
    private List<string> defaultNames = new List<string>()
    {
        "企鹅-P1",
        "猪猪-P1",
        "企鹅-P2",
        "猪猪-P2",
        "企鹅-P3",
        "猪猪-P3",
        "企鹅-P4",
        "猪猪-P4"
    };

    private void Start()
    {
        Title.gameObject.SetActive(true);
        CountDown.gameObject.SetActive(false);

        // 初始场景切换
//        StartCanvas.SetActive(true);
        ChooseRoleCanvas.SetActive(false);
    }

    // StartCanvas进度条显示
    public void ProgressBarPlay(int numerator, int denominator)
    {
        BlueProgressBar.fillAmount = (float)numerator / (float)denominator;
        FractionalText.text = numerator.ToString() + "/" + denominator.ToString();
    }
    // 切换Canvas
    public void changeCanvas()
    {
        // 切换至选人界面
        StartCanvas.SetActive(false);
        ChooseRoleCanvas.SetActive(true);
    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleAvailable(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(false);
        playerIcone[gid * 2 + uid].NameText.text = defaultNames[gid * 2 + uid];
        playerIcone[gid * 2 + uid].ChooseFrameImage.gameObject.SetActive(false);
    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleSelected(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(true);
        playerIcone[gid * 2 + uid].ConfirmMaskImage.gameObject.SetActive(false);
        //playerIcone[gid * 2 + uid].NameText.text = playerName;
        playerIcone[gid * 2 + uid].ConfirmFrameImage.gameObject.SetActive(false);
    }

    public void SetButtonRoleLocked(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(true);
        //playerIcone[gid * 2 + uid].ConfirmFrameImage.gameObject.SetActive(true);
    }

    public void CountDownTextSetActive()
    {
        Title.gameObject.SetActive(false);
        CountDown.gameObject.SetActive(true);
        CountDown.gameObject.GetComponent<AudioSource>().clip = CountDownAudio;
        CountDown.gameObject.GetComponent<AudioSource>().Play();
    }

    public void CountDownPlay(int time)
    {
        CountDown.text = time.ToString();
        if(time < 1)
        {
            CountDown.gameObject.GetComponent<AudioSource>().clip = CountDownOverAudio;
            CountDown.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void SetRoleNames(Dictionary<int, string> role2Name)
    {
        for (int i = 0; i < 8; i++)
        {
            playerIcone[i].NameText.text = role2Name.ContainsKey(i) ? role2Name[i] : defaultNames[i];
        }
    }
}

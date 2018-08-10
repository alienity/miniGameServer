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
    //public Button P1Pengu;
    //public Button P1Pig;
    //public Button P2Pengu;
    //public Button P2Pig;
    //public Button P3Pengu;
    //public Button P3Pig;
    //public Button P4Pengu;
    //public Button P4Pig;
    //public Sprite PenguHeadAltern;
    //public Sprite PenguHeadSelect;
    //public Sprite PenguHeadLock;
    //public Sprite PigHeadAltern;
    //public Sprite PigHeadSelect;
    //public Sprite PigHeadLock;

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
        InitArray();
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
    private void InitArray()
    {
        buttons = new Button[8];

        int i = 0;
        foreach (PlayerIcone pIcone in playerIcone)
        {
            // 主机端不需要button
            buttons[i].interactable = false;
            buttons[i++] = pIcone.button;
        }
        //buttons[0] = P1Pengu;
        //buttons[1] = P1Pig;
        //buttons[2] = P2Pengu;
        //buttons[3] = P2Pig;
        //buttons[4] = P3Pengu;
        //buttons[5] = P3Pig;
        //buttons[6] = P4Pengu;
        //buttons[7] = P4Pig;

        //for(int i = 0; i < 4; ++i )
        //{
        //    //button.image.color = Color.white;D:\github_MiniGame\miniGameServer\Assets\Resource\Textures\ChooseRoleSceneICon
        //    buttons[2 * i].GetComponent<Image>().sprite = PenguHeadAltern;
        //    buttons[2 * i + 1].GetComponent<Image>().sprite = PigHeadAltern;
        //}

    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleAvailable(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(false);
        playerIcone[gid * 2 + uid].NameText.text = defaultNames[gid * 2 + uid];
        playerIcone[gid * 2 + uid].ChooseFrameImage.gameObject.SetActive(false);
        //playerIcone[gid * 2 + uid].CrownImage.gameObject.SetActive(false);
        //BarImages[gid].gameObject.SetActive(false);
        //buttons[gid * 2 + uid].interactable = true;
        // 旧版本
        //buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadAltern : PigHeadAltern;
    }

    // todo 到时候在这里为 button 设置效果
    public void SetButtonRoleSelected(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(true);
        playerIcone[gid * 2 + uid].ConfirmMaskImage.gameObject.SetActive(false);
        //playerIcone[gid * 2 + uid].NameText.text = playerName;
        playerIcone[gid * 2 + uid].ConfirmFrameImage.gameObject.SetActive(false);
        //playerIcone[gid * 2 + uid].ChooseFrameImage.gameObject.SetActive(true);
        //playerIcone[gid * 2 + uid].CrownImage.gameObject.SetActive(false);
        // 旧版本
        // buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadSelect : PigHeadSelect;
    }

    public void SetButtonRoleLocked(int gid, int uid)
    {
        playerIcone[gid * 2 + uid].HeadSelect.gameObject.SetActive(true);
        playerIcone[gid * 2 + uid].ConfirmFrameImage.gameObject.SetActive(true);
        //playerIcone[gid * 2 + uid].ChooseFrameImage.gameObject.SetActive(false);
        //playerIcone[gid * 2 + uid].CrownImage.gameObject.SetActive(true);
        // 旧版本
        //buttons[gid * 2 + uid].GetComponent<Image>().sprite = (uid == 0) ? PenguHeadLock : PigHeadLock;
    }

    public void CountDownTextSetActive()
    {
        Title.gameObject.SetActive(false);
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
            playerIcone[i].NameText.text = role2Name.ContainsKey(i) ? role2Name[i] : defaultNames[i];
            // 旧版本
            //playerNames[i].text = role2Name.ContainsKey(i) ? role2Name[i] : defaultNames[i];
        }
    }
}

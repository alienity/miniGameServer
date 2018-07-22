using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour {

    public static GameOverUIController Instance { get; private set; }

    public List<Text> groupScores;
    public List<Image> groupImage;
    public Image winnerImage;
    public Text CountdownText;
    // 倒计时时长
    public float totalCountdownTime = 10f;

    private DataSaveController dataSaveController;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        dataSaveController = DataSaveController.Instance;
        if(dataSaveController != null)
            ShowScores();
    }

    //// Update is called once per frame
    void Update()
    {
        if (totalCountdownTime > 0f)
        {
            totalCountdownTime -= Time.deltaTime;
            UpdateCountdownTime(totalCountdownTime);
        }
        else
        {
            // TODO:切换回开始场景
        }
    }

    private void ShowScores()
    {
        int groupNumbers = dataSaveController.playerNumber / 2;
        int winnerId = 0;
        int winnerScores = 0;
        for (int i = 0; i < groupNumbers; ++i)
        {
            groupScores[i].gameObject.SetActive(true);
            groupScores[i].text = "得分："+ dataSaveController.scores[i].ToString();
            // TODO:切换为显示用户头像
            groupImage[i].color = dataSaveController.groupColor[i];
            if(winnerScores < dataSaveController.scores[i])
            {
                winnerScores = dataSaveController.scores[i];
                winnerId = i;
            }
        }
        winnerImage.color = dataSaveController.groupColor[winnerId];
    }

    // 更新倒计时
    private void UpdateCountdownTime(float remainTimes)
    {
        int remainTimeInteger = (int)remainTimes;
        CountdownText.text = remainTimeInteger.ToString();
    }
    
}

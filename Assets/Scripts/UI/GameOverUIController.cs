using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour {

    public static GameOverUIController Instance { get; private set; }

    public List<Sprite> WinnerFlagSprites;
    public List<Sprite> OthersFlagSprites;
    public List<Sprite> BackgroundSprites;

    [System.Serializable]
    public struct PlayerNameScoreOutput
    {
        public Image FlagImage;
        public Text  panguName;
        public Text  pigName;
        public Text  score;
    }
    public List<PlayerNameScoreOutput> playerNameScore;

    public Text CountdownText;
    public AudioClip CountDownAudio;
    public AudioClip CountDownOverAudio;
    public Image BackgroundImage;
    // 倒计时时长
    public float totalCountdownTime = 10f;

    private DataSaveController dataSaveController;
    private void Awake()
    {
        Instance = this;
        BackgroundImage.sprite = BackgroundSprites[0];
        playerNameScore[0].FlagImage.sprite = WinnerFlagSprites[0]; // 初始设置，防止显示为空
        playerNameScore[1].FlagImage.sprite = OthersFlagSprites[1];
        playerNameScore[2].FlagImage.sprite = OthersFlagSprites[2];
        playerNameScore[3].FlagImage.sprite = OthersFlagSprites[3];
        for (int i = 0; i < playerNameScore.Count; ++i)
        {
            //playerNameScore[i].panguName.text = "";
            //playerNameScore[i].pigName.text = "";
            playerNameScore[i].score.text = "0";
        }
    }
    private void Start()
    {
        CountdownText.gameObject.GetComponent<AudioSource>().clip = CountDownAudio;
        CountdownText.gameObject.GetComponent<AudioSource>().Play();
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
    
    // 更新倒计时
    private void UpdateCountdownTime(float remainTimes)
    {
        int remainTimeInteger = (int)remainTimes;
        CountdownText.text = remainTimeInteger.ToString();
        if (remainTimeInteger < 1)
        {
            CountdownText.gameObject.GetComponent<AudioSource>().clip = CountDownOverAudio;
            CountdownText.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void gameResultsDisplay(List<KeyValuePair<int, int>> lst, int groupNumbers)
    {
        // TODO: 用户名替换成真实用户名
        for (int i = 0; i < groupNumbers; ++i)
        {
            if (i == 0)
            {
                BackgroundImage.sprite = BackgroundSprites[lst[i].Key];
                playerNameScore[i].FlagImage.sprite = WinnerFlagSprites[lst[i].Key];
                playerNameScore[i].panguName.text = "P" + (lst[i].Key + 1).ToString();
                playerNameScore[i].pigName.text = "P" + (lst[i].Key + 1).ToString();
                playerNameScore[i].score.text = lst[i].Value.ToString();
            }
            else
            {
                playerNameScore[i].FlagImage.sprite = OthersFlagSprites[lst[i].Key];
                playerNameScore[i].panguName.text = "P" + (lst[i].Key + 1).ToString();
                playerNameScore[i].pigName.text = "P" + (lst[i].Key + 1).ToString();
                playerNameScore[i].score.text = lst[i].Value.ToString();
            }
        }
    }

}

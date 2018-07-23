using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour {

    public static GameOverUIController Instance { get; private set; }

    [System.Serializable]
    public struct PlayerNameScoreOutput
    {
        public Text  name;
        public Text  score;
    }
    
    public List<PlayerNameScoreOutput> playerNameScore;
    public Image winnerImage;
    public Text CountdownText;
    // 倒计时时长
    public float totalCountdownTime = 10f;

    private DataSaveController dataSaveController;
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < playerNameScore.Count; ++i)
        {
            playerNameScore[i].name.text = "";
            playerNameScore[i].score.text = "";
        }
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
    }
    
}

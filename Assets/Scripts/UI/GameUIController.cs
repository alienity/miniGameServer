using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class GameUIController : MonoBehaviour
{

    //public static GameUIController Instance { get; private set; }
    
    [System.Serializable]
    public struct GroupColdingTime
    {
        public Canvas groupPlayersImage;
        public Text penguColdingTime;
        public Text pigColdingTime;
        public Text groupScores;
    }

    // 渐变界面
    public Image blackPanel;
    // 所有的玩家冷却数据显示 pengu/pig
    public List<GroupColdingTime> coldingText;
    // 剩余游戏时长计时显示
    public Text countdownText;

    //private void Awake()
    //{
    //    Instance = this;
    //}

    //// Use this for initialization
    //private void Start()
    //{
        
    //}

    //// Update is called once per frame
    //private void Update()
    //{
        
    //}

    // 渐入
    public void DoFade(float endValue, float during)
    {
        //blackPanel.gameObject.SetActive(true);
        //blackPanel.DOFade(0, during);
        blackPanel.DOFade(endValue, during);
    }
    
    // 初始化组的数量和图标
    public void SetGroupInitial(int gps)
    {
        if (gps > coldingText.Count) return;
        for (int i = 0; i < gps; ++i)
            coldingText[i].groupPlayersImage.gameObject.SetActive(true);
    }

    // 更新分数数据
    public void UpdateScores(int gId, int score)
    {
        coldingText[gId].groupScores.text = score.ToString();
    }

    // 更新cd数据
    public void UpdateColdingTime(int gId, int uId, float time)
    {
        if(uId == (int)GroupPlayer.PlayerType.PENGU)
        {
            coldingText[gId].penguColdingTime.text = TimeToShow(time);
        }
        else if (uId == (int)GroupPlayer.PlayerType.PIG)
        {
            coldingText[gId].pigColdingTime.text = TimeToShow(time);
        }
    }

    // 更新计时数据
    public void UpdateRemainTimes(float remainTimes)
    {
        countdownText.text = TimeToShow(remainTimes);
    }


    private string TimeToShow(float time)
    {
        //return time > 1.0f ? time.ToString("F0") : time.ToString("F1");
        int secends = (int)time;
        return (secends / 60).ToString() + ":" + (secends % 60).ToString();
        //return secends.ToString();
    }

}

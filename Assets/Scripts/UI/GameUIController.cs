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
        public Image fireICone;
        public Text groupScores;
    }

    // 渐变界面
    public Image blackPanel;
    // 所有的玩家冷却数据显示 pengu/pig
    public List<GroupColdingTime> coldingText;
    // 剩余游戏时长计时显示
    public Text countdownText;
    public Image countdownShade;

    //private void Awake()
    //{
    //    Instance = this;
    //}

    // 渐入
    public void DoFade(float endValue, float during)
    {
        //blackPanel.gameObject.SetActive(true);
        //blackPanel.DOFade(0, during);
        blackPanel.DOFade(endValue, during);
    }
    
    // 初始化组的数量和图标
    /*
    public void SetGroupInitial(int gps)
    {
        if (gps > coldingText.Count) return;
        for (int i = 0; i < gps; ++i)
            coldingText[i].groupPlayersImage.gameObject.SetActive(true);
    }
    */
    // 初始化分数数据
    public void InitScores()
    {
        foreach(GroupColdingTime gct in coldingText)
        {
            gct.fireICone.gameObject.SetActive(false);
            gct.groupScores.text = "0";
        }
    }
    // 更新分数数据
    public void UpdateScores(int gId, int score)
    {
        coldingText[gId].groupScores.text = score.ToString();
    }

    // 更新cd数据
    /*
    public void UpdateColdingTime(int gId, int uId, float time, float MaxCoolingTime)
    {
        if(uId == (int)GroupPlayer.PlayerType.PENGU)
        {
            //coldingText[gId].penguColdingTime.text = TimeToShow(time);
            // TODO: 这里没写完 缺少一个获取冷却总时间获取，
            coldingText[gId].penguColdingImage.fillAmount = time / MaxCoolingTime;
        }
        else if (uId == (int)GroupPlayer.PlayerType.PIG)
        {
            //coldingText[gId].pigColdingTime.text = TimeToShow(time);
            coldingText[gId].pigColdingImage.fillAmount = time / MaxCoolingTime;
        }
    }
    */

    // 更新计时数据
    public void UpdateRemainTimes(float totalGameTime, float remainTimes)
    {
        countdownShade.fillAmount = remainTimes / totalGameTime;
        countdownText.text = TimeToShow(remainTimes);
    }

    private string TimeToShow(float time)
    {
        //return time > 1.0f ? time.ToString("F0") : time.ToString("F1");
        int secends = (int)time;
        //return (secends / 60).ToString() + ":" + (secends % 60).ToString();
        string str = (secends / 60).ToString() + ":";
        if ((secends % 60) >= 10)
            str += (secends % 60).ToString();
        else
            str += "0" + (secends % 60).ToString();

        return str;
    }

    // 更新“火苗”ICone在积分第一的玩家头上
    public void updateFireICone(List<KeyValuePair<int, int>> IDandScoreList, int winnerID, int winnerScore)
    {
        int id = 0;
        foreach (KeyValuePair<int, int> lst in IDandScoreList)
        {
            if (lst.Value >= winnerScore)
                coldingText[lst.Key].fireICone.gameObject.SetActive(true);
            else
                coldingText[lst.Key].fireICone.gameObject.SetActive(false);
        }

    }
}

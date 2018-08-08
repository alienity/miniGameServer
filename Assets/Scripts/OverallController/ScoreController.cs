﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class ScoreController : MonoBehaviour
{

    public List<GroupPlayer> groupPlayers;
    
    // 获取所有的组，并将对象加入到所有组
    public void SetGroupPlayers(List<GroupPlayer> groups)
    {
        groupPlayers = groups;
        foreach (GroupPlayer gp in groupPlayers)
            gp.SetScoreController(this);
    }

    // 获取所有玩家的分数
    public List<int> GetAllScores()
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < groupPlayers.Count; ++i)
            scores.Add(GetScore(i));
        return scores;
    }

    // 根据gId获取分数
    public int GetScore(int gId)
    {
        return groupPlayers[gId].TotalScore();
    }

    // 为某个玩家增加分数
    public void IncreaseScoreForPlayer(int score, int gId)
    {
        groupPlayers[gId].IncreaseScore(score);
        PopupScore(score, gId);
    }

    private void PopupScore(int score, int gId)
    {
        groupPlayers[gId].addScore.text = score.ToString();
        RectTransform rectTransform = groupPlayers[gId].addScore.GetComponent<RectTransform>();
       
        Color tempColor = groupPlayers[gId].addScore.color;
        tempColor.a = 1;
        groupPlayers[gId].addScore.color = tempColor;
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(2, 0.5f)).OnComplete(delegate
        {
            tempColor.a = 0;
            groupPlayers[gId].addScore.color = tempColor;
            rectTransform.localScale = Vector3.one;
        });
//        seq.Append(groupPlayers[gId].addScore.DOFade())
    }

    // 为所有玩家增加分数
    public void IncreaseScoreForAll(int score, int excludeGroupId)
    {
        for (int gId = 0; gId < groupPlayers.Count; ++gId)
        {
            if(gId != excludeGroupId)
                IncreaseScoreForPlayer(score, gId);
        }
    }
    
}

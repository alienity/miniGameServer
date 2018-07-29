using UnityEngine;
using System.Collections.Generic;

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
    }

    // 为所有玩家增加分数
    public void IncreaseScoreForAll(int score)
    {
        for (int gId = 0; gId < groupPlayers.Count; ++gId)
        {
            IncreaseScoreForPlayer(score, gId);
        }
    }
    
}

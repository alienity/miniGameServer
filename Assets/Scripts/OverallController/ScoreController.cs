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

    // 根据gId获取分数
    public int GetScore(int gId)
    {
        return groupPlayers[gId].TotalScore();
    }

    public void IncreaseScoreForPlayer(int score, int gId)
    {
        groupPlayers[gId].IncreaseScore(score);
    }

    public void IncreaseScoreForAll(int score)
    {
        for (int gId = 0; gId < groupPlayers.Count; ++gId)
        {
            IncreaseScoreForPlayer(score, gId);
        }
    }

}

using AllInOneMobile;
using UnityEngine;
using UnityEngine.UI;

public class TestAch : MonoBehaviour
{
    public Text text;
    int score = 0;
    
    public void Increment()
    {
        Achievements.Increment(GPGSIds.achievement_veteran);
    }

    public void Reveal()
    {
        Achievements.Reveal(GPGSIds.achievement_rich_man);
    }

    public void Unlock()
    {
        Achievements.Unlock(GPGSIds.achievement_good_start);
    }

    public void ShowUI()
    {
        Achievements.ShowUI();
    }

    public void PostScore()
    {
        Leaderboards.ReportScore(GPGSIds.leaderboard_distance, score);
    }

    public void AddScore()
    {
        score++;
        text.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        text.text = score.ToString();
    }

    public void ShowLeaderboards()
    {
        Leaderboards.ShowUI();
    }
}

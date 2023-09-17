using UnityEngine;
using TMPro;

public class GameScoreManager : SingletonWithMonobehaviour<GameScoreManager>
{
    [SerializeField] private TextMeshProUGUI noOfTurnsValueText;
    [SerializeField] private TextMeshProUGUI noOfMatchedValueText;

    private int noOfTurns = 0;
    private int noOfMatched = 0;

    public void UpdateTurnsScore(int score)
    {
        noOfTurns += score;
        noOfTurnsValueText.text = score.ToString();
    }

    public void UpdateMatchedScore(int matched)
    {
        noOfMatched += matched;
        noOfMatchedValueText.text = matched.ToString();
    }

    public int GetNoOfTurnsScore()
    {
        return noOfTurns;
    }

    public int GetNoOfMatchedScore()
    {
        return noOfMatched;
    }

    public void ResetScores()
    {
        noOfTurns = 0;
        noOfMatched = 0;
    }
}

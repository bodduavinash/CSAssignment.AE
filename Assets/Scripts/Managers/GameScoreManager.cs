using UnityEngine;
using TMPro;

public class GameScoreManager : SingletonWithMonobehaviour<GameScoreManager>, IGameDataPersistence
{
    [SerializeField] private TextMeshProUGUI noOfTurnsValueText;
    [SerializeField] private TextMeshProUGUI noOfMatchedValueText;

    private int noOfTurns = 0;
    private int noOfMatched = 0;

    public void UpdateTurnsScore(int score)
    {
        noOfTurns += score;
        noOfTurnsValueText.text = noOfTurns.ToString();
    }

    public void UpdateMatchedScore(int matched)
    {
        noOfMatched += matched;
        noOfMatchedValueText.text = noOfMatched.ToString();
    }

    public void ResetScores()
    {
        noOfTurns = 0;
        noOfMatched = 0;
    }

    public void LoadGameData(GameData gameData)
    {
        noOfTurns = gameData.noOfTurns;
        noOfMatched = gameData.noOfMatched;

        noOfTurnsValueText.text = noOfTurns.ToString();
        noOfMatchedValueText.text = noOfMatched.ToString();
    }

    public void SaveGameData(ref GameData gameData)
    {
        gameData.noOfTurns = noOfTurns;
        gameData.noOfMatched = noOfMatched;
    }
}

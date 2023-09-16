using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuController : MonoBehaviour
{
    private GameSceneManager _gameSceneManager;
    
    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);

        GetComponent<Button>().onClick.AddListener(OnPlayAgainButtonClicked);
    }

    private void OnPlayAgainButtonClicked()
    {
        _gameSceneManager.LoadScene(GameScenesEnum.LayoutMenuScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

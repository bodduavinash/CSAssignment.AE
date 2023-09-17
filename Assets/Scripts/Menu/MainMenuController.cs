using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button LoadButton;
    [SerializeField] private Button ExitButton;

    private GameSceneManager _gameSceneManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);

        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        LoadButton.onClick.AddListener(OnLoadButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);

        CheckIfGameDataIsLoaded();
    }

    private void CheckIfGameDataIsLoaded()
    {
        _dataPersistenceManager.LoadGameData();
        LoadButton.interactable = _dataPersistenceManager.IsGameDataLoaded();
    }

    private void OnPlayButtonClicked()
    {
        _gameSceneManager.LoadNextScene();
    }

    private void OnLoadButtonClicked()
    {
        _dataPersistenceManager.LoadGameData();
        _gameSceneManager.LoadScene(GameScenesEnum.GameScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
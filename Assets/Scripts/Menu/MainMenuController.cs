using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button PlayButton;

    private GameSceneManager _gameSceneManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);

        PlayButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        _gameSceneManager.LoadNextScene();
    }
}
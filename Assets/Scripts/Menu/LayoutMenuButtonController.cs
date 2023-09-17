using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutMenuButtonController : MonoBehaviour
{
    [SerializeField] private CardLayoutsEnum selectedGameLayout;

    private GameSceneManager _gameSceneManager;
    private UIManager _uiManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnLayoutButtonClicked());

        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _uiManager = (UIManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.UIManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);
    }

    private void OnLayoutButtonClicked()
    {
        _dataPersistenceManager.ResetGameData();

        _uiManager.CurrentLayoutEnumSelected = selectedGameLayout;
        _gameSceneManager.LoadNextScene();
    }
}

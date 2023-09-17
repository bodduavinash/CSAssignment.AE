using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button saveButton;

    private GameSceneManager _gameSceneManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.Instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        saveButton.onClick.AddListener(onSaveButtonClicked);
    }

    private void OnHomeButtonClicked()
    {
        _gameSceneManager.LoadScene(GameScenesEnum.LayoutMenuScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void onSaveButtonClicked()
    {
        _dataPersistenceManager.SaveGameData();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : SingletonWithMonobehaviour<DataPersistenceManager>
{
    private FileDataHandler _dataHandler;

    private GameData _gameData;

    private List<IGameDataPersistence> _dataPersistenceObjectsList;

    private void Awake()
    {
        if (FindObjectOfType<GameSceneManager>().gameObject.scene.name.CompareTo(DontDestroyOnLoadObjects.DONT_DESTROY_ON_LOAD) == 1)
        {
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        _dataHandler = new FileDataHandler(Application.persistentDataPath, "CyberSpeedGameData.json");
        _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void NewGameData()
    {
        _gameData = new GameData();
    }

    public void LoadGameData()
    {
        _gameData = _dataHandler.Load();

        if (_gameData == null)
        {
            Debug.Log("No save data was found. Initializing new game data!!!");
            NewGameData();
        }

        if (_dataPersistenceObjectsList == null || _dataPersistenceObjectsList.Count == 0)
        {
            _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
        }

        foreach (IGameDataPersistence dataPersistenceObj in _dataPersistenceObjectsList)
        {
            dataPersistenceObj.LoadGameData(_gameData);
        }
    }

    public void SaveGameData()
    {
        foreach (IGameDataPersistence dataPersistenceObj in _dataPersistenceObjectsList)
        {
            dataPersistenceObj.SaveGameData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    public void ResetGameData()
    {
        NewGameData();
        _dataHandler.Save(_gameData);
    }

    public bool IsGameDataLoaded()
    {
        return _gameData != null &&
            _gameData.currentLayout != CardLayoutsEnum.None &&
            _gameData.layoutSpritesList.Count > 0 &&
            _gameData.matchedCardList.Count > 0;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
        LoadGameData();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveGameData();
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private List<IGameDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IGameDataPersistence> dataPersistenceObjs = FindObjectsOfType<MonoBehaviour>().OfType<IGameDataPersistence>();

        return new List<IGameDataPersistence>(dataPersistenceObjs);
    }
}
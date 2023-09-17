using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadObjects : SingletonWithMonobehaviour<DontDestroyOnLoadObjects>
{
    [SerializeField]
    private List<DontDestroyOnLoadEnums> ListOfGameObjectsToFind = new List<DontDestroyOnLoadEnums>();

    private Dictionary<DontDestroyOnLoadEnums, Object> DictOfGameObjects = new Dictionary<DontDestroyOnLoadEnums, Object>();

    public readonly static string DONT_DESTROY_ON_LOAD = "DontDestroyOnLoad";

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        PopulateUndestroyedList();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateUndestroyedList();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PopulateUndestroyedList();
    }

    private void PopulateUndestroyedList()
    {
        for (int i = 0; i < ListOfGameObjectsToFind.Count; i++)
        {
            if (ListOfGameObjectsToFind[i] != DontDestroyOnLoadEnums.None &&
                !DictOfGameObjects.ContainsKey(ListOfGameObjectsToFind[i]))
            {
                GameObject dontDestroyOnLoadGameObject = GameObject.Find(ListOfGameObjectsToFind[i].ToString());

                Object classObject = dontDestroyOnLoadGameObject.GetComponent(ListOfGameObjectsToFind[i].ToString());

                DictOfGameObjects.Add(ListOfGameObjectsToFind[i], classObject);
            }
        }
    }

    public Object GetObjectFromDict(DontDestroyOnLoadEnums name)
    {
        if (DictOfGameObjects.Count == 0)
        {
            //Special case scenario when the new scene loaded, need to populate once again.
            PopulateUndestroyedList();
        }

        return DictOfGameObjects[name];
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonWithMonobehaviour<GameSceneManager>
{
    private void Awake()
    {
        if (FindObjectOfType<GameSceneManager>().gameObject.scene.name.CompareTo(DontDestroyOnLoadObjects.DONT_DESTROY_ON_LOAD) == 1)
        {
            DontDestroyOnLoad(this);
        }
    }

    public void LoadNextScene()
    {
        if ((SceneManager.GetActiveScene().buildIndex + 1) <= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
    }

    public void LoadScene(GameScenesEnum gameScene, LoadSceneMode sceneMode)
    {
        SceneManager.LoadSceneAsync((int)gameScene, sceneMode);
    }
}

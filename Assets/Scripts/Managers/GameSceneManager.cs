using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonWithMonobehaviour<GameSceneManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        int curScene = GetCurrentSceneIndex();

        SceneManager.LoadScene(curScene);
    }    

    public void LoadNextScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(curScene + 1);
    }

    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

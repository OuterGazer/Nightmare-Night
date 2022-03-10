using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(curScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

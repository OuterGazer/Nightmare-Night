using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject[] level1;
    [SerializeField] GameObject level2;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("EntryLevel2") && !this.level2.activeInHierarchy)
            {
                foreach (GameObject item in this.level1)
                    GameObject.Destroy(item);

                this.level2.SetActive(true);
            }
            else if(this.gameObject.CompareTag("EntryLevel2") && this.level2.activeInHierarchy)
            {
                return;
            }
            else
                LoadNextScene();
        }            
    }
}

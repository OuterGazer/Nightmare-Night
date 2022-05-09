using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject[] level1;
    [SerializeField] GameObject level2;

    [SerializeField] LightingSettings lightingSettings;

    private void Awake()
    {
        if(GetCurrentSceneIndex() == 0)
        {
            CheckpointTrigger checkpointTrigger = GameObject.FindObjectOfType<CheckpointTrigger>();
            if (checkpointTrigger.IsCheckpointActivated)
                TransitionfromLevel1ToLevel2();
        }        
    }

    public void ReloadScene()
    {
        int curScene = GetCurrentSceneIndex();

        SceneManager.LoadScene(curScene);
    }    

    public void LoadNextScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;

        if(curScene == 0)
        {
            CheckpointTrigger checkpointTrigger = GameObject.FindObjectOfType<CheckpointTrigger>();
            if (checkpointTrigger != null)
                GameObject.Destroy(checkpointTrigger);
        }

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
                TransitionfromLevel1ToLevel2();
            }
            else if(this.gameObject.CompareTag("EntryLevel2") && this.level2.activeInHierarchy)
            {
                return;
            }
            else
                LoadNextScene();
        }            
    }

    private void TransitionfromLevel1ToLevel2()
    {
        foreach (GameObject item in this.level1)
            GameObject.Destroy(item);

        this.level2.SetActive(true);

        Lightmapping.lightingSettings = this.lightingSettings;

        RenderSettings.fog = false;
        RenderSettings.skybox = null;
        RenderSettings.sun = null;
        RenderSettings.reflectionIntensity = 1.0f;
        RenderSettings.reflectionBounces = 5;
        RenderSettings.ambientLight = new Color32(29, 28, 25, 255);
    }
}

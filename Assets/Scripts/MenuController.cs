using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject optionsMenu;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] GameObject startCollider;

    [SerializeField] Transform startPoint;

    [SerializeField] GameObject beginningStoryPrompt;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider mouseSlider;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex  == 1) { return; }
        this.playerInput.enabled = false;
        this.playerHealth.enabled = false;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) { return; }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickStart()
    {
        GameObject.Destroy(this.startCollider);

        this.playerHealth.GetComponent<CharacterController>().enabled = false;

        GameObject player = this.playerHealth.gameObject;

        player.transform.position = startPoint.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 15.0f, 0.0f));

        this.playerHealth.GetComponent<CharacterController>().enabled = true;

        this.beginningStoryPrompt.SetActive(true);

        this.gameObject.SetActive(false);
    }

    public void OnClickCredits()
    {
        this.creditsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnClickOptions(GameObject menuToActivate)
    {
        menuToActivate.SetActive(true);

        this.musicSlider.value = PlayerPrefs.GetFloat("Volume", 0.50f);
        this.SFXSlider.value = PlayerPrefs.GetFloat("SFX", 0.25f);
        this.mouseSlider.value = PlayerPrefs.GetFloat("Sensitivity", 0.01f);

        this.gameObject.SetActive(false);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

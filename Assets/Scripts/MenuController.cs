using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject optionsMenu;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] GameObject startCollider;

    [SerializeField] Transform startPoint;

    [SerializeField] GameObject beginningStoryPrompt;

    private void Awake()
    {
        this.playerInput.enabled = false;
        this.playerHealth.enabled = false;
    }

    private void Start()
    {
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

    public void OnClickOptions()
    {
        this.optionsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

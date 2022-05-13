using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptController : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject weapons;
    [SerializeField] PlayerInput playerInput;

    [SerializeField] AudioClip closePaper;

    public void OnClickOk(GameObject prompt)
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.closePaper);

        prompt.SetActive(false);

        this.UI.SetActive(true);
        this.weapons.gameObject.SetActive(true);
        this.playerInput.enabled = true;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickOkOnFirstPrompt(GameObject prompt)
    {
        prompt.SetActive(false);

        this.playerInput.enabled = true;
        this.playerInput.GetComponent<PlayerHealth>().enabled = true;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

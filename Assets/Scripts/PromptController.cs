using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptController : MonoBehaviour
{
    [SerializeField] GameObject weapons;
    [SerializeField] PlayerInput playerInput;

    public void OnClickOk(GameObject prompt)
    {
        prompt.SetActive(false);

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

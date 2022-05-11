using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompt : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject weapons;
    [SerializeField] PlayerInput playerInput;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.UI.SetActive(false);
        this.weapons.SetActive(false);
        this.playerInput.enabled = false;

        Time.timeScale = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompt : MonoBehaviour
{
    [SerializeField] GameObject weapons;
    [SerializeField] PlayerInput playerInput;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.weapons.SetActive(false);
        this.playerInput.enabled = false;

        Time.timeScale = 0;
    }
}

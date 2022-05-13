using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prompt : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject weapons;
    [SerializeField] PlayerInput playerInput;

    [SerializeField] AudioClip openPaper;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.gameObject.GetComponent<AudioSource>().Play();

        this.UI.SetActive(false);
        this.weapons.SetActive(false);
        this.playerInput.enabled = false;

        Time.timeScale = 0;
    }
}

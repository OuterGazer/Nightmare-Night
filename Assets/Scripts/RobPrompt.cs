using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RobPrompt : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] WeaponSwitcher weapons;

    [SerializeField] GameObject historyPromptRob;

    [SerializeField] GameObject pickUpSparks;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.historyPromptRob.SetActive(true);

            this.UI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.pickUpSparks.SetActive(false);

            // Prevent player from moving and shooting
            GameObject.FindObjectOfType<PlayerInput>().enabled = false;
            this.weapons.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }

    public void OnPromptRead()
    {
        Application.Quit();
    }
}

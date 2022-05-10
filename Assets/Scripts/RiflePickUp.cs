using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RiflePickUp : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] WeaponSwitcher weapons;

    [SerializeField] GameObject historyPromptMel;

    [SerializeField] GameObject pickUpSparks;


    private bool hasThisInstanceBeenRead = false;


    private void OnTriggerEnter(Collider other)
    {
        if (this.hasThisInstanceBeenRead) { return; }

        if (other.CompareTag("Player"))
        {
            this.historyPromptMel.SetActive(true);

            this.UI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.hasThisInstanceBeenRead = true;
            this.pickUpSparks.SetActive(false);

            // Prevent player from moving and shooting
            GameObject.FindObjectOfType<PlayerInput>().enabled = false;
            this.weapons.gameObject.SetActive(false);
            Time.timeScale = 0;
        }        
    }

    public void OnPromptRead()
    {
        this.historyPromptMel.SetActive(false);

        this.UI.SetActive(true);
        this.weapons.gameObject.SetActive(true);
        GameObject.FindObjectOfType<PlayerInput>().enabled = true;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.weapons.HasRifle = true;
        this.weapons.CurrentWeapon = 1;

        GameObject.FindObjectOfType<Trapdoor>().OnTalkingWithMel();
    }
}
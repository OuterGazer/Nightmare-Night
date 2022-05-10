using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxePickUp : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] WeaponSwitcher weapons;

    [SerializeField] GameObject historyPromptMel;
    [SerializeField] GameObject historyPromptRob;

    [SerializeField] GameObject pickUpSparks;


    private static bool isUIActivated = false;
    private bool hasThisInstanceBeenRead = false;


    private void OnTriggerEnter(Collider other)
    {
        if (this.hasThisInstanceBeenRead) { return; }

        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("MelAxe"))
                this.historyPromptMel.SetActive(true);
            else if (this.gameObject.CompareTag("RobAxe"))
                this.historyPromptRob.SetActive(true);

            this.UI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.hasThisInstanceBeenRead = true;
            this.pickUpSparks.SetActive(false);
        }

        //TODO: have a different prompt pop up once you have the axe.

        // Prevent player from moving and shooting
        GameObject.FindObjectOfType<PlayerInput>().enabled = false;
        this.weapons.gameObject.SetActive(false);
        Time.timeScale = 0;        
    }

    public void OnPromptRead()
    {
        if (this.historyPromptMel.activeInHierarchy)
            this.historyPromptMel.SetActive(false);
        else if (this.historyPromptRob.activeInHierarchy)
            this.historyPromptRob.SetActive(false);
        
        this.UI.SetActive(true);
        this.weapons.gameObject.SetActive(true);
        GameObject.FindObjectOfType<PlayerInput>().enabled = true;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (isUIActivated) { return; }
        
        this.weapons.HasAxe = true;
        this.weapons.CurrentWeapon = 0;

        this.gameObject.SetActive(false);
        
        isUIActivated = true;
    }
}

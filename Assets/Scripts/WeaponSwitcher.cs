using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Button Mapping")]
    [SerializeField] InputAction weaponSelect;
    [SerializeField] InputAction scrollWheel;

    [Header("Misc.")]
    [SerializeField] int currentWeapon = 0;

    [Header("UI Elements")]
    [SerializeField] GameObject axeCrosshair;
    [SerializeField] GameObject rifleNormalCrosshair;
    [SerializeField] GameObject rifleZoomedCrosshair;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI ammoText;

    private void Awake()
    {
        this.weaponSelect.Enable();
        this.scrollWheel.Enable();
    }

    private void OnDestroy()
    {
        this.weaponSelect.Disable();
        this.scrollWheel.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = this.currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != this.currentWeapon)
            SetWeaponActive();
    }    

    private void ProcessKeyInput()
    {
        if (this.weaponSelect.triggered)
        {
            switch (this.weaponSelect.activeControl.displayName)
            {
                case string i when i.Contains("1"):
                    this.currentWeapon = 0;
                    break;

                case string i when i.Contains("2"):
                    this.currentWeapon = 1;
                    break;
            }
        }
    }

    private void ProcessScrollWheel()
    {
        if (this.scrollWheel.triggered)
        {
            // Each case include a statement to check if we have reached the first or last weapon and then loop back appropriately
            switch (this.scrollWheel.ReadValue<float>())
            {
                case float i when i < 0:
                    this.currentWeapon--;
                    if (this.currentWeapon < 0)
                        this.currentWeapon = 1;
                    break;

                case float i when i > 0:
                    this.currentWeapon++;
                    if (currentWeapon >= this.gameObject.transform.childCount)
                        this.currentWeapon = 0;
                    break;
            }
        }
    }

    private void SetWeaponActive()
    { 
        // Before changing weapons, check if we are zoomed in to return to normal view upon weapon change.
        WeaponZoom weaponZoom = GameObject.FindObjectOfType<WeaponZoom>();
        if (weaponZoom != null && weaponZoom.IsZoomedIn)
            weaponZoom.ReturnToNormalView();

        int weaponIndex = 0;

        foreach (Transform weapon in this.gameObject.transform)
        {
            if(weaponIndex == this.currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            weaponIndex++;            
        }

        UpdateUI(this.currentWeapon);
    }

    private void UpdateUI(int weaponIndex)
    {
        UpdateCrosshair(weaponIndex);
        UpdateWeaponNameAndAmmo(weaponIndex);
    }

    private void UpdateCrosshair(int weaponIndex)
    {
        this.axeCrosshair.SetActive(false);
        this.rifleNormalCrosshair.SetActive(false);
        this.rifleZoomedCrosshair.SetActive(false);

        switch (weaponIndex)
        {
            case 0:
                this.axeCrosshair.SetActive(true);
                break;

            case 1:
                this.rifleNormalCrosshair.SetActive(true);
                break;
        }
    }

    private void UpdateWeaponNameAndAmmo(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                this.weaponText.text = "Throwing Axe";
                this.ammoText.text = "";
                break;

            case 1:
                this.weaponText.text = "Hunting Rifle";
                this.ammoText.text = GameObject.FindObjectOfType<Ammo>().GetCurrentAmmoAmount(AmmoType.Bullets).ToString();
                break;
        }
    }
}

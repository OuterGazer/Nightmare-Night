using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Button Mapping")]
    [SerializeField] InputAction weaponSelect;
    [SerializeField] InputAction scrollWheel;

    [Header("Misc.")]
    [SerializeField] int currentWeapon = 0;

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
                    if (currentWeapon < 0)
                        this.currentWeapon = 1;
                    break;

                case float i when i > 0:
                    this.currentWeapon++;
                    if (currentWeapon >= (this.gameObject.transform.childCount - 1))
                        this.currentWeapon = 0;
                    break;
            }
        }
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach(Transform weapon in this.gameObject.transform)
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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Button Mapping")]
    [SerializeField] InputAction weaponSelect;
    [SerializeField] InputAction scrollWheel;

    private int currentWeapon;
    public int CurrentWeapon
    {
        get { return this.currentWeapon; }
        set { if(value == 0 || value == 1) { this.currentWeapon = value; } }
    }

    [Header("UI Elements")]
    [SerializeField] GameObject axeCrosshair;
    [SerializeField] GameObject rifleNormalCrosshair;
    [SerializeField] GameObject rifleZoomedCrosshair;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("SFX")]
    [SerializeField] AudioClip axeDraw;
    [SerializeField] AudioClip rifleDraw;


    private AudioSource audioSource;


    private bool hasAxe = false;
    public bool HasAxe
    {
        get { return this.hasAxe; }
        set { this.hasAxe = value; }
    }
    private bool hasRifle = false;
    public bool HasRifle
    {
        get { return this.hasRifle; }
        set { this.hasRifle = value; }
    }

    private void Awake()
    {
        this.weaponSelect.Enable();
        this.scrollWheel.Enable();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            this.hasAxe = true;
            this.hasRifle = true;
            this.currentWeapon = 1;
        }

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
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
                    if (!this.hasAxe) { return; }
                    this.currentWeapon = 0;
                    break;

                case string i when i.Contains("2"):
                    if (!this.hasRifle) { return; }
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
                    {
                        this.currentWeapon = 1;
                    }
                    break;

                case float i when i > 0:
                    this.currentWeapon++;
                    if (currentWeapon >= this.gameObject.transform.childCount)
                    {
                        this.currentWeapon = 0;
                    }
                    break;
            }

            CheckIfPlayerHasWeapon();
        }
    }

    private void CheckIfPlayerHasWeapon()
    {
        if ((this.currentWeapon == 1) && (!this.hasRifle))
            this.currentWeapon = 0;
            
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
                this.audioSource.PlayOneShot(this.axeDraw);
                break;

            case 1:
                this.weaponText.text = "Hunting Rifle";
                string curAmmo = GameObject.FindObjectOfType<Ammo>().GetCurrentAmmoAmount(AmmoType.Bullets).ToString();
                this.ammoText.text = curAmmo + "/5";
                this.audioSource.PlayOneShot(this.rifleDraw);
                break;
        }
    }
}

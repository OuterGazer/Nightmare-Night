using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHitPoints = default;
    private float curHitPoints;

    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject weapons;

    [SerializeField] TextMeshProUGUI healthText;

    [Header("SFX")]
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip deathSFX;


    private AudioSource audioSource;


    private bool isAlive = true;
    public bool IsAlive => this.isAlive;


    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponentInChildren<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.curHitPoints = this.maxHitPoints;

        UpdateHealthUI(this.curHitPoints);

        CheckpointTrigger checkpointTrigger = GameObject.FindObjectOfType<CheckpointTrigger>();
        if (checkpointTrigger.IsCheckpointActivated)
            this.gameObject.transform.position = checkpointTrigger.transform.position;
    }

    public bool AddHealth(float inHealthPoints)
    {
        if (this.curHitPoints < 100)
        {
            //this.curHitPoints += inHealthPoints;

            this.curHitPoints = 100;

            UpdateHealthUI(this.curHitPoints);

            return true;
        }

        return false;
    }

    private void UpdateHealthUI(float curHitPoints)
    {
        this.healthText.text = curHitPoints.ToString() + "/100";
    }

    public void DealDamage(float inDamage)
    {
        this.curHitPoints -= inDamage;

        ApplyHitVisualEffects();

        if (this.curHitPoints <= 0)
        {            
            this.curHitPoints = 0;

            if(this.isAlive)
                ProcessDeath();
        }

        if(this.isAlive)
            this.audioSource.PlayOneShot(this.hurtSFX);

        UpdateHealthUI(this.curHitPoints);
    }

    private void ProcessDeath()
    {
        this.isAlive = false;

        // Prevent player from moving and shooting
        this.gameObject.GetComponent<PlayerInput>().enabled = false;
        this.weapons.SetActive(false);

        this.audioSource.PlayOneShot(this.deathSFX);

        // Make enemies disengage from attacking the player and have them return to their starting positions
        Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, 5.0f, LayerMask.GetMask("Enemy"));
        foreach(Collider item in enemies)
        {
            item.GetComponent<EnemyMover>().SetIsPlayerAlive(false);
        }
        
        // Activate game over menu
        this.gameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ApplyHitVisualEffects()
    {
        float effectIntensity = 1 - (this.curHitPoints / this.maxHitPoints);

        GameObject.FindObjectOfType<PlayerHitEffect>().OnPlayerGettingHit(effectIntensity);
    }
}

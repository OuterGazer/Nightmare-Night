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

    private bool isAlive = true;
    public bool IsAlive => this.isAlive;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.curHitPoints = this.maxHitPoints;

        UpdateHealthUI(this.curHitPoints);
    }

    public void AddHealth(float inHealthPoints)
    {
        this.curHitPoints += inHealthPoints;

        if (this.curHitPoints >= 100)
        {
            this.curHitPoints = 100;
        }

        UpdateHealthUI(this.curHitPoints);
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

        UpdateHealthUI(this.curHitPoints);
    }

    private void ProcessDeath()
    {
        this.isAlive = false;

        // Prevent player from moving and shooting
        this.gameObject.GetComponent<PlayerInput>().enabled = false;
        this.weapons.SetActive(false);

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

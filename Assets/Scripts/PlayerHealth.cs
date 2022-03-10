using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHitPoints = default;
    private float curHitPoints;

    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject weapons;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.curHitPoints = this.maxHitPoints;
    }

    public void DealDamage(float inDamage)
    {
        this.curHitPoints -= inDamage;

        if(this.curHitPoints <= 0)
        {
            this.curHitPoints = 0;
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        this.gameObject.GetComponent<PlayerInput>().enabled = false;
        this.weapons.SetActive(false);
        GameObject.FindObjectOfType<EnemyMover>().enabled = false;

        this.gameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

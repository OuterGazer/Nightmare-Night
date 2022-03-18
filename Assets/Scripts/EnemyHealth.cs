using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints;
    [SerializeField] float enemyDeathDelay;

    private int currentHitPoints;

    private void Start()
    {
        this.currentHitPoints = this.maxHitPoints;
    }

    public void AddHealth(int inHealth)
    {
        this.currentHitPoints += inHealth;
    }

    public void SubtractHealth(int inHealth)
    {
        this.currentHitPoints -= inHealth;

        if(this.currentHitPoints < 1)
        {
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        // TODO: Add SFX and possible VFX

        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        this.gameObject.GetComponent<EnemyMover>().enabled = false;

        this.StartCoroutine(KillEnemy());
    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(this.enemyDeathDelay);
        GameObject.Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Axe"))
            ProcessDeath();
    }
}

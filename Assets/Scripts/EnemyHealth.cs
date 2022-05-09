using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMover))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints;
    [SerializeField] float colliderDeactivationDelay = default;
    [SerializeField] float enemyDeathDelay = default;

    private int currentHitPoints;

    private bool isDead = false;

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
            this.isDead = true;

            if (this.isDead)
            {
                ProcessDeath();
                return;
            }
                
        }

        this.gameObject.GetComponent<EnemyMover>().ActivateHitAnimation();
    }

    private void ProcessDeath()
    {
        // TODO: Add SFX and possible VFX

        EnemyMover enemyMover = DeactivateEnemy();
        
        ZombieTrigger zombieTrigger = this.gameObject.GetComponent<ZombieTrigger>();
        if (zombieTrigger != null)
            zombieTrigger.OnEnemyDeath();

        this.StartCoroutine(KillEnemy(enemyMover));
    }

    private EnemyMover DeactivateEnemy()
    { 
        EnemyMover enemyMover = this.gameObject.GetComponent<EnemyMover>();
        enemyMover.enabled = false;

        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;

        return enemyMover;
    }

    private IEnumerator KillEnemy(EnemyMover enemyMover)
    {
        enemyMover.ActivateDyingAnimation();

        yield return new WaitForSeconds(this.colliderDeactivationDelay);

        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(this.enemyDeathDelay);
        GameObject.Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Axe"))
            SubtractHealth(this.currentHitPoints);
    }
}

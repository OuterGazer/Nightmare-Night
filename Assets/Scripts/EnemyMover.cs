using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 7.0f;
    [SerializeField] float rotationSpeed = default;
    [SerializeField] float enemyMaxDisengageTime = default;
    private float enemyDisengageTime = 0f;


    private Animator enemyAnim;
    private NavMeshAgent navMeshAgent;
    private float distanceToTargetSqr = Mathf.Infinity;
    private Vector3 startPos;
    private LayerMask defaultMask;

    private bool isPlayerVisible = false;
    private bool isPlayerAlive = true;
    public void SetIsPlayerAlive(bool isAlive)
    {
        this.isPlayerAlive = isAlive;
    }
    private bool shouldEnemyEngage = false;
    private bool isMoving = false;
    private bool isProvoked = false;
    public void SetIsProvoked(bool isProvoked)
    {
        this.isProvoked = isProvoked;
    }

    private void Awake()
    {
        this.enemyAnim = this.gameObject.GetComponentInChildren<Animator>();
        this.navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        this.defaultMask = LayerMask.GetMask("Default");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.startPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.distanceToTargetSqr = (this.target.transform.position - this.gameObject.transform.position).sqrMagnitude;

        if (this.isProvoked && this.isPlayerAlive)
        {
            EngageTarget();
            return;
        }
        else if(this.distanceToTargetSqr <= (this.chaseRange * this.chaseRange))
        {
            this.shouldEnemyEngage = true;

            if (this.isPlayerVisible && this.isPlayerAlive)
            {                
                EngageTarget();
            }
            else
            {
                this.enemyDisengageTime += Time.deltaTime;

                if(this.enemyDisengageTime >= this.enemyMaxDisengageTime)
                    ReturnToStartPos();
            }                
        }
        else if(!Mathf.Approximately(this.navMeshAgent.velocity.sqrMagnitude, 0))
        {
            if (!this.isMoving) { return; }

            ReturnToStartPos();
        }
        else
        {
            DisengageFromTarget();
        }
    }

    private void FixedUpdate()
    {
        if(this.shouldEnemyEngage)
            this.isPlayerVisible = CheckIfPlayerIsVisible();
    }

    private void DisengageFromTarget()
    {
        this.shouldEnemyEngage = false;

        this.isMoving = false;
        this.enemyAnim.SetTrigger("idle");
    }

    private void ReturnToStartPos()
    {
        this.shouldEnemyEngage = false;

        this.isMoving = true;

        this.enemyAnim.SetTrigger("move");
        this.enemyAnim.SetBool("isAttacking", false);

        this.navMeshAgent.SetDestination(this.startPos);

        if (this.enemyDisengageTime != 0f)
            this.enemyDisengageTime = 0f;
    }

    private void EngageTarget()
    {
        this.isMoving = true;

        if(this.distanceToTargetSqr > (this.navMeshAgent.stoppingDistance*this.navMeshAgent.stoppingDistance))
        {
            this.enemyAnim.SetTrigger("move");
            this.enemyAnim.SetBool("isAttacking", false);

            this.navMeshAgent.SetDestination(this.target.position);
        }
        else
        {
            FaceTarget();

            this.isMoving = false;

            this.enemyAnim.SetBool("isAttacking", true);
        }
    }

    public void ActivateDyingAnimation()
    {
        this.enemyAnim.SetTrigger("die");
    }

    public void ActivateHitAnimation()
    {
        this.enemyAnim.SetTrigger("hit");
    }

    public void SetSpeed(float inSpeed)
    {
        this.navMeshAgent.speed = inSpeed;
    }

    private void FaceTarget()
    {
        Vector3 targetDirection = (this.target.position - this.gameObject.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,
                                                              lookRotation,
                                                              this.rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfPlayerIsVisible()
    {
        RaycastHit hit;

        Vector3 dirToPlayer = (new Vector3(this.target.transform.position.x, this.target.transform.position.y + 1.0f, this.target.transform.position.z) - this.gameObject.transform.position).normalized;
        
        float distToPlayer = Vector3.Distance(this.gameObject.transform.position, this.target.transform.position);

        if (Physics.Raycast(this.gameObject.transform.position, dirToPlayer, out hit, distToPlayer, this.defaultMask))
        {
            //Debug.Log($"Vision blocked by {hit.collider.name}");
            return false;
        }            
        else
        {
            return true;
        }
            
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, this.chaseRange);
    }
}

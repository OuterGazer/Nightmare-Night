using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 7.0f;
    [SerializeField] float rotationSpeed = default;


    private Animator enemyAnim;
    private NavMeshAgent navMeshAgent;
    private float distanceToTargetSqr = Mathf.Infinity;
    private Vector3 startPos;

    private bool isMoving = false;
    private bool isProvoked = false;
    public void SetIsProvoked(bool isProvoked)
    {
        this.isProvoked = isProvoked;
    }

    private void Awake()
    {
        this.enemyAnim = this.gameObject.GetComponent<Animator>();
        this.navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
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

        if (this.isProvoked)
        {
            EngageTarget();
            return;
        }
        else if(this.distanceToTargetSqr <= (this.chaseRange * this.chaseRange))
        {
            EngageTarget();
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

    private void DisengageFromTarget()
    {
        this.isMoving = false;
        this.enemyAnim.SetTrigger("idle");
    }

    private void ReturnToStartPos()
    {
        this.isMoving = true;

        this.enemyAnim.SetTrigger("move");
        this.enemyAnim.SetBool("isAttacking", false);

        this.navMeshAgent.SetDestination(this.startPos);
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

    private void FaceTarget()
    {
        Vector3 targetDirection = (this.target.position - this.gameObject.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,
                                                              lookRotation,
                                                              this.rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, this.chaseRange);
    }
}

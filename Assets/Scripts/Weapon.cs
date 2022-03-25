using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Control Input Options")]
    [SerializeField] InputAction shoot;
    [SerializeField] Camera FPSCamera;
    [SerializeField] Transform player;

    [Header("Weapon Characteristics")]
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject bulletImpactSparks;
    private Ammo ammoSlot;
    [SerializeField] float shootSoundRadius = default;

    [Header("Axe Characteristics")]
    [SerializeField] Vector3 axeThrowingForce = default;
    [SerializeField] Vector3 axeSpinningTorque = default;
    [SerializeField] Transform axeParent;
    [SerializeField] Transform axeChild;
    [SerializeField] float retrieveDist = default;
    [SerializeField] float axeVelocityCap = default;
    [Tooltip("Max time upon which axe velocity cap is activated so it quickly falls to the ground. Used to limit its range. Should be a small number less than 0.2 seconds")]
    [SerializeField] float throwThreshold = default;
    [SerializeField] float axeGravityUponLaunch = default;
    [SerializeField] float maxAxeSpinningSpeed = default;


    private Rigidbody axeRB;
    private MeshCollider axeCol;
    private float retrieveTimer = 0.0f;
    private Vector3 curGravity;
    private LayerMask enemyMask;


    private bool playerHasAxe = true;
    private bool isWeaponLoaded = true;


    private void Awake()
    {
        this.shoot.Enable();

        if (this.gameObject.CompareTag("Axe"))
        {
            this.axeRB = this.gameObject.GetComponentInChildren<Rigidbody>();
            this.axeCol = this.gameObject.GetComponentInChildren<MeshCollider>();

            this.axeRB.maxAngularVelocity = this.maxAxeSpinningSpeed;
        }
        else
        {
            this.ammoSlot = this.gameObject.GetComponent<Ammo>();
        }

        this.curGravity = Physics.gravity;
        this.enemyMask = LayerMask.GetMask("Enemy");
    }

    private void OnDestroy()
    {
        this.shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.axeChild != null)
            ProcessAxeBehaviour();

        if (this.shoot.triggered)
            Shoot();
    }

    private void ProcessAxeBehaviour()
    {
        // Increase Axe weight to lessen its range
        if (this.retrieveTimer >= this.throwThreshold && this.retrieveTimer <= 0.75f && this.axeRB.isKinematic == false)
        {
            this.axeRB.velocity *= this.axeVelocityCap;
        }
        // To make the axe retrievable by the player once it lays on the ground after throwing
        else if (this.retrieveTimer >= 0.75f)
        {
            if (Mathf.Approximately(this.axeRB.velocity.sqrMagnitude, 0.0f))
            {
                Physics.gravity = this.curGravity;

                this.axeRB.isKinematic = true;
                this.axeCol.isTrigger = true;

                this.retrieveTimer = 0.0f;
            }
        }

        // Retrieve axe when player is close enough
        if (!this.playerHasAxe && Mathf.Approximately(this.axeRB.velocity.sqrMagnitude, 0.0f))
        {
            float distToPlayer = (this.player.transform.position - this.axeChild.transform.position).sqrMagnitude;

            if (distToPlayer <= this.retrieveDist * this.retrieveDist)
                RetrieveAxe();
        }

        // If player has axe, check every frame to reposition it correctly, else start the counter to be able to retrieve it (player has thrown it)
        if (this.playerHasAxe)
        {
            this.axeChild.transform.localPosition = Vector3.zero;

            if (this.axeChild.transform.localRotation != Quaternion.identity)
                this.axeChild.transform.localRotation = Quaternion.identity;
        }
        else
            this.retrieveTimer += Time.deltaTime;
    }

    private void Shoot()
    {
        if (!this.gameObject.CompareTag("Axe"))
        {
            if (this.isWeaponLoaded && this.ammoSlot.AmmoAmount > 0)
                this.StartCoroutine(ShootBullet());
        }
        else
        {
            if (!this.playerHasAxe) { return; } 

            this.axeChild.transform.SetParent(null);

            this.axeRB.isKinematic = false;
            this.axeCol.isTrigger = false;

            this.axeRB.velocity = Vector3.zero;
            this.axeRB.AddRelativeForce(this.axeThrowingForce, ForceMode.Impulse);
            this.axeRB.AddRelativeTorque(this.axeSpinningTorque, ForceMode.Impulse);

            this.playerHasAxe = false;
        }        
    }

    private IEnumerator ShootBullet()
    {
        this.isWeaponLoaded = false;

        EmmitRaycast();

        // Nearby enemies will get provoked if the rifle is shot near them and attack the player.
        EmmitOverlapSphere();

        this.muzzleFlash.Play();

        this.ammoSlot.SubtractAmmo();

        //TODO: have the yield return depend on the duration of a loading sniper rifle SFX
        yield return new WaitForSeconds(3.5f);

        this.isWeaponLoaded = true;
    }
    private void EmmitRaycast()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(this.FPSCamera.transform.position, this.FPSCamera.transform.forward, out hit, this.range);

        if (isHit)
        {
            PlayHitParticle(hit);            

            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.SubtractHealth(this.damage);
                hit.collider.GetComponent<EnemyMover>().SetIsProvoked(true);
            }
        }
    }

    private void EmmitOverlapSphere()
    {
        Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.shootSoundRadius, this.enemyMask);

        if(enemies.Length > 0)
        {
            foreach(Collider item in enemies)
            {
                item.gameObject.GetComponent<EnemyMover>().SetIsProvoked(true);
            }
        }
    }

    private void PlayHitParticle(RaycastHit hit)
    {
        GameObject.Instantiate<GameObject>(this.bulletImpactSparks, hit.point, Quaternion.LookRotation(hit.normal));
    }

    private void RetrieveAxe()
    {
        this.axeChild.transform.SetParent(this.axeParent);

        this.axeChild.transform.localPosition = Vector3.zero;
        this.axeChild.transform.localRotation = Quaternion.identity;

        this.playerHasAxe = true;
    }

    public void OnAxeCollision()
    {
        Physics.gravity = new Vector3(0, this.axeGravityUponLaunch);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, this.shootSoundRadius);
    }
}

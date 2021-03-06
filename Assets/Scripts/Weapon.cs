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
    [SerializeField] GameObject fleshImpactVFX;
    private Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
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
    [SerializeField] GameObject pickupSparks;

    [Header("Axe SFX")]
    [SerializeField] AudioClip throwingSFX;
    [SerializeField] AudioClip axeBluntHit;
    [SerializeField] AudioClip axeFleshHit;
    [SerializeField] AudioClip axeDraw;

    [Header("Rifle SFX")]
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip dryShootSFX;
    [SerializeField] AudioClip bulletOut;
    [SerializeField] AudioClip bulletIn;
    [SerializeField] AudioClip bluntHit;
    [SerializeField] AudioClip fleshHit;
    [SerializeField] AudioClip metalHit;


    private Rigidbody axeRB;
    private MeshCollider axeCol;
    private float retrieveTimer = 0.0f;
    private Vector3 curGravity;
    private Vector3 axeLastPos = Vector3.positiveInfinity;
    private LayerMask enemyMask;
    private AudioSource rifleAudioSource;
    private AudioSource axeAudioSource;


    private bool playerHasAxe = true;
    public bool PlayerHasAxe => this.playerHasAxe;
    private bool isWeaponLoaded = true;
    private bool canRetrieveAxe = false;
    private bool hasBluntSFXPlayed = false;
    private bool hasFleshSFXPlayed = false;

    private void Awake()
    {
        this.shoot.Enable();

        if (this.gameObject.CompareTag("Axe"))
        {
            this.axeRB = this.gameObject.GetComponentInChildren<Rigidbody>();
            this.axeCol = this.gameObject.GetComponentInChildren<MeshCollider>();

            this.axeRB.maxAngularVelocity = this.maxAxeSpinningSpeed;

            this.pickupSparks.SetActive(false);
        }
        else
        {
            this.ammoSlot = this.gameObject.GetComponentInParent<Ammo>();
        }

        this.curGravity = Physics.gravity;
        this.enemyMask = LayerMask.GetMask("Enemy");
        this.rifleAudioSource = this.gameObject.GetComponent<AudioSource>();
        this.axeAudioSource = this.gameObject.GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        if (!this.isWeaponLoaded)
            this.StartCoroutine(LoadWeapon());            
    }

    private IEnumerator LoadWeapon()
    {
        if(this.rifleAudioSource.isPlaying)
            yield return new WaitForSeconds(0.3f);

        this.rifleAudioSource.PlayOneShot(this.bulletOut);

        yield return new WaitForSeconds(1.6f);

        this.rifleAudioSource.PlayOneShot(this.bulletIn);

        yield return new WaitForSeconds(0.6f);

        this.isWeaponLoaded = true;
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
            if (CheckIfAxeIsStill(this.axeChild.transform.localPosition)) // Mathf.Approximately(this.axeRB.velocity.sqrMagnitude, 0.0f
            {
                Physics.gravity = this.curGravity;

                this.axeRB.isKinematic = true;
                this.axeCol.isTrigger = true;

                this.canRetrieveAxe = true;
                this.pickupSparks.SetActive(true);

                this.retrieveTimer = 0.0f;
            }
        }

        // Retrieve axe when player is close enough
        if (this.canRetrieveAxe) 
        {
            float distToPlayer = (this.player.transform.position - this.axeChild.transform.position).sqrMagnitude;

            if (distToPlayer <= this.retrieveDist * this.retrieveDist)
                RetrieveAxe(false);
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

    private bool CheckIfAxeIsStill(Vector3 inPosition)
    {
        if (Mathf.Approximately(inPosition.sqrMagnitude, this.axeLastPos.sqrMagnitude))
        {
            return true;
        }            

        this.axeLastPos = inPosition;

        return false;
    }

    private void Shoot()
    {
        if (!this.gameObject.CompareTag("Axe"))
        {
            if (this.isWeaponLoaded && this.ammoSlot.GetCurrentAmmoAmount(this.ammoType) > 0)
            {
                ShootBullet();
                this.rifleAudioSource.PlayOneShot(this.shootSFX);
            }
            else
                this.rifleAudioSource.PlayOneShot(this.dryShootSFX);

        }
        else
        {
            if (!this.playerHasAxe) { return; }

            this.axeAudioSource.PlayOneShot(this.throwingSFX);

            this.axeChild.transform.SetParent(null);

            this.axeRB.isKinematic = false;
            this.axeCol.isTrigger = false;

            this.axeRB.velocity = Vector3.zero;
            this.axeRB.AddRelativeForce(this.axeThrowingForce, ForceMode.Impulse);
            this.axeRB.AddRelativeTorque(this.axeSpinningTorque, ForceMode.Impulse);

            this.playerHasAxe = false;
        }        
    }

    private void ShootBullet()
    {
        this.isWeaponLoaded = false;

        EmmitRaycast();

        // Nearby enemies will get provoked if the rifle is shot near them and attack the player.
        EmmitOverlapSphere();

        this.muzzleFlash.Play();

        this.ammoSlot.SubtractAmmo(this.ammoType);

        this.StartCoroutine(LoadWeapon());
    }
    private void EmmitRaycast()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(this.FPSCamera.transform.position, this.FPSCamera.transform.forward, out hit, this.range);

        if (isHit)
        {
            if(!hit.collider.CompareTag("Enemy"))
                PlayHitParticle(hit, this.bulletImpactSparks);
            else
                PlayHitParticle(hit, this.fleshImpactVFX);

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    this.rifleAudioSource.PlayOneShot(this.fleshHit);
                    enemyHealth.SubtractHealth(this.damage);
                    hit.collider.GetComponent<EnemyMover>().SetIsProvoked(true);
                }
            }
            else if (hit.collider.CompareTag("Artifact"))
            {
                this.rifleAudioSource.PlayOneShot(this.metalHit);
                hit.collider.GetComponent<Artifact>().KillBoss(hit.point);
            }                
            else
                this.rifleAudioSource.PlayOneShot(this.bluntHit);
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

    private void PlayHitParticle(RaycastHit hit, GameObject VFX)
    {
        GameObject.Instantiate<GameObject>(VFX, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public void RetrieveAxe(bool isEmergency)
    {
        if (isEmergency)
        {
            this.axeRB.velocity = Vector3.zero;
            this.axeRB.isKinematic = true;
            this.axeCol.isTrigger = true;
            this.hasBluntSFXPlayed = false;
            this.hasFleshSFXPlayed = false;
        }        

        this.axeChild.transform.SetParent(this.axeParent);

        this.axeChild.transform.localPosition = Vector3.zero;
        this.axeChild.transform.localRotation = Quaternion.identity;

        this.pickupSparks.SetActive(false);

        this.playerHasAxe = true;
        this.canRetrieveAxe = false;
        this.axeLastPos = Vector3.positiveInfinity;

        this.axeAudioSource.PlayOneShot(this.axeDraw);
    }

    public void OnAxeCollision(string collisionTag, Collision collision)
    {
        Physics.gravity = new Vector3(0, this.axeGravityUponLaunch);

        if (this.axeAudioSource.isPlaying && (!this.hasBluntSFXPlayed && !this.hasFleshSFXPlayed))
            this.axeAudioSource.Stop();

        if(collisionTag != "Enemy" && collisionTag != "Player")
        {
            if (!this.hasBluntSFXPlayed)
            {
                this.axeAudioSource.PlayOneShot(this.axeBluntHit);
                this.StartCoroutine(ReenableSFX("blunt"));
            }                
        }
        else
        {
            if (!this.hasFleshSFXPlayed)
            {
                this.axeAudioSource.PlayOneShot(this.axeFleshHit);
                GameObject.Instantiate<GameObject>(this.fleshImpactVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                this.StartCoroutine(ReenableSFX("flesh"));
            }
                
        }            
    }

    private IEnumerator ReenableSFX(string SFX)
    {
        switch (SFX)
        {
            case "blunt":
                this.hasBluntSFXPlayed = true;
                break;

            case "flesh":
                this.hasFleshSFXPlayed = true;
                break;
        }

        yield return new WaitForSeconds(1.5f);

        switch (SFX)
        {
            case "blunt":
                this.hasBluntSFXPlayed = false;
                break;

            case "flesh":
                this.hasFleshSFXPlayed = false;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, this.shootSoundRadius);
    }
}

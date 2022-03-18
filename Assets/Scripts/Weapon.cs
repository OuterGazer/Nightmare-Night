using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Control Input Options")]
    [SerializeField] InputAction shoot;
    [SerializeField] Camera FPSCamera;

    [Header("Weapon Characteristics")]
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject bulletImpactSparks;
    [SerializeField] Vector3 axeThrowingForce = default;
    [SerializeField] Vector3 axeSpinningTorque = default;

    private Rigidbody axeRB;

    private void Awake()
    {
        this.shoot.Enable();

        if (this.gameObject.CompareTag("Axe"))
            this.axeRB = this.gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void OnDestroy()
    {
        this.shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.shoot.triggered)
            Shoot();
    }

    private void Shoot()
    {
        if (!this.gameObject.CompareTag("Axe"))
        {
            EmmitRaycast();

            this.muzzleFlash.Play();
        }
        else
        {
            this.gameObject.transform.SetParent(null);

            this.axeRB.isKinematic = false;
            this.axeRB.AddRelativeForce(this.axeThrowingForce, ForceMode.Impulse);
            this.axeRB.AddRelativeTorque(this.axeSpinningTorque, ForceMode.Impulse);
        }        
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

    private void PlayHitParticle(RaycastHit hit)
    {
        GameObject.Instantiate<GameObject>(this.bulletImpactSparks, hit.point, Quaternion.LookRotation(hit.normal));
    }
}

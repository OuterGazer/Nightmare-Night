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

    private void Awake()
    {
        this.shoot.Enable();
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
        EmmitRaycast();

        this.muzzleFlash.Play();
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
            }
        }
    }

    private void PlayHitParticle(RaycastHit hit)
    {
        // TODO: correct the spark angle so it always plays perpendicular to the surface hit
        GameObject.Instantiate<GameObject>(this.bulletImpactSparks, hit.point, Quaternion.Euler(hit.normal));
    }
}

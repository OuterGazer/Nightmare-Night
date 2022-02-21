using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] InputAction shoot;
    [SerializeField] Camera FPSCamera;

    [SerializeField] float range;

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
        if (this.shoot.IsPressed())
            Shoot();
    }

    private void Shoot()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(this.FPSCamera.transform.position, this.FPSCamera.transform.forward, out hit, this.range);

        if (isHit)
            Debug.Log($"Hit {hit.collider.name}");
    }
}

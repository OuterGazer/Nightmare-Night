using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoAmount = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ammo ammo = GameObject.FindObjectOfType<Ammo>();

            if (!ammo.AddAmmo(this.ammoType, this.ammoAmount)) { return; }            

            GameObject.Destroy(this.gameObject);
        }
    }
}

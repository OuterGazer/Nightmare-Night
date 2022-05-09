using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float amountToHeal = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().AddHealth(this.amountToHeal);

            // TODO: add SFX

            GameObject.Destroy(this.gameObject);
        }
    }
}

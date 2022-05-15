using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoAmount = default;

    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ammo ammo = GameObject.FindObjectOfType<Ammo>();

            if (!ammo.AddAmmo(this.ammoType, this.ammoAmount)) { return; }

            AudioSource.PlayClipAtPoint(this.audioSource.clip, Camera.main.transform.position, this.audioSource.volume);

            GameObject.Destroy(this.gameObject);
        }
    }
}

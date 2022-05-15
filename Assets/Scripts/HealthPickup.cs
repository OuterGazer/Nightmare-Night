using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float amountToHeal = default;

    [SerializeField] AudioClip openBottle;
    [SerializeField] AudioClip reliefSFX;

    private AudioSource audioSource;
    private Collider potionCol;
    private MeshRenderer potionMesh;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.potionCol = this.gameObject.GetComponent<MeshCollider>();
        this.potionMesh = this.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerHealth>().AddHealth(this.amountToHeal))
            {
                this.StartCoroutine(PlayDrinkSFX());

                this.potionCol.enabled = false;
                this.potionCol.enabled = false;
            }
        }
    }

    private IEnumerator PlayDrinkSFX()
    {
        this.audioSource.PlayOneShot(this.openBottle);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.PlayOneShot(this.reliefSFX);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        GameObject.Destroy(this.gameObject);
    }
}

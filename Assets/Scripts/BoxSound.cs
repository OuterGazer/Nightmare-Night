using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    [SerializeField] AudioClip boxHit;

    private AudioSource audioSource;

    private void OnEnable()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player") && collision.gameObject.layer != 9)
        {
            this.audioSource.PlayOneShot(this.boxHit);
        }
            
    }
}

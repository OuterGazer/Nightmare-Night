using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [SerializeField] Transform[] libraryLights;

    [SerializeField] AudioClip lightsOn;
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void TurnOffTheLights(Transform[] inLights)
    {
        for (int i = 0; i < inLights.Length; i++)
        {
            inLights[i].gameObject.SetActive(false);
        }

        ManageLibraryLights(true);
    }

    public void TurnOnTheLights(Transform[] inLights)
    {
        for (int i = 0; i < inLights.Length; i++)
        {
            inLights[i].gameObject.SetActive(true);
        }

        ManageLibraryLights(false);
    }

    private void ManageLibraryLights(bool shouldBeLit)
    {
        for (int i = 0; i < this.libraryLights.Length; i++)
        {
            libraryLights[i].gameObject.SetActive(shouldBeLit);
        }

        if (shouldBeLit)
        {
            if (this.audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.PlayOneShot(this.lightsOn);
        }
    }
}

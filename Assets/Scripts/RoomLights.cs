using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLights : MonoBehaviour
{
    [SerializeField] Transform[] lights;
    [SerializeField] Transform referencePoint;
    [SerializeField] AudioClip lightsOn;


    private LightingController lightingController;
    private AudioSource audioSource;


    [SerializeField] private bool areLightsOn;
    public bool AreLightsOn => this.areLightsOn;
    

    private void Awake()
    {
        this.lightingController = GameObject.FindObjectOfType<LightingController>();
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        float distToRefPoint = (other.ClosestPoint(this.referencePoint.position) - this.referencePoint.position).sqrMagnitude;

        if (other.CompareTag("Player"))
        {
            switch (this.areLightsOn)
            {
                case true:
                    if(distToRefPoint > 1.0f)
                        TurnLightsOff();
                    break;
                case false:
                    if (distToRefPoint < 1.0f)
                        TurnLightsOn();
                    break;
            }
        }
    }

    public void TurnLightsOff()
    {
        this.lightingController.SendMessage("TurnOffTheLights", this.lights);
        this.areLightsOn = false;
    }

    public void TurnLightsOn()
    {
        this.lightingController.SendMessage("TurnOnTheLights", this.lights);

        if (this.audioSource.isPlaying)
            this.audioSource.Stop();

        this.audioSource.PlayOneShot(this.lightsOn);

        this.areLightsOn = true;
    }

    public void ManageRoomLightsManually(bool shouldBeLit)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(shouldBeLit);
        }

        this.areLightsOn = shouldBeLit;

        if (shouldBeLit)
        {
            if (this.audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.PlayOneShot(this.lightsOn);
        }
    }
}

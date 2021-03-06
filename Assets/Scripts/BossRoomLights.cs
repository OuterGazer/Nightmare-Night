using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLights : MonoBehaviour
{
    [SerializeField] Transform[] bossRoomLights;
    [SerializeField] RoomLights artifactRoomLights;
    [SerializeField] Transform referencePoint;

    [SerializeField] GameObject roomEnemies;

    [SerializeField] bool areLightsOn;

    [SerializeField] AudioClip lightsOn;
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        if (GameObject.FindObjectOfType<CheckpointTrigger>().IsCheckpointActivated)
        {            
            ManageRoomLightsManually(true);
            this.roomEnemies.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        float distToRefPoint = (other.ClosestPoint(this.referencePoint.position) - this.referencePoint.position).sqrMagnitude;

        if (other.CompareTag("Player"))
        {
            switch (this.artifactRoomLights.AreLightsOn)
            {
                case true:
                    if (distToRefPoint > 1.0f)
                    {
                        ManageRoomLightsManually(true);
                        this.roomEnemies.SetActive(true);
                        this.artifactRoomLights.ManageRoomLightsManually(false);
                    }
                    break;
                case false:
                    if (distToRefPoint < 1.0f)
                    {
                        ManageRoomLightsManually(false);
                        this.roomEnemies.SetActive(false);
                        this.artifactRoomLights.ManageRoomLightsManually(true);
                    }
                    break;
            }
        }
    }

    public void ManageRoomLightsManually(bool shouldBeLit)
    {
        for (int i = 0; i < this.bossRoomLights.Length; i++)
        {
            this.bossRoomLights[i].gameObject.SetActive(shouldBeLit);
        }

        this.areLightsOn = shouldBeLit;

        if (shouldBeLit)
        {
            GameObject.FindObjectOfType<SoundController>().SwitchSongs();

            if (this.audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.PlayOneShot(this.lightsOn);
        }
            
        else
        {
            GameObject.FindObjectOfType<SoundController>().SwitchSongsBack();
        }
    }
}

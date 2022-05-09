using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLights : MonoBehaviour
{
    [SerializeField] Transform[] bossRoomLights;
    [SerializeField] RoomLights artifactRoomLights;
    [SerializeField] Transform referencePoint;

    [SerializeField] bool areLightsOn;

    private void Awake()
    {
        if (GameObject.FindObjectOfType<CheckpointTrigger>().IsCheckpointActivated)
        {
            ManageRoomLightsManually(true);
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
                        this.artifactRoomLights.ManageRoomLightsManually(false);
                    }
                    break;
                case false:
                    if (distToRefPoint < 1.0f)
                    {
                        ManageRoomLightsManually(false);
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
    }
}

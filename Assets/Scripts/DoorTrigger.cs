using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] RoomLights roomLights;
    [SerializeField] Transform referencePoint;

    private void OnTriggerExit(Collider other)
    {
        float distToRefPoint = (other.ClosestPoint(this.referencePoint.position) - this.referencePoint.position).sqrMagnitude;

        if (other.CompareTag("Player"))
        {
            switch (this.roomLights.AreLightsOn)
            {
                case true:
                    if (distToRefPoint > 1.0f)
                        this.roomLights.TurnLightsOff();
                    break;
                case false:
                    if (distToRefPoint < 1.0f)
                        this.roomLights.TurnLightsOn();
                    break;
            }
        }
    }
}

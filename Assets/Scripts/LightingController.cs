using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [SerializeField] Transform entranceRoom;

    public void TurnOffTheLights(Transform[] inLights)
    {
        for (int i = 0; i < inLights.Length; i++)
        {
            inLights[i].gameObject.SetActive(false);
        }
        
    }

    public void TurnOnTheLights(Transform[] inLights)
    {
        for (int i = 0; i < inLights.Length; i++)
        {
            inLights[i].gameObject.SetActive(true);
        }

    }
}

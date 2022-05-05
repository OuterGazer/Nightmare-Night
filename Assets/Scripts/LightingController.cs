using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [SerializeField] Transform[] libraryLights;

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
    }
}

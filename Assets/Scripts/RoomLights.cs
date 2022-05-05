using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLights : MonoBehaviour
{
    [SerializeField] Transform[] lights;

    [SerializeField] private bool areLightsOn;
    public bool AreLightsOn => this.areLightsOn;

    private LightingController lightingController;

    private void Awake()
    {
        this.lightingController = GameObject.FindObjectOfType<LightingController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (this.areLightsOn)
            {
                case true:
                    this.lightingController.SendMessage("TurnOffTheLights", this.lights);
                    this.areLightsOn = false;
                    break;
                case false:
                    this.lightingController.SendMessage("TurnOnTheLights", this.lights);
                    this.areLightsOn = true;
                    break;
            }            
        }
    }
}

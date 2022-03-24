using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] GameObject sniperRifle;
    [SerializeField] float normalFOV = default;
    [SerializeField] float zoomedFOV = default;
    [SerializeField] float rotSpeedReduction = default;

    private StarterAssetsInputs input;
    private FirstPersonController playerMovement;
    private float curRotSpeed;

    private bool isZoomedIn = false;

    private void Awake()
    {
        this.input = this.gameObject.GetComponent<StarterAssetsInputs>();
        this.playerMovement = this.gameObject.GetComponent<FirstPersonController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.virtualCam.m_Lens.FieldOfView = this.normalFOV;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.input.zoomIn)
        {
            // If axe is equipped, don't zoom in
            if(!this.sniperRifle.activeInHierarchy) { return; }

            if (!this.isZoomedIn)
            {
                this.virtualCam.m_Lens.FieldOfView = this.zoomedFOV;
                this.curRotSpeed = this.playerMovement.RotationSpeed;
                this.playerMovement.RotationSpeed *= this.rotSpeedReduction;
            }
            else
            {
                this.virtualCam.m_Lens.FieldOfView = this.normalFOV;
                this.playerMovement.RotationSpeed = this.curRotSpeed;
            }

            this.isZoomedIn = !this.isZoomedIn;

            this.input.zoomIn = false;
        }
    }
}

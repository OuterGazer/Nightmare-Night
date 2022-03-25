using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] float normalFOV = default;
    [SerializeField] float zoomedFOV = default;
    [SerializeField] float rotSpeedReduction = default;

    private StarterAssetsInputs input;
    private FirstPersonController playerMovement;
    private float curRotSpeed;

    private bool isZoomedIn = false;
    public bool IsZoomedIn => this.isZoomedIn;

    private void Awake()
    {
        this.input = GameObject.FindObjectOfType<StarterAssetsInputs>();
        this.playerMovement = GameObject.FindObjectOfType<FirstPersonController>();
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
            if (!this.isZoomedIn)
            {
                this.virtualCam.m_Lens.FieldOfView = this.zoomedFOV;
                this.curRotSpeed = this.playerMovement.RotationSpeed;
                this.playerMovement.RotationSpeed *= this.rotSpeedReduction;

                this.isZoomedIn = true;
            }
            else
            {
                ReturnToNormalView();
            }

            this.input.zoomIn = false;
        }
    }

    public void ReturnToNormalView()
    {
        this.virtualCam.m_Lens.FieldOfView = this.normalFOV;
        this.playerMovement.RotationSpeed = this.curRotSpeed;

        this.isZoomedIn = false;
    }
}

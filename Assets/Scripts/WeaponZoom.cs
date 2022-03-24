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

    private StarterAssetsInputs input;

    private bool isZoomedIn = false;

    private void Awake()
    {
        this.input = this.gameObject.GetComponent<StarterAssetsInputs>();
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
            }
            else
            {
                this.virtualCam.m_Lens.FieldOfView = this.normalFOV;
            }

            this.isZoomedIn = !this.isZoomedIn;

            this.input.zoomIn = false;
        }
    }
}

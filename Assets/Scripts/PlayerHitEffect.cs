using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHitEffect : MonoBehaviour
{
    [SerializeField] float effectDurationModifier = default;


    CinemachineImpulseSource screenShake;
    Vignette vignetteEffect;
    Grain grainEffect;


    private bool shouldEffectsBeAttenuated = false;


    private void Awake()
    {
        this.screenShake = this.gameObject.GetComponent<CinemachineImpulseSource>();
        this.vignetteEffect = this.gameObject.GetComponent<PostProcessVolume>().sharedProfile.GetSetting<Vignette>();
        this.grainEffect = this.gameObject.GetComponent<PostProcessVolume>().sharedProfile.GetSetting<Grain>();
    }

    private void Start()
    {
        this.vignetteEffect.intensity.value = 0.0f;
        this.grainEffect.intensity.value = 0.0f;
    }

    public void OnPlayerGettingHit(float effectIntensity)
    {
        this.screenShake.GenerateImpulse();

        this.vignetteEffect.intensity.value = effectIntensity;
        this.grainEffect.intensity.value = effectIntensity;

        this.shouldEffectsBeAttenuated = true;
    }

    private void Update()
    {
        if (this.shouldEffectsBeAttenuated)
        {
            this.vignetteEffect.intensity.value -= Time.deltaTime/this.effectDurationModifier;
            this.grainEffect.intensity.value -= Time.deltaTime/this.effectDurationModifier;

            if (this.vignetteEffect.intensity.value <= 0)
                this.vignetteEffect.intensity.value = 0;

            if (this.grainEffect.intensity.value <= 0)
                this.grainEffect.intensity.value = 0;

            if((this.vignetteEffect.intensity.value <= 0) && (this.grainEffect.intensity.value <= 0))
                this.shouldEffectsBeAttenuated = false;
        }
    }
}

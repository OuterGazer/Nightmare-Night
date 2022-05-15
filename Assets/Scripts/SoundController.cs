using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip song2;
    [SerializeField] float songSwitchDelayFactor;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] AudioClip testSFX;
    [SerializeField] Slider mouseSlider;

    private AudioClip song1;
    private AudioSource audioSource;
    private FirstPersonController player;

    private bool shouldSongsbeSwitched = false;
    private bool shouldSongsbeSwitchedBack = false;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.player = GameObject.FindObjectOfType<FirstPersonController>();

        this.song1 = this.audioSource.clip;

        this.audioSource.ignoreListenerPause = true;
        this.audioSource.ignoreListenerVolume = true;
    }

    private void Start()
    {
        this.audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.50f);
        AudioListener.volume = PlayerPrefs.GetFloat("SFX", 0.25f);
        this.player.RotationSpeed = PlayerPrefs.GetFloat("Sensitivity", 0.01f);
    }

    public void SwitchSongs()
    {
        this.shouldSongsbeSwitched = true;
    }

    public void SwitchSongsBack()
    {
        this.shouldSongsbeSwitchedBack = true;
    }

    public void SetMusicVolume()
    {
        this.audioSource.volume = this.musicSlider.value;
    }

    public void SetSFXVolume()
    {
        AudioListener.volume = this.SFXSlider.value;
        AudioSource.PlayClipAtPoint(this.testSFX, Camera.main.transform.position);
    }

    public void SetSensitivity()
    {
        this.player.RotationSpeed = this.mouseSlider.value;
    }

    private void Update()
    {
        if (this.shouldSongsbeSwitched)
        {
            if (this.audioSource.clip == this.song1)
            {
                this.audioSource.volume -= Time.deltaTime / this.songSwitchDelayFactor;
            }


            if (Mathf.Approximately(this.audioSource.volume, 0.0f))
            {
                this.audioSource.clip = this.song2;
                this.audioSource.Play();
            }


            if (this.audioSource.clip == this.song2)
            {
                this.audioSource.volume += Time.deltaTime / this.songSwitchDelayFactor;
            }


            if (this.audioSource.volume >= 0.5f && this.audioSource.clip == this.song2)
            {
                this.shouldSongsbeSwitched = false;
            }
        }
        else if (this.shouldSongsbeSwitchedBack)
        {
            if (this.audioSource.clip == this.song2)
            {
                this.audioSource.volume -= Time.deltaTime / this.songSwitchDelayFactor;
            }


            if (Mathf.Approximately(this.audioSource.volume, 0.0f))
            {
                this.audioSource.clip = this.song1;
                this.audioSource.Play();
            }


            if (this.audioSource.clip == this.song1)
            {
                this.audioSource.volume += Time.deltaTime / this.songSwitchDelayFactor;
            }


            if (this.audioSource.volume >= 0.5f && this.audioSource.clip == this.song1)
            {
                this.shouldSongsbeSwitchedBack = false;
            }
        }
        
            
    }
}

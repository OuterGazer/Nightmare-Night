using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip song2;
    [SerializeField] float songSwitchDelayFactor;

    private AudioClip song1;
    private AudioSource audioSource;

    private bool shouldSongsbeSwitched = false;
    private bool shouldSongsbeSwitchedBack = false;

    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        this.song1 = this.audioSource.clip;

        this.audioSource.ignoreListenerPause = true;
        this.audioSource.ignoreListenerVolume = true;
    }

    public void SwitchSongs()
    {
        this.shouldSongsbeSwitched = true;
    }

    public void SwitchSongsBack()
    {
        this.shouldSongsbeSwitchedBack = true;
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

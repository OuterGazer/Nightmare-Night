using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SoundController : MonoBehaviour
{
    [SerializeField] InputAction pauseGame;

    [SerializeField] AudioClip song2;
    [SerializeField] float songSwitchDelayFactor;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] AudioClip testSFX;
    [SerializeField] Slider mouseSlider;
    [SerializeField] Slider musicPauseSlider;
    [SerializeField] Slider SFXPauseSlider;
    [SerializeField] Slider mousePauseSlider;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject weapons;


    private AudioClip song1;
    private AudioSource audioSource;
    private FirstPersonController player;

    private bool shouldSongsbeSwitched = false;
    private bool shouldSongsbeSwitchedBack = false;
    public bool enableWeapon = false;

    private void Awake()
    {
        this.pauseGame.Enable();

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.player = GameObject.FindObjectOfType<FirstPersonController>();

        this.song1 = this.audioSource.clip;

        this.audioSource.ignoreListenerPause = true;
        this.audioSource.ignoreListenerVolume = true;
    }

    private void OnDestroy()
    {
        this.pauseGame.Disable();
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

    public void SetMusicVolumeFromPAuse()
    {
        this.audioSource.volume = this.musicPauseSlider.value;
    }

    public void SetSFXVolumeFomPause()
    {
        AudioListener.volume = this.SFXPauseSlider.value;
        AudioSource.PlayClipAtPoint(this.testSFX, Camera.main.transform.position);
    }

    public void SetSensitivityFromPause()
    {
        this.player.RotationSpeed = this.mousePauseSlider.value;
    }

    private void Update()
    {
        ChangeSong();

        if (this.pauseGame.triggered && !this.pauseMenu.activeSelf)
        {
            this.pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.playerInput.enabled = false;
            this.weapons.SetActive(false);

            Time.timeScale = 0;
        }
        else if(this.pauseGame.triggered && this.pauseMenu.activeSelf)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        this.pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.playerInput.enabled = true;

        if(enableWeapon)
            this.weapons.SetActive(true);

        Time.timeScale = 1;
    }

    private void ChangeSong()
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

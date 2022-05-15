using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnFunctionality : MonoBehaviour
{
    [SerializeField] GameObject menuToReturn;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider mouseSlider;

    public void OnClickReturn()
    {
        this.menuToReturn.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnOptionsClickReturn()
    {
        this.menuToReturn.SetActive(true);

        PlayerPrefs.SetFloat("Volume", this.musicSlider.value);
        PlayerPrefs.SetFloat("SFX", this.SFXSlider.value);
        PlayerPrefs.SetFloat("Sensitivity", this.mouseSlider.value);

        this.gameObject.SetActive(false);
    }
}

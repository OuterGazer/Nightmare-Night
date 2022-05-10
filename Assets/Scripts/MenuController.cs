using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerHealth playerHealth;

    private void Awake()
    {
        this.playerInput.enabled = false;
        this.playerHealth.enabled = false;
    }

    public void OnClickStart()
    {
        this.gameObject.transform.position = new Vector3(82.1289978f, 5.8912487f, 30.4769993f);

        this.playerInput.enabled = true;
        this.playerHealth.enabled = true;
    }

    public void OnClickCredits()
    {

    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

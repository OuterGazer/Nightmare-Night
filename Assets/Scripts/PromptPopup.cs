using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptPopup : MonoBehaviour
{
    [SerializeField] Prompt prompt;

    private void OnTriggerEnter(Collider other)
    {
        this.prompt.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarStoneExit : MonoBehaviour
{
    private bool isExitActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (this.isExitActivated) { return; }

        if (other.CompareTag("Player") )
        {
            GameObject.FindObjectOfType<LevelTransition>().EndLevelTrigger.Invoke();

            this.isExitActivated = true;
        }
    }
}

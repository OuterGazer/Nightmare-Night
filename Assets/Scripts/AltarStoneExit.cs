using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarStoneExit : MonoBehaviour
{
    public bool isZombie1Killed = false;
    public bool isZombie2Killed = false;
    public void OnZombieKilled()
    {
        if(this.isZombie1Killed && this.isZombie2Killed)
            GameObject.FindObjectOfType<LevelTransition>().EndLevelTrigger.Invoke();
    }
}

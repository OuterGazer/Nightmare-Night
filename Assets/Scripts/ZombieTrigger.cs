using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrigger : MonoBehaviour
{
    public void OnEnemyDeath()
    {
        AltarStoneExit exitAltar = GameObject.FindObjectOfType<AltarStoneExit>();

        if(this.gameObject.name.Contains("1"))
            exitAltar.isZombie2Killed = true;
        else
            exitAltar.isZombie1Killed = true;

        exitAltar.OnZombieKilled();
    }
}

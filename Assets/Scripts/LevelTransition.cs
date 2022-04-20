using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] GameObject exitAltar;
    [SerializeField] GameObject altarFire;

    private Action endLevelTrigger;
    public Action EndLevelTrigger => this.endLevelTrigger;

    private void Start()
    {
        this.altarFire.SetActive(false);

        this.endLevelTrigger = new Action(OnAltarActivation);
    }

    public void OnAltarActivation()
    {
        this.altarFire.SetActive(true);

        this.exitAltar.transform.Translate(1.0f, 0.0f, 1.0f, this.gameObject.transform);

        // Move altar out of the way to show the exit stairs and light the fire
    }
}

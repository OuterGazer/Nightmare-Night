using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHitPoints = default;
    private float curHitPoints;

    // Start is called before the first frame update
    void Start()
    {
        this.curHitPoints = this.maxHitPoints;
    }

    public void DealDamage(float inDamage)
    {
        this.curHitPoints -= inDamage;

        if(this.curHitPoints <= 0)
        {
            this.curHitPoints = 0;
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        Debug.Log("Player is dead!");
    }
}

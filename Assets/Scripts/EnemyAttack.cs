using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float damage = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AttackHitEvent()
    {
        if (this.target == null) { return; }

        Debug.Log("Bang! Bang!");
    }
}

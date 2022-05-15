using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float damage = default;


    private PlayerHealth target;

    private void Awake()
    {
        this.target = GameObject.FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        if (this.target == null) { return; }

        this.target.DealDamage(this.damage);
    }
}

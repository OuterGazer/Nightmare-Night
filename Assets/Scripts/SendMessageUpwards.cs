using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageUpwards : MonoBehaviour
{
    [SerializeField] EnemyAttack enemyAttack;

    public void AttackHitEvent()
    {
        this.enemyAttack.SendMessage("AttackHitEvent");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageUpwards : MonoBehaviour
{
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyMover enemyMover;

    public void AttackHitEvent()
    {
        this.enemyAttack.SendMessage("AttackHitEvent");
    }

    public void SetEnemySpeed(float inSpeed)
    {
        this.enemyMover.SendMessage("SetSpeed", inSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer bossGlowingSkin;

    [SerializeField] EnemyHealth[] bossRoomEnemies;

    [SerializeField] GameObject exitDoor;

    private bool isBossDead = false;

    public void KillBoss(Vector3 hitPoint)
    {
        if (this.isBossDead) { return; }

        this.gameObject.GetComponent<BossRay>().enabled = false;
        this.gameObject.GetComponent<LineRenderer>().enabled = false;
        this.gameObject.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        this.bossGlowingSkin.material.color = Color.white;
        this.bossGlowingSkin.material.DisableKeyword("_EMISSION");

        this.gameObject.AddComponent<Rigidbody>().AddForceAtPosition(hitPoint.normalized * 10, hitPoint, ForceMode.Impulse);

        this.bossGlowingSkin.GetComponentInParent<EnemyHealth>().ProcessDeath();
        foreach (EnemyHealth item in this.bossRoomEnemies)
            item.ProcessDeath();

        this.isBossDead = true;

        this.exitDoor.transform.Rotate(Vector3.up, -105.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.isBossDead) { return; }

        if (collision.gameObject.CompareTag("Axe"))
            KillBoss(collision.impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehaviour : MonoBehaviour
{
    [SerializeField] Weapon axe;

    private void OnCollisionEnter(Collision collision)
    {
        this.axe.OnAxeCollision(collision.gameObject.tag);
    }
}

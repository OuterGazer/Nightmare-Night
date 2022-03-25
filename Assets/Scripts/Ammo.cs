using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] int ammoAmount = default;
    /// <summary>
    /// Gets the current ammo amount for this weapon.
    /// </summary>
    public int AmmoAmount => this.ammoAmount;

    public void AddAmmo(int inAmmo)
    {
        this.ammoAmount += inAmmo;
    }

    public void SubtractAmmo()
    {
        this.ammoAmount--;

        if (this.ammoAmount <= 0)
            this.ammoAmount = 0;
    }
}

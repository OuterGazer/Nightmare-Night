using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmount;
        public int maxAmmoAmount;
    }

    [SerializeField] AmmoSlot[] ammoSlots;


    public int GetCurrentAmmoAmount(AmmoType inAmmoType)
    {
        return GetAmmoSlot(inAmmoType).ammoAmount;
    }
    public bool AddAmmo(AmmoType inAmmoType, int inAmmoAcquired)
    {
        if (GetAmmoSlot(inAmmoType).ammoAmount == GetAmmoSlot(inAmmoType).maxAmmoAmount)
        { 
            return false;
        }
        else
        {
            GetAmmoSlot(inAmmoType).ammoAmount += inAmmoAcquired;
            return true;
        }
        
        
    }

    public void SubtractAmmo(AmmoType inAmmoType)
    {
        int curAmmoAmount = GetAmmoSlot(inAmmoType).ammoAmount--;

        if (curAmmoAmount <= 0)
            GetAmmoSlot(inAmmoType).ammoAmount = 0;
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach(AmmoSlot item in ammoSlots)
        {
            if(item.ammoType == ammoType)
            {
                return item;
            }
        }

        return null;
    }
}

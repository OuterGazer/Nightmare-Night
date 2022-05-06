using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TextMeshProUGUI ammoText;


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

            UpdateAmmoText(GetAmmoSlot(inAmmoType).ammoAmount);

            return true;
        }
        
        
    }

    public void SubtractAmmo(AmmoType inAmmoType)
    {
        int curAmmoAmount = GetAmmoSlot(inAmmoType).ammoAmount--;

        if (curAmmoAmount <= 0)
            GetAmmoSlot(inAmmoType).ammoAmount = 0;

        UpdateAmmoText(GetAmmoSlot(inAmmoType).ammoAmount);
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

    private void UpdateAmmoText(int ammoAmount)
    {
        this.ammoText.text = ammoAmount.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFunctionality : MonoBehaviour
{
    [SerializeField] GameObject menuToReturn;

    public void OnClickReturn()
    {
        this.menuToReturn.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

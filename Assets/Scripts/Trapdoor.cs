using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    public void OnTalkingWithMel()
    {
        this.gameObject.transform.Rotate(Vector3.right, 90.0f);

    }
}

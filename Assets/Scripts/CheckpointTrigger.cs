using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool isCheckpointActivated = false;
    public bool IsCheckpointActivated => isCheckpointActivated;

    private void Awake()
    {
        CheckpointTrigger[] checkpointTriggers = GameObject.FindObjectsOfType<CheckpointTrigger>();

        if(checkpointTriggers.Length > 1)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            this.isCheckpointActivated = true;
    }
}

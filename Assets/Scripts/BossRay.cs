using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRay : MonoBehaviour
{
    [SerializeField] GameObject boss;

    private LineRenderer lifeLine;

    private void Awake()
    {
        this.lifeLine = this.gameObject.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.lifeLine.SetPosition(0, this.gameObject.transform.position);
        this.lifeLine.SetPosition(1, new Vector3(this.boss.transform.position.x, this.boss.transform.position.y + 2.5f, this.boss.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        this.lifeLine.SetPosition(1, new Vector3(this.boss.transform.position.x, this.boss.transform.position.y + 2.5f, this.boss.transform.position.z));
    }
}

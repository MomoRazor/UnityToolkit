using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimer : MonoBehaviour
{

    public GameObject bossGo;
    float stopwatch = 0;
    public float spawnAtSeconds = 30;
    bool spawned = false;

    // Update is called once per frame
    void Update()
    {
        if(spawned) return;

        stopwatch += Time.deltaTime;

        if(stopwatch > spawnAtSeconds) {
            GameObject boss = Instantiate(bossGo);
            boss.transform.position = this.transform.position + new Vector3(0, 0, -5);
            spawned = true;
        }
    }
}

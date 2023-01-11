using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject[] mobs;
    public island_generator islandGenerator;
    public int spawnEveryXSec = 1;
    List<GameObject> tiles = new List<GameObject>();
    bool primed = false;
    float elapsedTime = 0f;
    int ticker = 0;

    // Update is called once per frame
    void Update()
    {
        if(islandGenerator.getIsGenerated() && !primed) {
            tiles = islandGenerator.getTiles();
            primed = true;
            return;
        }

        if(!primed){
            return;
        }

        int tick = Mathf.FloorToInt(elapsedTime);

        if(tick > ticker){
            ticker = tick;
            spawnMob();
        }

        elapsedTime += Time.deltaTime;
    }

    void spawnMob () {
        if(ticker % spawnEveryXSec == 0){
            Transform tileTransform = tiles[Random.Range(0, tiles.Count)].transform;

            int rand = Random.Range(0, mobs.Length);
            
            GameObject mob = Instantiate(mobs[rand]);
            mob.transform.position = new Vector3(tileTransform.position.x, tileTransform.position.y, 20);
        }
    }
}

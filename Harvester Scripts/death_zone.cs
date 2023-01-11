using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_zone : MonoBehaviour
{
    public GameObject island;
    public GameObject bossSpawner;
    public float closingSpeed = 1;
    public float minimumRadius = 5;

    float currentRadius = 0;
    float initialRadius = 0;
    island_generator islandGenerator;
    bool runOnce = true;
    List<GameObject> tiles;
    

    void Start () {
        islandGenerator = island.GetComponent<island_generator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!islandGenerator.getIsGenerated()){
            return;
        }

        if(runOnce){
            currentRadius = islandGenerator.getRadius();
            initialRadius = currentRadius;
            tiles = islandGenerator.getTiles();
            runOnce = false;
            return;
        }

        for(int i = 0; i < tiles.Count; i++){
            Vector3 tilePosition = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y, 0);
            Vector3 bossPosition = new Vector3(bossSpawner.transform.position.x, bossSpawner.transform.position.y, 0);
            float distance = Vector3.Distance(tilePosition,bossPosition);

            if( distance >= currentRadius ) {
                tiles[i].GetComponent<tile_death>().Kill();
            } else {
                tiles[i].GetComponent<tile_death>().Revive();
            }
        }

        currentRadius = Mathf.Lerp(currentRadius, minimumRadius, Time.deltaTime / closingSpeed );

        // if(Input.GetKeyDown(KeyCode.Space)){
        //     PushBackRadius(0.5f);
        // }

    }

    public void PushBackRadius (float value) {
        currentRadius += value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class island_generator : MonoBehaviour
{
    public Vector2Int mapSize = new Vector2Int(101,101);
    public bool isRadial = false;
    public int numberOfMiners = 1;
    public int minerTtl = 20;
    public GameObject[] ground;
    public GameObject[] deadGround;
    public GameObject wallCollider;

    int[][] virtualMap;
    List<GameObject> mapObjects = new List<GameObject>();
    bool isGenerated = false;

    // Start is called before the first frame update
    public void GenerateIsland()
    {
        // Initialise virtual map
        virtualMap = new int[mapSize.x][];
        for(int i = 0; i < mapSize.y; i++){
            virtualMap[i] = new int[mapSize.y];
            for(int j = 0;j < mapSize.y; j++){
                virtualMap[i][j] = 0;
            }
        }

        // Generate virtual map
        // Vector2Int minerPositionStartingPoint = new Vector2Int(mapSize.x-1, mapSize.y/2);
        Vector2Int minerPositionStartingPoint = new Vector2Int(mapSize.x/2, mapSize.y/2);

        // Vector2Int minerPositionStartingPoint = Vector2Int.zero;

        // Debug.Log("Start position: " + minerPositionStartingPoint);

        // Radial generation
        if(isRadial){
            GenerateRadialMap(minerPositionStartingPoint);
        } else {
            GenerateRandomMap(minerPositionStartingPoint);
        }
    }

    void GenerateRandomMap (Vector2Int startingPoint) {

        for(int miner = 0; miner < numberOfMiners; miner++){

            Vector2Int minerPosition = startingPoint;

            for(int life = 0; life < minerTtl; life++){
                int randX = 0;
                int randY = 0;

                if(life != 0){
                    Vector2Int newMinerPosition;

                    do{
                        randX = Random.Range(-1, 2);
                        randY = Random.Range(-1, 2);

                        newMinerPosition = minerPosition + new Vector2Int(randX, randY);
                    } while (
                        newMinerPosition.x < 0 || newMinerPosition.x > mapSize.x - 1
                        ||
                        newMinerPosition.y < 0 || newMinerPosition.y > mapSize.y - 1
                    );
                    minerPosition = newMinerPosition;
                }
                
                virtualMap[minerPosition.x][minerPosition.y] = 1;

                if(minerPosition.x + 1 < mapSize.x - 1){
                    virtualMap[minerPosition.x + 1][minerPosition.y] = 1;
                }

                if(minerPosition.x - 1 > 0 + 1){
                    virtualMap[minerPosition.x - 1][minerPosition.y] = 1;
                }

                if(minerPosition.y + 1 < mapSize.y - 1){
                    virtualMap[minerPosition.x][minerPosition.y + 1] = 1;
                }

                if(minerPosition.y - 1 > 0 + 1){
                    virtualMap[minerPosition.x][minerPosition.y - 1] = 1;
                }
                
            }
            
        }

        SpawnMap(startingPoint);
    }

    void GenerateRadialMap (Vector2Int startingPoint) {
        int maxDistance = (mapSize.x/2) - 1;
        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                float distance = Vector2Int.Distance(startingPoint, new Vector2Int(x, y));
                if(distance < maxDistance){
                    virtualMap[x][y] = 1;
                }
            }
        }

        SpawnMap(startingPoint);
    }

    void SpawnMap (Vector2Int startPosition) {
        Transform parent = this.transform;

        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                Vector3 center = new Vector3(0 - startPosition.x + x, 0 - startPosition.y + y, 0);
                List<Vector3> position = new List<Vector3>();
                position.Add(center);

                // if(virtualMap[x][y] == 0) continue;
                if(virtualMap[x][y] == 0) {
                    if(
                        x < mapSize.x-1 && virtualMap[x+1][y] == 1 ||
                        x > 0 && virtualMap[x-1][y] == 1 ||
                        y < mapSize.y-1 && virtualMap[x][y+1] == 1 ||
                        y > 0 && virtualMap[x][y-1] == 1 
                    ) {
                        GameObject wall = GameObject.Instantiate(wallCollider);
                        wall.transform.position = position[0] + new Vector3(0, 0, 999f);
                        wall.transform.parent = parent;
                    }
                    continue;
                }

                for(int i = 0; i < position.Count; i++){
                    int selection = Random.Range(0, ground.Length);

                    float startZ = 100f;
                    
                    GameObject deadTile = GameObject.Instantiate(deadGround[selection]);
                    deadTile.transform.position = position[i] + new Vector3(0, 0, startZ + (0.2f * (float) y));
                    deadTile.transform.parent = parent;

                    GameObject tile = GameObject.Instantiate(ground[selection]);
                    tile.transform.position = position[i] + new Vector3(0, 0, startZ + (0.2f * (float) y) - 0.1f);
                    tile.transform.parent = parent;

                    mapObjects.Add(tile);
                }
            }
        }

        isGenerated = true;
    }

    public bool getIsGenerated () {
        return isGenerated;
    }

    public List<GameObject> getTiles () {
        return mapObjects;
    }

    public float getRadius () {
        return ((float) mapSize.x/2f);
    }
}

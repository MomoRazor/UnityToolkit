using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkMapGenerator : MonoBehaviour
{
    //128x128
    public GameObject[] ground, lava;
    public GameObject colliderBlock;
    public Vector2Int mapSize = new Vector2Int(150, 150);
    public int[][] map;
    public List<GameObject> spawnedGround = new List<GameObject>();
    public List<GameObject> spawnedLava = new List<GameObject>();
    public List<Vector3> spawnedLavaPositions = new List<Vector3>();
    public List<GameObject> spawnedColliders = new List<GameObject>();
    public GameObject[][] spawnedMap;
    public Vector3 camPosOriginal;
    MauroSpawner mSpawner;
    public float groundBlockDiff = 0;
    public float multiplier = 0;
    public List<GameObject> spawnedBuildings = new List<GameObject>();
    public int borderoffset = 20;

    //Game
    public GameObject playerPrefab, soulPitPrefab;
    GameObject player, soulPit;

    private void Start()
    {
        //Random.InitState(1000);
        mSpawner = GetComponent<MauroSpawner>();
        SpawnMap();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F)) {
            SpawnMap();
        }
        */
    }

    public void SpawnMap() 
    {
        ResetVars();
        CreateTemplate(20, 100);
        InitMap();
        SetUpGame();
    }

    void ResetVars() 
    {
        Globals.isBuildMode = false;
        Globals.playerDead = false;

        GameObject[] so = GameObject.FindGameObjectsWithTag("soulSpawn");
        for (int i = so.Length - 1; i > -1; i--)
        {
            Destroy(so[i], 0);
        }

        for (int i = spawnedGround.Count - 1; i > -1; i--) 
        {
            Destroy(spawnedGround[i], 0);
        }
        spawnedGround.Clear();

        for (int i = spawnedLava.Count - 1; i > -1; i--)
        {
            Destroy(spawnedLava[i], 0);
        }
        spawnedLava.Clear();

        for (int i = spawnedColliders.Count - 1; i > -1; i--)
        {
            Destroy(spawnedColliders[i], 0);
        }
        spawnedColliders.Clear();

        for (int i = mSpawner.Humans.Count - 1; i > -1; i--)
        {
            Destroy(mSpawner.Humans[i], 0);
        }
        mSpawner.Humans.Clear();

        for (int i = mSpawner.Angels.Count - 1; i > -1; i--)
        {
            Destroy(mSpawner.Angels[i], 0);
        }
        mSpawner.Angels.Clear();

        for (int i = spawnedBuildings.Count - 1; i > -1; i--)
        {
            Destroy(spawnedBuildings[i], 0);
        }
        spawnedBuildings.Clear();

        map = new int[mapSize.x][];
        spawnedMap = new GameObject[mapSize.x][];

        for (int i = 0; i < mapSize.x; i++)
        {
            spawnedMap[i] = new GameObject[mapSize.y];
            map[i] = new int[mapSize.y];
        }

        if (player) {
            Destroy(player);
        }

    }

    void CreateTemplate(int numberOfMiners = 10, int numberOfBlocks = 100) 
    {
        Vector2Int[] minerLoc = new Vector2Int[numberOfMiners];
        for (int i = 0; i < minerLoc.Length; i++) 
        {
            minerLoc[i] = new Vector2Int(Random.Range((mapSize.x / 2) - (mapSize.x / 4), (mapSize.x / 2) + (mapSize.x / 4)), Random.Range((mapSize.y / 2) - (mapSize.y / 4), (mapSize.y / 2) + (mapSize.y / 4)));
            
            for (int j = 0; j < numberOfBlocks; j++) {
                bool next = true;

                do {
                    next = true;

                    Vector2Int temp = minerLoc[i];

                    Vector2Int displace = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));

                    minerLoc[i] += displace;

                    if (minerLoc[i].x < 0 || minerLoc[i].x >= mapSize.x)
                    {
                        minerLoc[i] = temp;
                        next = false;
                    }

                    if (minerLoc[i].y < 0 || minerLoc[i].y >= mapSize.y)
                    {
                        minerLoc[i] = temp;
                        next = false;
                    }
                } while(!next);

                //Center
                if (minerLoc[i].x > -1 + borderoffset && minerLoc[i].y > -1 + borderoffset && minerLoc[i].x < mapSize.x - borderoffset && minerLoc[i].y < mapSize.y - borderoffset) 
                {
                    map[minerLoc[i].x][minerLoc[i].y] = 1;
                }
                //Top
                if (minerLoc[i].x > -1 + borderoffset && minerLoc[i].y + 1 > -1 + borderoffset && minerLoc[i].x < mapSize.x - borderoffset && minerLoc[i].y + 1 < mapSize.y - borderoffset)
                {
                    map[minerLoc[i].x][minerLoc[i].y + 1] = 1;
                }
                //Bottom
                if (minerLoc[i].x > -1 + borderoffset && minerLoc[i].y - 1 > -1 + borderoffset && minerLoc[i].x < mapSize.x - borderoffset && minerLoc[i].y - 1 < mapSize.y - borderoffset)
                {
                    map[minerLoc[i].x][minerLoc[i].y - 1] = 1;
                }
                //Left
                if (minerLoc[i].x - 1 > -1 + borderoffset && minerLoc[i].y > -1 + borderoffset && minerLoc[i].x - 1 < mapSize.x - borderoffset && minerLoc[i].y < mapSize.y - borderoffset)
                {
                    map[minerLoc[i].x - 1][minerLoc[i].y] = 1;
                }
                //Right
                if (minerLoc[i].x + 1 > -1 + borderoffset && minerLoc[i].y > -1 + borderoffset && minerLoc[i].x + 1 < mapSize.x - borderoffset && minerLoc[i].y < mapSize.y - borderoffset)
                {
                    map[minerLoc[i].x + 1][minerLoc[i].y] = 1;
                }

            }
        }

    }

    void InitMap()
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                multiplier = 1f / ((float)map.Length * (float)map[i].Length);

                if (map[i][j] == 1)
                {
                    groundBlockDiff = (multiplier * (((float)i * (float)map[i].Length) + (float)j));
                    spawnedGround.Add(Instantiate(ground[Random.Range(0, ground.Length - 1)], new Vector3((float)j, (float)i, -5f + groundBlockDiff), Quaternion.identity));
                    spawnedGround[spawnedGround.Count - 1].transform.SetParent(this.transform);
                    spawnedMap[j][i] = spawnedGround[spawnedGround.Count - 1];
                }
                else
                {
                    if (i > 1 && i < map.Length - 2)
                    {
                        if (j > 1 && j < map[i].Length - 2)
                        {
                            if (map[i + 1][j] == 1 || map[i - 1][j] == 1 || map[i][j + 1] == 1 || map[i][j - 1] == 1)
                            {
                                spawnedColliders.Add(Instantiate(colliderBlock, new Vector3((float)j, (float)i, 0), Quaternion.identity));
                                spawnedColliders[spawnedColliders.Count - 1].transform.SetParent(this.transform);
                                spawnedMap[j][i] = spawnedColliders[spawnedColliders.Count - 1];
                            }
                        }
                    }
                }

                spawnedLava.Add(Instantiate(lava[Random.Range(0, lava.Length - 1)], new Vector3((float)j, (float)i, 0), Quaternion.identity));
                spawnedLava[spawnedLava.Count - 1].transform.SetParent(this.transform);
                spawnedLavaPositions.Add(spawnedLava[spawnedLava.Count - 1].transform.position);
                if (!spawnedMap[j][i]) {
                    spawnedMap[j][i] = spawnedLava[spawnedLava.Count - 1];
                }
            }
        }

        int[][] temp = new int[map.Length][];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = new int[map[0].Length];
            for (int j = 0; j < temp[i].Length; j++)
            {
                temp[i][j] = map[i][j];
            }
        }

        for (int i = 0; i < temp.Length; i++)
        {
            for (int j = 0; j < temp[i].Length; j++)
            {
                map[i][j] = temp[j][i];
            }
        }
    }

    void SetUpGame() {
        bool spawn = false;
        do {
            int r = Random.Range(0, spawnedGround.Count);
            Vector3Int pitSpawn = new Vector3Int((int)spawnedGround[r].transform.position.x, (int)spawnedGround[r].transform.position.y, -6);

            if (map[pitSpawn.x][pitSpawn.y] == 1 &&
                map[pitSpawn.x][pitSpawn.y + 1] == 1 &&
                map[pitSpawn.x + 1][pitSpawn.y] == 1 &&
                map[pitSpawn.x + 1][pitSpawn.y + 1] == 1 &&
                map[pitSpawn.x - 1][pitSpawn.y] == 1) {
                spawn = true;
            }

            if (spawn) {
                soulPit = Instantiate(soulPitPrefab, pitSpawn, Quaternion.identity);
                spawnedBuildings.Add(soulPit);
                Vector3 playerSpawnPoint = new Vector3(pitSpawn.x - 1, pitSpawn.y, -7);
                player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
                Camera.main.transform.position = new Vector3(playerSpawnPoint.x, playerSpawnPoint.y, -20);
                camPosOriginal = Camera.main.transform.position;

                map[pitSpawn.x][pitSpawn.y] = 2;
                map[pitSpawn.x + 1][pitSpawn.y] = 2;
                map[pitSpawn.x + 1][pitSpawn.y + 1] = 2;
                map[pitSpawn.x][pitSpawn.y + 1] = 2;
            }
        } while (!spawn);

        //for testing
        //MarkPathfinder mpf = new MarkPathfinder(map);
        //mpf.SaveMap();
    }

}

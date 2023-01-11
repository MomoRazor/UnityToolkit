using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroSpawner : MonoBehaviour
{
    public List<GameObject> Humans;
    public List<GameObject> Angels;

    public GameObject Human;
    public GameObject Angel;

    private float humanPeriod = 0.85f;
    private float lastHuman;

    private float angelPeriod = 4f;
    private float lastAngel;

    // Start is called before the first frame update
    void Start()
    {
        lastHuman = Time.time;
        lastAngel = Time.time;
        Humans = new List<GameObject>();
        Angels = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastHuman >= humanPeriod)
        {
            lastHuman = Time.time;
            SpawnSoul();
        }

        if (Time.time - lastAngel >= angelPeriod)
        {
            lastAngel = Time.time;
            SpawnAngel();
        }
    }

    void SpawnSoul()
    {
        int r = Random.Range(0, GetComponent<MarkMapGenerator>().spawnedGround.Count);
        Vector3 playerSpawnPoint = new Vector3((int)GetComponent<MarkMapGenerator>().spawnedGround[r].transform.position.x, (int)GetComponent<MarkMapGenerator>().spawnedGround[r].transform.position.y, -6f);
        GameObject human = Instantiate(Human, playerSpawnPoint, Quaternion.identity);
        Humans.Add(human);

       
    }

    void SpawnAngel()
    {
        if(Angels.Count < 10)
        {
            int r = Random.Range(0, GetComponent<MarkMapGenerator>().spawnedGround.Count);
            Vector3 playerSpawnPoint = new Vector3((int)GetComponent<MarkMapGenerator>().spawnedGround[r].transform.position.x, (int)GetComponent<MarkMapGenerator>().spawnedGround[r].transform.position.y, -6f);
            GameObject angel = Instantiate(Angel, playerSpawnPoint, Quaternion.identity);
            Angels.Add(angel);
        }
    }
}

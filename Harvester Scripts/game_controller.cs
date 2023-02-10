using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{
    public GameObject player;
    public island_generator islandGenerator;
    float damageMultiplier = 1f;

    void SpawnPlayer (){
        GameObject spawnedPlayer = Instantiate(player);
        spawnedPlayer.transform.position = Vector3.zero;

        Camera.main.GetComponent<CameraFollow2D>().setTarget(spawnedPlayer);
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        islandGenerator.GenerateIsland();

        SpawnPlayer();

        GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>().reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

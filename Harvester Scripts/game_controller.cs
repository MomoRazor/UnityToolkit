using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    bool isPlayingSound;
    float soundTimeout = 0f;

    public GameObject player;
    public island_generator islandGenerator;
    float damageMultiplier = 1f;

    void SpawnPlayer (){
        GameObject spawnedPlayer = Instantiate(player);
        spawnedPlayer.transform.position = Vector3.zero;

        Camera.main.GetComponent<CameraMovement>().player = spawnedPlayer;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }


    void Start()
    {
        islandGenerator.GenerateIsland();

        SpawnPlayer();

        GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>().reset();
    }

    void Update()
    {
        if (isPlayingSound)
        {
            soundTimeout -= Time.deltaTime;
            if (soundTimeout <= 0)
            {
                isPlayingSound = false;
            }
        }
    }


    public void PlayHitSound()
    {
        if(!isPlayingSound)
        {
            AudioClip clip;
            float random = Random.Range(0f, 2f);
            if (random < 1)
            {
                clip = hit1;
            }
            else
            {
                clip = hit3;
            }
            gameObject.GetComponent<AudioSource>().PlayOneShot(clip);

            isPlayingSound = true;
            soundTimeout = 0.3f;
        }
    }
        
}

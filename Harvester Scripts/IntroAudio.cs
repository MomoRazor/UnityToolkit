using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAudio : MonoBehaviour
{
    public AudioClip intro1, intro2;
    public AudioSource audioSource;
    public float introLength = 15f;
    public SpriteRenderer frontDrop;
    float currentTime = 0f;
    bool playOnce1 = true;
    bool playOnce2 = true;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= 0f && currentTime <= 10f){
            float opacity = Mathf.Lerp(frontDrop.color.a, 0, Time.deltaTime / 1f);
            frontDrop.color = new Color(frontDrop.color.r, frontDrop.color.g, frontDrop.color.b, opacity);
        }

        if(currentTime >= 1.5f && playOnce1){
            playOnce1 = false;
            audioSource.PlayOneShot(intro1);
        }

        if(currentTime >= 9.5f && playOnce2){
            playOnce2 = false;
            audioSource.PlayOneShot(intro2);
        }

        if(currentTime >= 14f){
            float opacity = Mathf.Lerp(frontDrop.color.a, 1.0f, Time.deltaTime / 0.5f);
            frontDrop.color = new Color(frontDrop.color.r, frontDrop.color.g, frontDrop.color.b, opacity);
        }

        if(currentTime >= introLength){
            SceneManager.LoadScene(1);
        }
    }
}

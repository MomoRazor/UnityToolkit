using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistor : MonoBehaviour
{
    public AudioClip menuMusic, gameMusic, defeatMusic;
    int lastScore = 0;
    int difficulty = 1;
    bool victory = true;
    AudioSource aS;
    bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        aS = this.GetComponent<AudioSource>();
    }

    void Update () {
        Camera.main.GetComponent<AudioListener>().enabled = !isMuted;
    }

    public void reset () {
        aS.Stop();
        aS.clip = gameMusic;
        aS.Play();
        lastScore = 0;
        difficulty = 1;
        victory = true;
    }

    public void setLastScore (int score) {
        lastScore = score;
    }

    public int getLastScore () {
        return lastScore;
    }

    public void win () {
        victory = true;
        aS.Stop();
        aS.clip = menuMusic;
        aS.Play();
    }

    public void lose () {
        victory = false;
        aS.Stop();
        aS.clip = defeatMusic;
        aS.Play();
    }

    public bool toggleMute () {
        isMuted = !isMuted;
        return isMuted;
    }
}

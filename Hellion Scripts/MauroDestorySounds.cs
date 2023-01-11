using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroDestorySounds : MonoBehaviour
{
    public void Play(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip, 0.4f);
    }

    public void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}

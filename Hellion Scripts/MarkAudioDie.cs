using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAudioDie : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}

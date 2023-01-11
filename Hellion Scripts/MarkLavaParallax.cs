using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkLavaParallax : MonoBehaviour
{

    public MarkMapGenerator mmg;

    Vector3[] next;

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = (Camera.main.transform.position - mmg.camPosOriginal) * 0.1f;
        
        for (int i = 0; i < mmg.spawnedLava.Count; i++)
        {
            mmg.spawnedLava[i].transform.position = Vector3.Lerp(mmg.spawnedLava[i].transform.position, mmg.spawnedLavaPositions[i] + diff, Time.deltaTime * 0.2f);
        }
        
    }
}

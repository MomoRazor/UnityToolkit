using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroBlood : MonoBehaviour
{
    private float aliveStart;
    private float alivePeriod = 10f;

    // Start is called before the first frame update
    void Start()
    {
        aliveStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - aliveStart >= alivePeriod)
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkCameraFollow : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 2);
    }
}

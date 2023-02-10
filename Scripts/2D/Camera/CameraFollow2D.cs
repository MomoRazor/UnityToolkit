using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public float followSpeed = 8f;

    public GameObject target;

    public void setTarget (GameObject newTarget){
        target = newTarget;
    }


    void FixedUpdate()
    {
        Vector3 newTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y, Camera.main.transform.position.z);
        
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newTargetPosition,  followSpeed);
    }
}

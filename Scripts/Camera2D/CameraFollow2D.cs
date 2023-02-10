using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public float followSpeed = 8f;

    public GameObject _target;

    public void setTarget (GameObject target){
        _target = target;
    }


    void FixedUpdate()
    {
        Vector3 newTarget = new Vector3(_target.transform.position.x, _target.transform.position.y, Camera.main.transform.position.z);
        
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newTarget, Time.deltaTime * followSpeed);
    }
}

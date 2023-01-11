using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySlime : MonoBehaviour
{
    SlimeSpawner parentSlime;
    SwarmMovement movement;
    float movementDelay = 0.8f;
    float movementTimer = 0f;

    void Start()
    {
        movement = gameObject.GetComponent<SwarmMovement>();
        movement.enabled = false;
    }

    void FixedUpdate()
    {
        if (movementTimer <= movementDelay)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer >= movementDelay)
            {
                movement.enabled = true;
            }
        }      
    }

    public void SetParentSlime(SlimeSpawner parent)
    {
        parentSlime = parent;
    }

    public void alertParentOnDeath()
    {
        parentSlime.BabyDeathAlert();
    }
}

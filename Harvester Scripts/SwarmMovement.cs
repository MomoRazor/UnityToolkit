using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{

    public float speed = 20f;
    GameObject player;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
  
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // disabled because of slimes
        // if (body.velocity.x < -0.1f)
        // {
        //     spriteRenderer.flipX = true;
        // }
        // else if (body.velocity.x > 0.1f)
        // {
        //     spriteRenderer.flipX = false;
        // }
    }

    void FixedUpdate()
    {
        if (!gameObject.GetComponent<SwarmAttack>().getIsAttacking())
        {
            Vector3 direction = Vector3.Normalize(new Vector3(player.transform.position.x,player.transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));
            body.velocity = direction * speed;
        }
        else
        {
            body.velocity = new Vector3(0f, 0f, 0f);
        }    
    }
}

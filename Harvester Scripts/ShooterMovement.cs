using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMovement : MonoBehaviour
{
    // TODO strafing is a bit wonky, needs to make sure its always in range
    float maxShootingDistance;
    public float minPlayerDistance = 3f;
    public float speed = 8f;
    float randomizedMinPlayerDistance;
    float distanceToPlayer = 0f;
    Vector3 direction;
    bool isChasing = true;
    bool isStrafing = false;
    float chillingTimer = 0f;
    float strafingTimer = 0f;
    GameObject player;
    
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");

        randomizedMinPlayerDistance = minPlayerDistance * Random.Range(0.75f, 1.25f);
    }

    void Update()
    {
        if (body.velocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (body.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (isChasing)
        {
            if (distanceToPlayer > randomizedMinPlayerDistance)
            {
                direction = Vector3.Normalize(new Vector3(player.transform.position.x,player.transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));
                body.velocity = direction * speed;
            }
            else
            {
                StartChilling();
            }
        }       
        else if (!isStrafing)
        {
            chillingTimer -= Time.deltaTime;
            if (chillingTimer <= 0f)
            {
                StartStrafing();
            }
        }
        else
        {
            body.velocity = direction * speed;
            strafingTimer -= Time.deltaTime;
            if (strafingTimer <= 0f)
            {
                StopStrafing();
            }
        }     
    }

    void StartChilling()
    {
        isChasing = false;
        chillingTimer = Random.Range(1f, 3f);

        body.velocity = new Vector3(0f, 0f, 0f);
    }

    void StartStrafing()
    {
        isStrafing = true;
        strafingTimer = Random.Range(1f, 3f);

        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
    }

    void StopStrafing()
    {
        isStrafing = false;

        if (distanceToPlayer > randomizedMinPlayerDistance)
        {
            isChasing = true;
        }      
    }
}

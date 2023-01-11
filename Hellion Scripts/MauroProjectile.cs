using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroProjectile : MonoBehaviour
{
    public bool fired = false;

    private float lifespan = 0.5f;
    private float firedTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10.9f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            if(Time.time - firedTime >= lifespan)
            {
                Destroy(gameObject);
            }
            //Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }
    }

    public void Fire(Vector2 vel)
    {
        firedTime = Time.time;
        fired = true;
        GetComponent<Rigidbody2D>().velocity = vel;
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Human" || collision.tag == "Angel")
        {
            Destroy(gameObject);
        }
    }
}

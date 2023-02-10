using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeGhost : MonoBehaviour
{
    public GameObject head;
    public GameObject legs;
    public SpriteRenderer headSprite;
    public SpriteRenderer legsSprite;
    private Rigidbody2D body;

    private float lifetime = 0.2f;
    private float dodgeTimer;

    void Start()
    {
        headSprite = head.GetComponent<SpriteRenderer>();
        legsSprite = legs.GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();

        headSprite.color = new Color32(255, 255, 255, 64);
        legsSprite.color = new Color32(255, 255, 255, 64);

        dodgeTimer = lifetime;
    }

    void Update()
    {
        dodgeTimer -= Time.deltaTime;

        byte alpha = (byte)((dodgeTimer / lifetime) * 64);
        headSprite.color = new Color32(255, 255, 255, alpha);
        legsSprite.color = new Color32(255, 255, 255, alpha);

        if (dodgeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void StartDodging(Vector2 startVelocity)
    {
        body.velocity = startVelocity;
        dodgeTimer = 0f;
    }
}

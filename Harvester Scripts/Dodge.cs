using UnityEngine;

public class Dodge : MonoBehaviour
{
    public bool isDodging = false;
    public bool canDodge = true;
    float dodgeSpeed = 20f;
    float dodgeDuration = 0.3f;
    public float dodgeTimeout = 1.5f;
    float dodgeTimer = 0f;
    Rigidbody2D body;
    CapsuleCollider2D col;
    
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        dodgeTimer = dodgeTimeout;
    }

    void Update()
    {
        dodgeTimer += Time.deltaTime;
        if (dodgeTimer >= dodgeTimeout)
        {
            canDodge = true;
        }
        else{
            canDodge = false;
        }

        if (isDodging)
        {
            body.velocity *= 0.9f;
            if (dodgeTimer >= dodgeDuration)
            {
                StopDodging();
            }
        }

        if (Input.GetKeyDown("space"))
        {
            if (canDodge)
            {
                StartDodging();
            }       
        }
    }

    void StartDodging()
    {
        Vector2 inputDodgeVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDodgeVelocity.Normalize();
        if (inputDodgeVelocity.magnitude > 0.1f)
        {
            isDodging = true;
            canDodge = false;
            body.velocity = inputDodgeVelocity * dodgeSpeed;
            col.enabled = false;
            dodgeTimer = 0f;
        }       
    }

    void StopDodging()
    {
        col.enabled = true;
        isDodging = false;
    }
}

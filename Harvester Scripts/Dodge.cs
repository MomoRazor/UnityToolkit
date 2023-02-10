using UnityEngine;

public class Dodge : MonoBehaviour
{
    public AudioClip dash;
    public AudioClip fail;
    AudioSource audio;
    bool hasFailedDodging = false;

    public bool isDodging = false;
    public bool canDodge = true;
    public float dodgeSpeed = 30f;
    float dodgeDuration = 0.2f;
    public float dodgeTimeout = 1f;
    float dodgeTimer = 0f;
    Rigidbody2D body;
    CapsuleCollider2D col;

    public GameObject ghost;
    private Transform startPosition;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        dodgeTimer = dodgeTimeout;
        audio = gameObject.GetComponent<AudioSource>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
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

            GameObject ghostFrame = Instantiate(ghost, startPosition);
            ghostFrame.GetComponent<DodgeGhost>().head.GetComponent<SpriteRenderer>().sprite = playerMovement.headSprite.sprite;

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
                hasFailedDodging = false;
            }
            else if (!hasFailedDodging)
            {
                audio.volume = 0.1f;
                audio.PlayOneShot(fail);
                hasFailedDodging = true;
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

            audio.volume = 0.15f;
            audio.PlayOneShot(dash);

            startPosition = transform;
        }       
    }

    void StopDodging()
    {
        col.enabled = true;
        isDodging = false;
    }
}

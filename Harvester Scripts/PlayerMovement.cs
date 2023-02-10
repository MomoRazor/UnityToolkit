using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Sprite lookLeftDown;
    public Sprite lookLeftUp;
    public Sprite lookRightDown;
    public Sprite lookRightUp;
    public Sprite lookLeftDownOverlay;
    public Sprite lookLeftUpOverlay;
    public Sprite lookRightDownOverlay;
    public Sprite lookRightUpOverlay;
    public GameObject head;
    public SpriteRenderer headSprite;
    public GameObject legs;
    public float speedMultiplier = 10f;
    public GameObject weapon;

    Vector2 inputVelocity;
    Rigidbody2D body;
    Dodge dodge;
    private SpriteRenderer _renderer;
    private Gun _gun;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        dodge = gameObject.GetComponent<Dodge>();
        headSprite = head.GetComponent<SpriteRenderer>();
        _gun = weapon.GetComponentInChildren<Gun>();
    }

    void Update()
    {
        inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVelocity = Vector2.ClampMagnitude(inputVelocity, 1.0f);

        if (inputVelocity.magnitude >= 0.1f){
            legs.GetComponent<Animator>().enabled = true;
        }
        else{
            legs.GetComponent<Animator>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!dodge.isDodging)
        {
            body.velocity = inputVelocity * speedMultiplier;
        }       
    }

    public void SetFacingDirection(Vector3 direction)
    {
        if (direction.x > 0){
            legs.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<PlayerHealth>().skeleton.GetComponent<SpriteRenderer>().flipX = false;

            if (direction.y > 0){
                if (dodge.canDodge)
                {
                    headSprite.sprite = lookRightUpOverlay;
                }
                else{
                    headSprite.sprite = lookRightUp;
                }             
            }
            else{
                if (dodge.canDodge)
                {
                    headSprite.sprite = lookRightDownOverlay;
                }
                else{
                    headSprite.sprite = lookRightDown;
                }     
            }           
        }
        else{
            legs.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<PlayerHealth>().skeleton.GetComponent<SpriteRenderer>().flipX = true;

            if (direction.y > 0){
                if (dodge.canDodge)
                {
                    headSprite.sprite = lookLeftUpOverlay;
                }
                else{
                    headSprite.sprite = lookLeftUp;
                }     
            }
            else{
                if (dodge.canDodge)
                {
                    headSprite.sprite = lookLeftDownOverlay;
                }
                else{
                    headSprite.sprite = lookLeftDown;
                }     
            }
        }
    }
}

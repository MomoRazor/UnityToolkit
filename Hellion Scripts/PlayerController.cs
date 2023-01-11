using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] rightSprite;
    public Sprite[] leftSprite;
    public Sprite bloodBlade;
    public Sprite normalBlade;

    private float lastAnimation;
    private float animationPeriod = 0.03f;

    public AudioClip[] audio;

    private AudioSource audiosrc;

    private float acceleration = 5f;
    private float breakspeed = 15f;
    private float topspeed = 7f;
    private Vector3 speed;
    private Rigidbody2D rig;
    private float idleperiod = 0.3f;
    private float lastAction;
    private bool left = false;

    private int sword_dir = 2; // 0 - left, 2 - right
    private GameObject sword;
    private Vector3 swingTarget;
    private bool midStrike = false;
    private float strikePeriod = 0.5f;

    private float gothit = 0f;
    private float recovery = 0.5f;
    public int health = 5;
    public int totalhealth = 5;
    private float pushback = 10f;
    private bool dead = false;

    private bool no_btn;

    public GameObject HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        lastAnimation = Time.time;
        audiosrc = GetComponent<AudioSource>();
        HealthBar = GameObject.FindGameObjectWithTag("PlayerHealth");
        HealthBar.GetComponent<MauroPlayerHealth>().UpdateHealth(health, totalhealth);
        gothit = Time.time;
        sword = transform.GetChild(0).gameObject;
        lastAction = Time.time;
        rig = GetComponent<Rigidbody2D>();
        speed = Vector3.zero;
    }

    void FixedUpdate()
    {
        MovementCheck();
        IdleStanding();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.isBuildMode)
        {
            FightCheck();
        }
    }

    void MovementCheck()
    {
        rig.velocity = speed;
        no_btn = true;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            no_btn = false;
            sword_dir = 0;
            left = true;
            WalkingAnimation(true);
            //GetComponent<SpriteRenderer>().sprite = leftSprite;
            if (!midStrike)
            {
                sword.transform.eulerAngles = new Vector3(0, 180, 0);
                swingTarget = new Vector3(0, 180, 0);
            }
            speed.x -= acceleration;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            no_btn = false;
            sword_dir = 2;
            left = false;
            WalkingAnimation(false);
            //GetComponent<SpriteRenderer>().sprite = rightSprite;
            if (!midStrike)
            {
                sword.transform.eulerAngles = new Vector3(0, 0, 0);
                swingTarget = new Vector3(0, 0, 0);
            }
            speed.x += acceleration;
        }
        else
        {
            speed.x = Mathf.Lerp(speed.x, 0, Time.deltaTime * breakspeed);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            no_btn = false;
            if (left)
            {
                WalkingAnimation(true);
                //GetComponent<SpriteRenderer>().sprite = leftSprite;
                if (!midStrike)
                {
                    sword.transform.eulerAngles = new Vector3(0, 180, 0);
                    swingTarget = new Vector3(0, 180, 0);
                }
            }
            else
            {
                WalkingAnimation(false);
                // GetComponent<SpriteRenderer>().sprite = rightSprite;
                if (!midStrike)
                {
                    sword.transform.eulerAngles = new Vector3(0, 0, 0);
                    swingTarget = new Vector3(0, 0, 0);
                }
            }
            speed.y += acceleration;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            no_btn = false;
            if (left)
            {
                WalkingAnimation(true);
                //GetComponent<SpriteRenderer>().sprite = leftSprite;
                if (!midStrike)
                {
                    sword.transform.eulerAngles = new Vector3(0, 180, 0);
                    swingTarget = new Vector3(0, 180, 0);
                }
            }
            else
            {
                WalkingAnimation(false);
                //GetComponent<SpriteRenderer>().sprite = rightSprite;
                if (!midStrike)
                {
                    sword.transform.eulerAngles = new Vector3(0, 0, 0);
                    swingTarget = new Vector3(0, 0, 0);
                }
            }
            speed.y -= acceleration;
        }
        else
        {
            speed.y = Mathf.Lerp(speed.y, 0, Time.deltaTime * breakspeed);
        }

        speed = Vector3.ClampMagnitude(new Vector3(Mathf.Clamp(speed.x, -topspeed, topspeed), Mathf.Clamp(speed.y, -topspeed, topspeed), 0), topspeed);
    }

    void FightCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audiosrc.PlayOneShot(audio[Random.Range(2,5)], 0.4f);
            midStrike = true;
            sword.transform.GetComponent<PolygonCollider2D>().enabled = true;
            if (sword_dir == 0)
            {
                sword.transform.eulerAngles = new Vector3(0, 180, 270);
                swingTarget = new Vector3(0, 180, 90);
            }
            else if(sword_dir == 2)
            {
                sword.transform.eulerAngles = new Vector3(0, 0, 250);
                swingTarget = new Vector3(0, 0, 90);
            }
        }
        sword.transform.eulerAngles = new Vector3(swingTarget.x, swingTarget.y,Mathf.Lerp(sword.transform.eulerAngles.z, swingTarget.z, Time.deltaTime * 8));

        if(FastApproximately(sword.transform.eulerAngles.z,swingTarget.z, 5f))
        {
            midStrike = false;
            sword.transform.GetComponent<PolygonCollider2D>().enabled = false;
            if (left)
            {
                swingTarget = new Vector3(0, 180, 0);
            }
            else
            {
                swingTarget = new Vector3(0, 0, 0);
            }
        }

    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }

    void IdleStanding()
    {
        if (no_btn && Time.time - lastAction > idleperiod)
        {
            lastAction = Time.time;
            if(GetComponent<SpriteRenderer>().sprite == idleSprites[0])
            {
                GetComponent<SpriteRenderer>().sprite = idleSprites[1];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = idleSprites[0];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWeapon")
        {
            if (Time.time - gothit >= recovery)
            {
                audiosrc.PlayOneShot(audio[0], 0.3f);
                gothit = Time.time;
                Vector3 direction = transform.position - collision.transform.parent.position;
                speed = direction.normalized * pushback;
                health -= 1;
                HealthBar.GetComponent<MauroPlayerHealth>().UpdateHealth(health, totalhealth);
            }
        }
        if (health <= 0)
        {
            Globals.playerDead = true;
            Destroy(gameObject);
        }
    }

    void WalkingAnimation(bool left)
    {
        if(Time.time - lastAnimation >= animationPeriod)
        {
            lastAnimation = Time.time;
            if (left)
            {
                if (GetComponent<SpriteRenderer>().sprite == leftSprite[0])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[1];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[1])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[2];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[2])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[3];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[3])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[4];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[4])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[5];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[5])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[6];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[7])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[8];
                }
                else if (GetComponent<SpriteRenderer>().sprite == leftSprite[8])
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[0];
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = leftSprite[0];
                }
            }
            else
            {

                if (GetComponent<SpriteRenderer>().sprite == rightSprite[0])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[1];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[1])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[2];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[2])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[3];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[3])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[4];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[4])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[5];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[5])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[6];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[7])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[8];
                }
                else if (GetComponent<SpriteRenderer>().sprite == rightSprite[8])
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[0];
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[0];
                }
            }
        }
      
    }

    public void ChangeBlade(bool back)
    {
        if (!back)
        {
            if (sword.transform.GetComponent<SpriteRenderer>().sprite == normalBlade)
            {
                sword.transform.GetComponent<SpriteRenderer>().sprite = bloodBlade;
            }
        }
        else
        {

            if (sword.transform.GetComponent<SpriteRenderer>().sprite == bloodBlade)
            {
                sword.transform.GetComponent<SpriteRenderer>().sprite = normalBlade;
            }
        }
    }
}

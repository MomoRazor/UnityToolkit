using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    public float closest;
    public int index;
    public AudioClip[] clips;


    public GameObject soul;
    public Sprite[] rightSprite;
    public Sprite[] leftSprite;

    private float lastAnimation;
    private float animationPeriod = 0.03f;

    private AudioSource src;

    private bool dead = false;
    private float pushback = 10f;
    private float arrowPushback = 2f;
    private int souls;
    private float gothit;
    private float recovery = 0.3f;
    private float totalhealth = 5f;
    private float health = 5f;

    private MarkMapGenerator mapscript;
    private MauroSpawner spawnscript;

    private GameObject target;
    private GameObject Player;

    private float speed = 0;
    private float topspeed = 4.5f;
    private float attackTesh = 5f;
    private float saveTesh = 5f;
    private Vector3 vel;
    private Rigidbody2D rig;

    private Vector3 randomDir;
    private float maxIdleSpeed = 5f;

    private float lastAttack;
    private float attackPeriod = 1f;
    private bool left;
    private GameObject sword;
    private Vector3 swingTarget;
    private bool midStrike = false;
    private float strikePeriod = 0.7f;

    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        lastAttack = Time.time;
        randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        randomDir = randomDir.normalized;
        mapscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MarkMapGenerator>();
        spawnscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MauroSpawner>();
        sword = transform.GetChild(0).gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody2D>();
        canvas = transform.GetChild(1).gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceToTarget;
        if (Player != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, Player.transform.position);
        }
        else
        {
            distanceToTarget = 9999f;
        }

        if (Time.time - gothit >= recovery)
        {
            if (distanceToTarget < attackTesh)
            {
                if (!src.isPlaying)
                {
                    src.clip = clips[Random.Range(0, 3)];
                    src.Play();
                }
                canvas.SetActive(true);
                if (distanceToTarget <= 1.1)
                {
                    rig.velocity = Vector3.zero;
                    SwingSword();
                }
                else
                {
                    MoveToTarget();
                }
            }
            else
            {
                src.Stop();
                canvas.SetActive(false);
                MoveToSoul();
            }
        }

        sword.transform.eulerAngles = new Vector3(swingTarget.x, swingTarget.y, Mathf.LerpAngle(sword.transform.eulerAngles.z, swingTarget.z, Time.deltaTime * 8));

        if (FastApproximately(sword.transform.eulerAngles.z, swingTarget.z, 5f))
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

        if (vel.x > 0)
        {
            left = false;
            WalkingAnimation(false);
            //GetComponent<SpriteRenderer>().sprite = rightSprite;
            if (!midStrike)
            {
                sword.transform.eulerAngles = new Vector3(0, 0, 0);
                swingTarget = new Vector3(0, 0, 0);
            }
        }
        else
        {
            left = true;
            WalkingAnimation(true);
            //GetComponent<SpriteRenderer>().sprite = leftSprite;
            if (!midStrike)
            {
                sword.transform.eulerAngles = new Vector3(0, 180, 0);
                swingTarget = new Vector3(0, 180, 0);
            }
        }

    }

    void MoveToSoul()
    {
        closest = 999999f;
        if (spawnscript.Humans.Count != 0)
        {
            float dist = 0f;
            for (int i = 0; i < spawnscript.Humans.Count; i++)
            {
                dist = Vector3.Distance(transform.position, spawnscript.Humans[i].transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    target = spawnscript.Humans[i];
                }
            }
            if (closest <= saveTesh && closest >= 1f && target != null)
            {
                Vector3 direction = target.transform.position - transform.position;

                speed++;
                speed = Mathf.Clamp(speed, -topspeed, topspeed);
                vel = direction.normalized * speed;
            }
            else if (closest <= 1f && target != null)
            {
                vel = Vector3.zero;
                SwingSword();
            }
            else
            {
                transform.GetComponent<CircleCollider2D>().enabled = true;
                vel = randomDir * Random.Range(0, maxIdleSpeed);
            }

        }
        else
        {
            transform.GetComponent<CircleCollider2D>().enabled = true;
            vel = randomDir * Random.Range(0, maxIdleSpeed);
        }


        rig.velocity = vel;

    }

    void MoveToTarget()
    {
        transform.GetComponent<CircleCollider2D>().enabled = false;
        Vector3 direction = Player.transform.position - transform.position;

        speed++;
        speed = Mathf.Clamp(speed, -topspeed, topspeed);
        vel = direction.normalized * speed;

        rig.velocity = vel;
    }

    void SwingSword()
    {
        if (Time.time - lastAttack >= attackPeriod)
        {
            lastAttack = Time.time;
            midStrike = true;
            sword.transform.GetComponent<PolygonCollider2D>().enabled = true;
            if (left)
            {
                sword.transform.eulerAngles = new Vector3(0, 180, 90);
                swingTarget = new Vector3(0, 180, 280);
            }
            else
            {
                sword.transform.eulerAngles = new Vector3(0, 0, 90);
                swingTarget = new Vector3(0, 0, 280);
            }
        }
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        randomDir = Vector3.Reflect(randomDir, collision.contacts[0].normal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            if (Time.time - gothit >= recovery)
            {
                src.PlayOneShot(clips[3], 0.3f);
                gothit = Time.time;
                Vector3 direction = transform.position - Player.transform.position;
                rig.velocity = direction.normalized * pushback;
                health -= 1f;
            }
        }
        else if (collision.tag == "Arrow")
        {
            src.PlayOneShot(clips[3], 0.3f);
            gothit = Time.time;
            Vector3 direction = transform.position - Player.transform.position;
            rig.velocity = direction.normalized * arrowPushback;
            health -= 0.5f;
        }

        if (health <= 0 && !dead)
        {
            dead = true;
            for (int i = 0; i < 3; i++)
            {
                Instantiate(soul, transform.position, Quaternion.identity);
            }
            spawnscript.Angels.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            if (collision.tag == "Weapon" || collision.tag == "Arrow")
            {
                canvas.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2((health / totalhealth), 0.03f);
            }
        }
    }

    void WalkingAnimation(bool left)
    {
        if (Time.time - lastAnimation >= animationPeriod)
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
                    GetComponent<SpriteRenderer>().sprite = rightSprite[0];
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = rightSprite[0];
                }
            }
        }
    }
}

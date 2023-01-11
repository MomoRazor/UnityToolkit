using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public GameObject soul;
    public Sprite[] rightSprite;
    public Sprite[] leftSprite;
    public AudioClip[] audio;
    public GameObject dyingSound;
    public GameObject blood;

    private AudioSource audiosrc;

    private float lastAnimation;
    private float animationPeriod = 0.03f;

    private MarkMapGenerator mapscript;
    private MauroSpawner spawnerscript;
    private float closest = 999999f;
    private GameObject target;

    private float breakspeed = 12f;
    private GameObject player;
    private float topspeed = 3f;
    private Vector3 direction;
    private Vector3 vel;

    private float speed;
    private Rigidbody2D rig;

    private float distanceTresh = 4f;

    private float lifeStarted;
    private float lifeSpan = 15f;

    private Vector3 randomDir;
    private float maxIdleSpeed = 5f;

    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
        mapscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MarkMapGenerator>();
        spawnerscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MauroSpawner>();
        randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        randomDir = randomDir.normalized;
        lifeStarted = Time.time;
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        CheckMovement();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Time.time - lifeStarted >= lifeSpan)
        {
            // TODO call lifetime over method
            Destroy(gameObject);
        }*/
    }

    void CheckMovement()
    {
            bool gettingAttracted = false;

            GameObject[] attrs = GameObject.FindGameObjectsWithTag("Attractor");
            int closestAttr = -1;

            if (attrs.Length > 0)
            {
                closestAttr = -1;
                float minDist = 1000000;
                for (int i = 0; i < attrs.Length; i++)
                {
                    float x = Vector2.Distance(transform.position, attrs[i].transform.position);
                    if (x < minDist && x < attrs[i].GetComponent<MarkAttractor>().radiusOfAttraction)
                    {
                        minDist = x;
                        closestAttr = i;
                        gettingAttracted = true;
                    }
                }
            }

            if (gettingAttracted)
            {
                int[][] m = attrs[closestAttr].GetComponent<MarkAttractor>().mpf.GetPaths();
                Vector2Int curr = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                Vector2Int next = Vector2Int.zero;
                int smallestTile = 100000;

                if (m[curr.x][curr.y] < smallestTile && m[curr.x][curr.y] >= 0)
                {
                    smallestTile = m[curr.x][curr.y];
                    next = new Vector2Int(curr.x, curr.y);
                }

                if (m[curr.x + 1][curr.y] < smallestTile && m[curr.x + 1][curr.y] >= 0)
                {
                    smallestTile = m[curr.x + 1][curr.y];
                    next = new Vector2Int(curr.x + 1, curr.y);
                }

                if (m[curr.x - 1][curr.y] < smallestTile && m[curr.x - 1][curr.y] >= 0)
                {
                    smallestTile = m[curr.x - 1][curr.y];
                    next = new Vector2Int(curr.x - 1, curr.y);
                }

                if (m[curr.x][curr.y + 1] < smallestTile && m[curr.x][curr.y + 1] >= 0)
                {
                    smallestTile = m[curr.x][curr.y + 1];
                    next = new Vector2Int(curr.x, curr.y + 1);
                }

                if (m[curr.x][curr.y - 1] < smallestTile && m[curr.x][curr.y - 1] >= 0)
                {
                    smallestTile = m[curr.x][curr.y - 1];
                    next = new Vector2Int(curr.x, curr.y - 1);
                }

                Vector2 nextPos = new Vector2((float)next.x, (float)next.y);

                Vector2 dir = nextPos - curr;
                dir = dir.normalized * topspeed;
                rig.velocity = dir;

            }else if (Vector3.Magnitude(rig.velocity) == 0)
            {
                vel = (-randomDir) * Random.Range(0, maxIdleSpeed);
                rig.velocity = vel;
            }
            else
            {
                if (player != null && Vector2.Distance(transform.position, player.transform.position) <= distanceTresh)
                {
                    direction = transform.position - player.transform.position;
                    randomDir = direction.normalized;
                    speed++;
                    speed = Mathf.Clamp(speed, -topspeed, topspeed);
                    vel = direction.normalized * speed;
                }
                else
                {
                    vel = randomDir * Random.Range(0, maxIdleSpeed);
                }

                rig.velocity = vel;
            }

            if (rig.velocity.x < 0)
            {
                WalkAnimate(true);
            }
            else if (rig.velocity.x > 0)
            {
                WalkAnimate(false);
            }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Death by friendly weapon
    {
        if(collision.tag == "Weapon")
        {
            collision.transform.parent.GetComponent<PlayerController>().ChangeBlade(false);
            if (!dead)
            {
                Instantiate(soul, new Vector3(transform.position.x, transform.position.y, -11), Quaternion.identity);
                Instantiate(blood, new Vector3(transform.position.x, transform.position.y, blood.transform.position.z), Quaternion.identity);
                dead = true;
                GameObject die = Instantiate(dyingSound, transform.position, transform.rotation);
                die.GetComponent<MauroDestorySounds>().Play(audio[3]);
                die.GetComponent<MauroDestorySounds>().Play(audio[Random.Range(0,3)]);
            }
            spawnerscript.Humans.Remove(gameObject);
            Destroy(gameObject);
        }
        else if(collision.tag == "Arrow")
        {
            if (!dead)
            {
                Instantiate(soul, new Vector3(transform.position.x, transform.position.y, -11), Quaternion.identity);
                Instantiate(blood, new Vector3(transform.position.x, transform.position.y, blood.transform.position.z), Quaternion.identity);
                dead = true;
                GameObject die = Instantiate(dyingSound, transform.position, transform.rotation);
                die.GetComponent<MauroDestorySounds>().Play(audio[3]);
                die.GetComponent<MauroDestorySounds>().Play(audio[Random.Range(0, 3)]);
            }
            spawnerscript.Humans.Remove(gameObject);
            Destroy(gameObject);
        }
        else if(collision.tag == "EnemyWeapon")
        {
            if (!dead)
            {
                dead = true;
                Instantiate(blood, new Vector3(transform.position.x, transform.position.y, blood.transform.position.z), Quaternion.identity);
                GameObject die = Instantiate(dyingSound, transform.position, transform.rotation);
                die.GetComponent<MauroDestorySounds>().Play(audio[3]);
                die.GetComponent<MauroDestorySounds>().Play(audio[Random.Range(0, 3)]);
            }
            spawnerscript.Humans.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        randomDir = Vector3.Reflect(randomDir, collision.contacts[0].normal);
    }

    void WalkAnimate(bool left)
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
}

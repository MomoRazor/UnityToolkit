using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkSoulScript : MonoBehaviour
{
    public Sprite[] anim;
    public AudioClip pitArrive;

    private float lastAnimation;
    private float animationPeriod = 0.1f;

    public Vector3 pidTuningX = new Vector3(0.001f, 0.0005f, 0.002f);
    public Vector3 pidTuningY = new Vector3(0.001f, 0.0005f, 0.002f);
    GameObject currentPit;
    PID pidX, pidY;
    Rigidbody2D rb;

    private void Start()
    {
        lastAnimation = Time.time;

        pidX = new PID(pidTuningX.x, pidTuningX.y, pidTuningX.z);
        pidY = new PID(pidTuningY.x, pidTuningY.y, pidTuningY.z);
    }

    private void Update()
    {
        Animate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!rb) {
            rb = GetComponent<Rigidbody2D>();
            //rb.velocity = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
        }

        if (!currentPit) // Get Pits
        {
            GameObject[] tempPits = GameObject.FindGameObjectsWithTag("Pit");
            if (tempPits.Length > 0)
            {
                int tempSelection = 10000;
                float minDist = 100000;
                for (int i = 0; i < tempPits.Length; i++)
                {
                    float d = Vector2.Distance(this.transform.position, tempPits[i].transform.position);
                    if (d < minDist && !tempPits[i].GetComponent<MarkPit>().isFull)
                    {
                        minDist = d;
                        tempSelection = i;
                    }
                }
                if (tempSelection == 10000)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    currentPit = tempPits[tempSelection];
                    Vector2 bv = transform.position - currentPit.transform.position;
                    bv *= new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
                    bv = bv.normalized;
                    bv *= 10;
                    rb.velocity = bv;

                }
            }
        }
        else 
        {
            float velX = rb.velocity.x + pidX.Update(currentPit.transform.position.x, this.transform.position.x, Time.deltaTime);
            float velY = rb.velocity.y + pidY.Update(currentPit.transform.position.y, this.transform.position.y, Time.deltaTime);
            rb.velocity = new Vector2(velX, velY);
            //this.transform.LookAt(new Vector3(transform.position.x + rb.velocity.x, transform.position.y + rb.velocity.y, transform.position.z));
            float a = Mathf.Atan2(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x));
            a = a * Mathf.Rad2Deg;
            if (velY >= 0)
            {
                if (velX > 0)
                {
                    a = -90 + a;
                }
                else
                {
                    a = 90 - a;
                }
            }
            else
            {
                if (velX > 0)
                {
                    a = -90 - a;
                }
                else
                {
                    a = 90 + a;
                }
            }
            transform.localEulerAngles = new Vector3(0, 0, a);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pit" && !other.GetComponent<MarkPit>().isFull)
        {
            other.GetComponent<MarkPit>().IncrementSoulFragment();
            Destroy(gameObject);
        }
    }
    */

    private void OnTriggerStay2D(Collider2D other)
    {
        /*
        if (other.tag == "Pit" && !other.GetComponent<MarkPit>().isFull) {
            //other.GetComponent<MarkPit>().IncrementSoulFragment();

            if (!GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<AudioSource>().PlayOneShot(pitArrive, 0.3f);
            }
        }
        */
        if (other.tag == "Pit" && !other.GetComponent<MarkPit>().isFull)
        {
            GameObject x = new GameObject("SoulAudio");
            x.transform.position = transform.position;
            x.AddComponent<AudioSource>();
            x.GetComponent<AudioSource>().spatialBlend = 1;
            x.GetComponent<AudioSource>().PlayOneShot(pitArrive, 0.3f);
            x.AddComponent<MarkAudioDie>();

            other.GetComponent<MarkPit>().IncrementSoulFragment();
            Destroy(gameObject);
        }
        else if (other.tag == "Pit" && other.GetComponent<MarkPit>().isFull)
        {
            currentPit = null;
        }
    }

    void Animate()
    {
        if(Time.time - lastAnimation >= animationPeriod)
        {
            lastAnimation = Time.time;
            if(GetComponent<SpriteRenderer>().sprite == anim[0])
            {
                GetComponent<SpriteRenderer>().sprite = anim[1];
            }
            else if (GetComponent<SpriteRenderer>().sprite == anim[1])
            {
                GetComponent<SpriteRenderer>().sprite = anim[2];
            }
            else if (GetComponent<SpriteRenderer>().sprite == anim[2])
            {
                GetComponent<SpriteRenderer>().sprite = anim[3];
            }
            else if (GetComponent<SpriteRenderer>().sprite == anim[3])
            {
                GetComponent<SpriteRenderer>().sprite = anim[0];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = anim[0];
            }
        }
    }

}
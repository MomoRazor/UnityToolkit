using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroTurret : MonoBehaviour
{

    public AudioClip[] clip;
    public Sprite Unloaded;
    public Sprite Loaded;
    public GameObject Projectile;

    private GameObject Eco;
    private float minRange = 8f;
    private MauroSpawner spawnscript;
    private AudioSource src;

    private float fireSpeed = 30f;

    private float lastFired;
    private float firePeriod = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Eco = GameObject.FindGameObjectWithTag("eco");
        src = GetComponent<AudioSource>();
        lastFired = Time.time;
        spawnscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MauroSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = FindClosestTarget();
        if(target != null)
        {
            Vector3 dir = target.transform.position - transform.GetChild(0).position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.LookAt(target.transform.position);
        }
        Reload();
        Fire(target);
    }

    GameObject FindClosestTarget()
    {
        GameObject target= null;
        float dist;
        float closest = 99999f;
        for (int i = 0; i < spawnscript.Humans.Count; i++)
        {
            dist = Vector3.Distance(transform.position, spawnscript.Humans[i].transform.position);
            if (dist < closest)
            {
                closest = dist;
                target = spawnscript.Humans[i];
            }
        }

        for (int i = 0; i < spawnscript.Angels.Count; i++)
        {
            dist = Vector3.Distance(transform.position, spawnscript.Angels[i].transform.position);
            if (dist < closest)
            {
                closest = dist;
                target = spawnscript.Angels[i];
            }
        }

        if(closest < minRange)
        {
            return target;
        }
        else
        {
            return null;
        }
    }

    void Reload()
    {

        if (Time.time - lastFired >= firePeriod-0.5f)
        {
            if (transform.GetChild(0).childCount == 0 || transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<MauroProjectile>().fired)
            {
                GameObject proj = Instantiate(Projectile, Projectile.transform.position, transform.GetChild(0).rotation);
                proj.transform.parent = transform.GetChild(0).transform;
                proj.transform.localPosition = Projectile.transform.position;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Loaded;
            }
        }
    }

    void Fire(GameObject Target)
    {
        if(Time.time - lastFired >= firePeriod)
        {
            lastFired = Time.time;

            if (Target != null && !transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<MauroProjectile>().fired)
            {
                if(Eco.GetComponent<MarkEconomy>().amount >= 0.15)
                {
                    Eco.GetComponent<MarkEconomy>().Buy(0.15f);
                    src.PlayOneShot(clip[Random.Range(0, 2)], 0.3f);
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Unloaded;
                    Vector3 direction = Target.transform.position - transform.position;
                    direction = direction.normalized;
                    transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<MauroProjectile>().Fire(direction * fireSpeed);
                }
            }
        }
    }
}

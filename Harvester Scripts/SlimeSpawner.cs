using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{

    // Slime spawners now spawn baby slimes until they reach spawn limit
    // Baby slimes correctly update parent slime spawner on death
    // Still gotta do slime trail effect
    // Also baby slimes currently attack with melee not ranged

    public int spawnLimit = 10;
    public float spawnPeriod = 3f;
    public GameObject slime;

    private float _lastSpawn = 0f;
    private Rigidbody2D _body;
    private int currentSlimeNumber = 0;
    private bool isBirthing = false;
    private float birthDuration = 1.2f;
    private float birthTimer = 0f;
    private Animator animator;
  
    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsBirthing", false);
    }

    void Update()
    {
        if(spawnPeriod + Random.Range(0f, spawnPeriod) <= Time.timeSinceLevelLoad - _lastSpawn){
            if (currentSlimeNumber < spawnLimit && !isBirthing && currentSlimeNumber <= spawnLimit)
            {
                StartBirthing();
            }           
        }
    }

    void FixedUpdate()
    {
        if (isBirthing)
        {
            birthTimer += Time.deltaTime;

            if (birthTimer > birthDuration)
            {
                SpawnSlime();
            }
            
        }
    }


    void StartBirthing()
    {
        isBirthing = true;
        animator.SetBool("IsBirthing", true);
    }

    void SpawnSlime()
    {
        animator.SetBool("IsBirthing", false);

        birthTimer = 0f;
        isBirthing = false;       
        _lastSpawn = Time.timeSinceLevelLoad;

        GameObject newSlime = Instantiate(slime, transform.position + (Vector3.right * 1.94f), new Quaternion(0,0,0,0));
        newSlime.GetComponent<BabySlime>().SetParentSlime(this);
        currentSlimeNumber++;           
    }

    public void BabyDeathAlert()
    {
        currentSlimeNumber--;
    }
}

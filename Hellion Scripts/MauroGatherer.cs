using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroGatherer : MonoBehaviour
{

    public Sprite Loaded;
    public Sprite Unloaded;
    public GameObject Human;

    private GameObject Eco;
    private MauroSpawner spawnerscript;

    private float lastGather;
    private float gatherPeriod = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Eco = GameObject.FindGameObjectWithTag("eco");
        spawnerscript = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MauroSpawner>();
        lastGather = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateCharacter();
    }

    void GenerateCharacter()
    {
        if (Time.time - lastGather >= gatherPeriod)
        {
            if(Eco.GetComponent<MarkEconomy>().amount >= 0.2)
            {
                Eco.GetComponent<MarkEconomy>().Buy(0.2f);
                lastGather = Time.time;
                GetComponent<SpriteRenderer>().sprite = Unloaded;
                GameObject newguy = Instantiate(Human, new Vector3(transform.position.x, Mathf.Floor(transform.position.y - 1), transform.position.z), Human.transform.rotation);
                spawnerscript.Humans.Add(newguy);
            }
        }else if(Time.time - lastGather >= gatherPeriod - 1)
        {
            GetComponent<SpriteRenderer>().sprite = Loaded;
        }
    }
}

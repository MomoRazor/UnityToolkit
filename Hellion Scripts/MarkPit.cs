using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkPit : MonoBehaviour
{
    public GameObject soul;
    public Sprite FilledWell;
    public Sprite EmptyWell;
    public float soulFragments = 0;
    public float maximumFragments = 150;
    public bool isFull = false;
    public GameObject healthIndicator;

    private int healthCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        healthIndicator = GameObject.FindGameObjectWithTag("healthIndicator");
    }

    // Update is called once per frame
    void Update()
    {
        if(soulFragments == 0 && GetComponent<SpriteRenderer>().sprite != EmptyWell)
        {
            GetComponent<SpriteRenderer>().sprite = EmptyWell;
        }

        if (soulFragments >= maximumFragments) {
            isFull = true;
        }
        else{
            isFull = false;
        }
        /*
        if (Input.GetKeyDown(KeyCode.V)) {
            Instantiate(soul, new Vector3(transform.position.x + Random.Range(-10, 11), transform.position.y + Random.Range(-10, 11), -8), Quaternion.identity);
        }
        */
    }

    public void IncrementSoulFragment() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (soulFragments == 0)
        {
            transform.GetChild(0).transform.GetComponent<ParticleSystem>().Play();
            GetComponent<SpriteRenderer>().sprite = FilledWell;
        }
        if (soulFragments < maximumFragments)
        {
            healthCounter++;
            soulFragments += 1f; //To Change
            if(healthCounter >= 15)
            {
                healthCounter = 0;
                if(player != null)
                {
                    if (player.GetComponent<PlayerController>().health < player.GetComponent<PlayerController>().totalhealth)
                    {
                        player.GetComponent<PlayerController>().health++;
                        player.GetComponent<PlayerController>().HealthBar.GetComponent<MauroPlayerHealth>().UpdateHealth(player.GetComponent<PlayerController>().health, player.GetComponent<PlayerController>().totalhealth);
                    }

                }
            }
            healthIndicator.GetComponent<Text>().text = healthCounter.ToString() + "/15";
        }
        if (soulFragments > maximumFragments) {
            soulFragments = maximumFragments;
        }
    }

    public float Spend(float amount) {
        float sf = soulFragments - amount;

        if (sf < 0)
        {
            soulFragments = 0;
            return -1f * sf;
        }
        else 
        {
            soulFragments = sf;
            return 0;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDamage : MonoBehaviour
{
    GameObject player;
    bool takingDamage = false;
    float ticker = 0;
    int lastTick = 0;
    public int damage = 10;

    void Start () {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(takingDamage){
            // Debug.Log("Get Out!");
            ticker += Time.deltaTime;
            int tickerInt = Mathf.FloorToInt(ticker);
            if(tickerInt > lastTick){
                lastTick = tickerInt;
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        takingDamage = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        takingDamage = false;
    }
}

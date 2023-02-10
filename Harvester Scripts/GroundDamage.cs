using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDamage : MonoBehaviour
{
    GameObject player;
    bool takingDamage = false;
    float ticker = 0;
    int lastTick = 0;
    public float damage = 10;
    public bool isPlayer = false;

    // Update is called once per frame
    void Update()
    {
        if(takingDamage){
            // Debug.Log("Get Out!");
            ticker += Time.deltaTime;
            int tickerInt = Mathf.FloorToInt(ticker);
            if(tickerInt > lastTick){
                lastTick = tickerInt;
                if (isPlayer)
                {
                    gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                }
                else
                {
                    gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                }               
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<tile_death>())
        {
            takingDamage = true;
        }      
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<tile_death>())
        {
            takingDamage = false;
        }            
    }
}

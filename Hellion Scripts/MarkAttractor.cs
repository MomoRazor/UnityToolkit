using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAttractor : MonoBehaviour
{
    public float radiusOfAttraction = 10;

    private float referenceToActual = 10;
    private GameObject Eco;
    private float lastCost;

    public MarkPathfinder mpf;
    MarkMapGenerator mmg;
    int layer = 0;


    private void Start()
    {
        Eco = GameObject.FindGameObjectWithTag("eco");
    }

    private void Update()
    {
        RunCost();
    }

    public void SetLayer(int i) {
        layer = i;
    }

    public void RegenMPF() {
        while (!mmg)
        {
            mmg = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MarkMapGenerator>();
        }
        mpf = new MarkPathfinder(mmg.map, layer);
    }

    void RunCost()
    {
        if(Time.time - lastCost >= 1)
        {
            lastCost = Time.time;
            if (Eco.GetComponent<MarkEconomy>().amount >= 0.1f)
            {
                Eco.GetComponent<MarkEconomy>().Buy(0.1f);
                radiusOfAttraction = referenceToActual;
            }
            else
            {
                radiusOfAttraction = 0;
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkEconomy : MonoBehaviour
{
    MarkMapGenerator mmg;
    public float amount = 0;
    public Text soulIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (!mmg) {
            mmg = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MarkMapGenerator>();
        }
        amount = 0;
        float max = 0;
        for (int i = 0; i < mmg.spawnedBuildings.Count; i++) {
            if (mmg.spawnedBuildings[i].tag == "Pit") {
                amount += mmg.spawnedBuildings[i].GetComponent<MarkPit>().soulFragments;
                max += mmg.spawnedBuildings[i].GetComponent<MarkPit>().maximumFragments;
            }
        }
        amount = (float) System.Math.Round(amount, 2);

        soulIndicator.text = amount.ToString() + "/" + max.ToString();
        //Debug.Log(amount);
    }

    public void Buy(float a) {
        if (a <= amount) {
            for (int i = 0; i < mmg.spawnedBuildings.Count; i++)
            {
                if (mmg.spawnedBuildings[i].tag == "Pit")
                {
                    a = mmg.spawnedBuildings[i].GetComponent<MarkPit>().Spend(a);
                    if (a == 0) {
                        return;
                    }
                }
            }
        }
    }
}

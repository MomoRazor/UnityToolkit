using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauroPlayerHealth : MonoBehaviour
{

    public GameObject fullhead;
    public GameObject emptyhead;

    public void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.transform.localScale.x != 1)
            {
                transform.GetChild(i).gameObject.transform.localScale = Vector3.one;
            }
        }
    }

    public void UpdateHealth(int currhealth, int totalhealth)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int j = 0; j < currhealth; j++)
        {
            GameObject hey = Instantiate(fullhead, new Vector3(fullhead.transform.position.x + (j * 50f), fullhead.transform.position.y, 0), fullhead.transform.rotation);
            hey.transform.SetParent(transform);
            hey.transform.localPosition = new Vector3(fullhead.transform.position.x + (j * 50f), fullhead.transform.position.y, 0);
        }

        for(int m = currhealth; m < totalhealth; m++)
        {
            GameObject hey = Instantiate(emptyhead, new Vector3(emptyhead.transform.position.x + (m * 50f), emptyhead.transform.position.y, 0), emptyhead.transform.rotation);
            hey.transform.SetParent(transform);
            hey.transform.localPosition = new Vector3(fullhead.transform.position.x + (m * 50f), fullhead.transform.position.y, 0);
        }
    }
}

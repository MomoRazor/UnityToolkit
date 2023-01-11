using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int score = GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>().getLastScore();
        this.GetComponent<Text>().text = "" + score;
    }
}

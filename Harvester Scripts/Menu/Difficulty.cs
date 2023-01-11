using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public GameObject difficultyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChange () {
        difficultyText.GetComponent<Text>().text = "Difficulty (" + this.GetComponent<Slider>().value + ")";
    }
}

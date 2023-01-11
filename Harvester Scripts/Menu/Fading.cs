using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    public GameObject[] imageObjects;
    int selection = 0;

    float maxScale = 3f;
    float timeLimit = 5f;

    float currentTime = 0f;

    // Update is called once per frame
    void Update()
    {
        Color currentColor = imageObjects[selection].GetComponent<Image>().color;
        RectTransform currentRect = imageObjects[selection].GetComponent<RectTransform>();

        if(currentTime >= timeLimit) {
            imageObjects[selection].GetComponent<Image>().color = new Color(currentColor.r,currentColor.g,currentColor.b,0);
            currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,0);
            imageObjects[selection].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            currentTime = 0f;

            int tempSelection = 0;

            do {
                tempSelection = Random.Range(0, imageObjects.Length);
            } while (tempSelection == selection);
            
            selection = tempSelection;
        }

        float scale = Mathf.Lerp(currentRect.localScale.x, maxScale, Time.deltaTime / timeLimit);
        imageObjects[selection].GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);

        float currentOpacity = currentColor.a;
        float opacity = Mathf.Lerp(currentOpacity, 1, Time.deltaTime / timeLimit);
        imageObjects[selection].GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);

        currentTime += Time.deltaTime;
    }
}

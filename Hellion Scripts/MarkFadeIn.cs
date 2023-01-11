using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkFadeIn : MonoBehaviour
{
    Image x;
    // Start is called before the first frame update
    void Start()
    {
        x = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (int i = 100; i > -1; i-=5) {
            Color y = x.color;
            y.a = (float)i / 100f;
            x.color = y;
            yield return new WaitForSeconds(.01f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarkMMFadeIntoGame : MonoBehaviour
{
    Image x;
    bool runOnce = true;
    public GameObject bg1, bg2, bg3, bg4;

    private void Start()
    {

        bg3.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && runOnce)
        {
            runOnce = false;
            x = GetComponent<Image>();
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        for (int i = 0; i < 110; i += 5)
        {
            Color y = x.color;
            y.a = (float)i / 100f;
            x.color = y;
            yield return new WaitForSeconds(.01f);
        }
        //SceneManager.LoadScene(1);
        bg1.SetActive(false);
        bg2.SetActive(false);
        bg4.SetActive(false);
        bg3.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (int i = 100; i > -1; i -= 5)
        {
            Color y = x.color;
            y.a = (float)i / 100f;
            x.color = y;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        for (int i = 0; i < 110; i += 5)
        {
            Color y = x.color;
            y.a = (float)i / 100f;
            x.color = y;
            yield return new WaitForSeconds(.01f);
        }
        SceneManager.LoadScene(1);
    }
}

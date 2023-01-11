using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarkMMFadeIntoGameDie : MonoBehaviour
{
    Image x;
    bool runOnce = true;

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
        SceneManager.LoadScene(1);
    }
}

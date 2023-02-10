using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momentum : MonoBehaviour
{
    float momentum = 0f;
    game_controller gameController;
    ScoreKeeper scoreKeeper;

    public float maxDamageMultiplier = 2f;
    public float maxMomentum = 1000f;
    public float momentumLossRate = 2f;

    public GameObject momentumBar;

    float timer = 0f;
    int killStreak = 1;

    void Start()
    {
        gameController = gameObject.GetComponent<game_controller>();
        scoreKeeper = gameObject.GetComponent<ScoreKeeper>();
        
        UpdateMomentum();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            DepleteMomentum(momentumLossRate);
        }
    }

    public float GetMomentum()
    {
        return momentum;
    }

    public void AddMomentum(float addedMomentum)
    {
        momentum += addedMomentum + (scoreKeeper.getKs() / 2);
        UpdateMomentum();     
    }

    public void DepleteMomentum(float depletedMomentum)
    {
        momentum -= depletedMomentum;
        UpdateMomentum();
    }

    void UpdateMomentum()
    {
        if (momentum < 0)
        {
            momentum = 0;
        }
        else if (momentum > maxMomentum)
        {
            momentum = maxMomentum;
        }
        gameController.SetDamageMultiplier(((momentum / maxMomentum) * ((maxDamageMultiplier) - 1)) + 1);

        momentumBar.transform.localScale = new Vector3(momentum / maxMomentum, 1, 1);
    }
}

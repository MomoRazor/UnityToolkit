using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momentum : MonoBehaviour
{
    float momentum = 0f;
    game_controller gameController;
    public float maxDamageMultiplier = 2f;
    public float maxMomentum = 1000f;
    public float momentumLossRate = 2f;
    private float timer = 0f;
    private int killStreak = 1;

    void Start()
    {
        gameController = GameObject.Find("game_controller").GetComponent<game_controller>();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            AddMomentum(momentumLossRate * -1);
        }
    }

    public float GetMomentum()
    {
        return momentum;
    }

    void AddMomentum(float addedMomentum)
    {
        momentum += addedMomentum + (killStreak / 2);
        if (momentum < 0)
        {
            momentum = 0;
        }
        else if (momentum > maxMomentum)
        {
            momentum = maxMomentum;
        }
        gameController.SetDamageMultiplier(((momentum / maxMomentum) * ((maxDamageMultiplier) - 1)) + 1);
    }

    public void ResetKillStreak(){
        killStreak = 0;
    }

    public void AddToKillStreak(int multiplier = 1)
    {
        killStreak += 1 * multiplier;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;
    public Text ksText;
    int score = 0;
    int ks = 0;

    Momentum momentumBar;

    Persistor persistor;

    void Start () {
        persistor = GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>();
        momentumBar = gameObject.GetComponent<Momentum>();
    }

    public int getKs()
    {
        return ks;
    }

    public void addScore(string type) {
        ks+=1;
        float momentum = 5f;

        switch(type){
            case "melee":
                score += 1 * ks;
                momentum += 1;
                break;
            case "mg":
                score += 3 * ks;
                momentum += 3;
                break;
            case "burst":
                score += 5 * ks;
                momentum += 5;
                break;
            case "sg":
                score += 10 * ks;
                momentum += 10;
                break;
            case "gat":
                score += 30 * ks;
                momentum += 30;
                break;
            case "slimeSmall":
                score += 5 * ks;
                momentum += 5;
                break;
            case "slime":
                score += 30 * ks;
                momentum += 30;
                break;
            case "boss":
                score *= 2;
                break;
            default:
                break;
        }

        persistor.setLastScore(score);

        scoreText.text = "Score: " + score;
        ksText.text = "KillStreak: " + ks;

        momentumBar.AddMomentum(momentum);
    }

    public void revertKs () {
        ks = 0;
        ksText.text = "KillStreak: " + ks;

        momentumBar.DepleteMomentum(50);
    }   
}

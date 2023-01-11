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

    Persistor persistor;

    void Start () {
        persistor = GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>();
    }

    public void addScore(string type) {
        ks+=1;

        switch(type){
            case "melee":
                score += 1 * ks;
                break;
            case "mg":
                score += 3 * ks;
                break;
            case "burst":
                score += 5 * ks;
                break;
            case "sg":
                score += 10 * ks;
                break;
            case "gat":
                score += 30 * ks;
                break;
            case "slimeSmall":
                score += 5 * ks;
                break;
            case "slime":
                score += 30 * ks;
                break;
            case "boss":
                score *= 2;
                break;
            default:
                break;
        }

        persistor.setLastScore(score);

        scoreText.text = "Score: " + score;
        ksText.text = "KS: " + ks;
    }

    public void revertKs () {
        ks = 0;
        ksText.text = "KS: " + ks;
    }
}

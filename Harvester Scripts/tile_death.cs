using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_death : MonoBehaviour
{
    public float fadeSpeed = 1f;
    bool isDead = false;
    SpriteRenderer spriteRenderer;
    float opacity = 1;
    
    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update () {
        // Set Damage Trigger
        if(isDead){
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
            opacity = Mathf.Lerp(opacity, 0, Time.deltaTime / fadeSpeed);
            this.GetComponent<BoxCollider2D>().enabled = true;
        } else {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
            opacity = Mathf.Lerp(opacity, 1, Time.deltaTime / fadeSpeed);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void Kill () {
        isDead = true;
    }

    public void Revive () {
        isDead = false;
    }
}

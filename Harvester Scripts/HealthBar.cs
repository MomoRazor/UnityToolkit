using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBarEmpty;
    public GameObject healthBarOverlay;
    
    public void DisplayHealthBar()
    {        
        healthBarEmpty.GetComponent<SpriteRenderer>().enabled = true;
        healthBarOverlay.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideHealthBar()
    {
        healthBarEmpty.GetComponent<SpriteRenderer>().enabled = false;
        healthBarOverlay.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBarOverlay.transform.localScale = new Vector3 (healthPercentage, healthBarOverlay.transform.localScale.y, healthBarOverlay.transform.localScale.z);
    }
}

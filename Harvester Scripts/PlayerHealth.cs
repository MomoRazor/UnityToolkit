using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //TODO add dying by bad tile
    public GameObject healthBarPrefab;
    HealthBar healthBar;

    // private SpriteRenderer hurtRenderer; 
    public float maxHealth = 100f;
    float currentHealth;
    float regenRate = 2f;
    float regenTimer = 0f;
    float healthBarDisplayDuration = 5f;
    float healthBarDisplayTimer = 0f;
    public GameObject gC;
    public GameObject persistor;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = healthBarPrefab.GetComponent<HealthBar>();
        healthBar.HideHealthBar();
        gC = GameObject.FindGameObjectWithTag("GameController");
        persistor = GameObject.FindGameObjectWithTag("Persistor");
        // hurtRenderer = gameObject.transform.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenRate)
        {
            Regen();
            regenTimer = 0f;
        }

        healthBarDisplayTimer += Time.deltaTime;
        if (healthBarDisplayTimer >= healthBarDisplayDuration)
        {
            healthBarDisplayTimer = 0f;
            healthBar.HideHealthBar();
        }
    }

    void Regen()
    {
        // hurtRenderer.enabled = false;
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        // hurtRenderer.enabled = true;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            die();
            // game over
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        healthBar.DisplayHealthBar();
        healthBarDisplayTimer = 0f;

        gC.GetComponent<ScoreKeeper>().revertKs();
    }

    void die(){
        persistor.GetComponent<Persistor>().lose();
        SceneManager.LoadScene(3);
    }

   

    
}

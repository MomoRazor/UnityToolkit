using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //TODO add dying by bad tile
    public GameObject healthBarPrefab;
    HealthBar healthBar;

    public GameObject skeleton;
    private SpriteRenderer hurtRenderer;
    public AudioClip hurtSound;

    public float maxHealth = 50f;
    public float damageInvincibilityTime = 0.3f;
    float currentHealth;
    public float secondsToRegen = 5f;
    float regenTimer = 0f;
    float healthBarDisplayDuration = 5f;
    float healthBarDisplayTimer = 0f;
    float invincibilityTimer = 0f;
    public GameObject gC;
    public GameObject persistor;
    CapsuleCollider2D collider;
    Dodge dodge;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = healthBarPrefab.GetComponent<HealthBar>();
        healthBar.HideHealthBar();
        gC = GameObject.FindGameObjectWithTag("GameController");
        persistor = GameObject.FindGameObjectWithTag("Persistor");
        collider = gameObject.GetComponent<CapsuleCollider2D>();

        hurtRenderer = skeleton.GetComponent<SpriteRenderer>();
        dodge = gameObject.GetComponent<Dodge>();
    }

    void Update()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= secondsToRegen)
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

        if (collider.enabled == false && !dodge.isDodging)
        {
            invincibilityTimer -= Time.deltaTime;
            hurtRenderer.enabled = true;
            if (invincibilityTimer >= 0.1f && invincibilityTimer <= 0.2f)
            {
                hurtRenderer.enabled = false;
            }
            if (invincibilityTimer <= 0)
            {
                collider.enabled = true;
                hurtRenderer.enabled = false;
            }
        }
        else if (collider.enabled == true)
        {
            if (hurtRenderer.enabled == true)
            {
                hurtRenderer.enabled = false;
            }
        }
    }

    void Regen()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        gameObject.GetComponent<AudioSource>().volume = 0.15f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(hurtSound);

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

        collider.enabled = false;
        invincibilityTimer = damageInvincibilityTime;
    }

    void die(){
        persistor.GetComponent<Persistor>().lose();
        SceneManager.LoadScene(3);
    }

   

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public GameObject healthBarPrefab;
    HealthBar healthBar;

    death_zone dZ;
    ScoreKeeper sK;
    GameObject gC;

    public float maxHealth = 100;
    float currentHealth;
    float healthBarDisplayDuration = 5f;
    float healthBarDisplayTimer = 0f;
    private BabySlime _slime;
    public string enemyType;
    Persistor persistor;

    private bool hasDied = false;
    private float deathDuration = 0.25f;
    private float deathTimer = 0f;
    Vector3 defaultScale;
    Vector3 deathPosition;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = healthBarPrefab.GetComponent<HealthBar>();
        healthBar.HideHealthBar();
        _slime = GetComponent<BabySlime>();
        dZ = GameObject.FindGameObjectWithTag("DeathZoneManager").GetComponent<death_zone>();
        gC = GameObject.FindGameObjectWithTag("GameController");
        sK = gC.GetComponent<ScoreKeeper>();
        persistor = GameObject.FindGameObjectWithTag("Persistor").GetComponent<Persistor>();
        
        deathTimer = deathDuration;
        defaultScale = transform.localScale;
    }

    void Update()
    {
        healthBarDisplayTimer += Time.deltaTime;
        if (healthBarDisplayTimer >= healthBarDisplayDuration)
        {
            healthBarDisplayTimer = 0f;
            healthBar.HideHealthBar();
        }

        if (hasDied)
        {
            deathTimer -= Time.deltaTime;
            healthBar.HideHealthBar();

            transform.localScale = new Vector3(defaultScale.x, defaultScale.y * deathTimer / deathDuration, defaultScale.z);
            transform.position = new Vector3(deathPosition.x, (deathPosition.y - 1) + transform.localScale.y / defaultScale.y, deathPosition.z);

            if (deathTimer <= 0)
            {
                FinishDying();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;           
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        healthBar.DisplayHealthBar();
        healthBarDisplayTimer = 0f;
        if (currentHealth <= 0)
        {
            StartDying();
        }
        
    }

    void StartDying()
    {
        if (!hasDied)
        {
            hasDied = true;
            deathPosition = transform.position;
        }
    }

    void FinishDying()
    {
        if (_slime)
        {
            _slime.alertParentOnDeath();
        }

        dZ.PushBackRadius(0.3f);
        sK.addScore(enemyType);

        if (enemyType == "boss")
        {
            persistor.win();
            SceneManager.LoadScene(4);                       
        }

        Destroy(gameObject);
    }
}

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
    }

    void FixedUpdate()
    {
        healthBarDisplayTimer += Time.deltaTime;
        if (healthBarDisplayTimer >= healthBarDisplayDuration)
        {
            healthBarDisplayTimer = 0f;
            healthBar.HideHealthBar();
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
            die();
        }
        
    }

    void die(){       
        if(_slime)
        {
            _slime.alertParentOnDeath();
        }

        dZ.PushBackRadius(0.3f);
        sK.addScore(enemyType);

        // TODO: add to momentum bar

        if(enemyType == "boss"){
            persistor.win();
            SceneManager.LoadScene(4);
        }


        Destroy(gameObject);
    }

   

    
}

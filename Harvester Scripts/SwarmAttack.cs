using UnityEngine;

public class SwarmAttack : MonoBehaviour
{
    public float damage = 4f;
    public float attackTimeout = 0.5f;
    float attackTimer = 0f;

    private bool _isAttacking;

    void Start()
    {
        attackTimer = attackTimeout;
    }


    public bool getIsAttacking(){
        return _isAttacking;
    }

    void FixedUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackTimeout)
        {
            _isAttacking = false;
        }
    }

    void OnTriggerStay2D (Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !_isAttacking)
        {
            InitiateAttack();
        }
    }

    void InitiateAttack()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().TakeDamage((int)damage);
        _isAttacking = true;
        attackTimer = 0f;
    }
}

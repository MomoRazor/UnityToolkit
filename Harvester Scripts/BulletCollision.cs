using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D collider)
    {
        GameObject collidingGameObject = collider.gameObject;
        
        if(gameObject.tag == "Player" && collidingGameObject.tag == "EnemyBullet"){
            CapsuleCollider2D capsule = gameObject.GetComponent<CapsuleCollider2D>();
            if (capsule.enabled)
            {
                float damage = collidingGameObject.GetComponent<Bullet>().getDamage();
                gameObject.GetComponent<PlayerHealth>().TakeDamage((int) damage);
                Destroy(collidingGameObject);   
            }

        }
        else if(gameObject.tag == "Enemy" && collidingGameObject.tag == "PlayerBullet"){
            float damage = collidingGameObject.GetComponent<Bullet>().getDamage();
            gameObject.GetComponent<EnemyHealth>().TakeDamage((int) damage);
            Destroy(collidingGameObject);
        }
    }
}
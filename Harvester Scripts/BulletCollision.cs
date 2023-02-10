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
            game_controller game = GameObject.FindWithTag("GameController").GetComponent<game_controller>();
            game.PlayHitSound();

            float damage = collidingGameObject.GetComponent<Bullet>().getDamage() * game.GetDamageMultiplier();
            gameObject.GetComponent<EnemyHealth>().TakeDamage((int)damage);
            Destroy(collidingGameObject);
        }
    }
}
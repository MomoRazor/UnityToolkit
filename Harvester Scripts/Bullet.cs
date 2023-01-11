using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;

    private float _damage = 1f;
    private float _range;
    private Vector3 _startPosition;
    private Vector3 _direction;

    public void setDirection(Vector3 direction) {
        _direction = direction;
    }
    public void setRange(float range){
        _range = range;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;  
    }

    void Update()
    {
        float distanceTravelled = Vector2.Distance(transform.position, _startPosition);

        if(distanceTravelled >= _range){
            bulletEnd();
        }else{
            transform.Translate(_direction * Time.deltaTime * speed);
        }
    }

    void bulletEnd(){
        Destroy(gameObject);
    }

    public float getDamage(){
        return _damage;
    }

    public void setDamage(float damage){
        _damage = damage;
    }
}
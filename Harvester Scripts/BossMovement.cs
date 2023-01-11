using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public AudioClip BossEntrance;
    public float speed = 3f;
    
    private GameObject _player;
    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
        this.GetComponent<AudioSource>().PlayOneShot(BossEntrance);
    }

    void Update()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(_player.transform.position.x,_player.transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));
        _body.velocity = direction * speed;
        if (_body.velocity.x < -0.1f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_body.velocity.x > 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
    }
}

using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float speedMultiplier = 10f;

    private Vector2 _inputVelocity;
    private Rigidbody2D _body;

    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _inputVelocity = Vector2.ClampMagnitude(_inputVelocity, 1.0f);

    }

    void FixedUpdate()
    {
        _body.velocity = _inputVelocity * Time.deltaTime * speedMultiplier;     
    }
}

using UnityEngine;

public class BossGunControl : GunControl
{
    private GameObject _player;
    private Gun _gun;
    private SpriteRenderer _gunRenderer;

    void Start()
    {
        _gun = gameObject.GetComponentInChildren<Gun>();
        _gunRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update(){
        Vector3 difference = transform.position - GameObject.FindWithTag("Player").transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(rotationZ < -90 || rotationZ > 90){
            _gunRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180+rotationZ);
        }else{
            _gunRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }

        _gun.fire();

        // float zValue;
        // if ((target.transform.position.y - player.transform.position.y) > 0)
        // {
        //     zValue = 1;
        // }
        // else{
        //     zValue = -1;
        // }
        // transform.position = new Vector3(transform.position.x, transform.position.y, zValue);
    }

    public override GameObject getTarget()
    {
        return GameObject.FindWithTag("Player");
    }
}

using UnityEngine;

public class EnemyGunControl : GunControl
{
    private Gun _gun;
    private float _range;
    private GameObject _player;

    void Start() {
        _gun = gameObject.GetComponentInChildren<Gun>();
        _range = _gun.range;
    }

    void Update(){
        float distance = Vector2.Distance(gameObject.transform.position, GameObject.FindWithTag("Player").transform.position);
        if(_range >= distance){
            _gun.fire();
        }
    }

    public override GameObject getTarget()
    {
        return GameObject.FindWithTag("Player");
    }
}

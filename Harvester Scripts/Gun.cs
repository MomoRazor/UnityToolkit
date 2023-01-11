using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public float damage = 1f;
    public GameObject Projectile;
    public float firePeriodInSeconds = 1f;
    public float range;
    public bool hideGun;
    public bool hasCooldown = false;
    public float cooldownPeriod = 0.1f;
    public int heatLimit = 100;


    public bool limitedAmmo = false;
    public int ammo = 100;
    public float bulletStartOffset = 0.75f;

    private int currentHeat = 0;
    private GameObject _target;
    private float _lastFired = 0f;
    private bool _heated = false;

    void Start()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if(hideGun && renderer){
            renderer.enabled = false;
        }
    }

    void Update(){
        if(hasCooldown){
            if(Time.timeSinceLevelLoad - _lastFired >= cooldownPeriod){
                _lastFired = Time.timeSinceLevelLoad;
                if(currentHeat > 0){
                    currentHeat--;
                }else {
                    _heated = false;
                }
            }
        }
    }

    bool checkCooldown(){
        return !hasCooldown || (hasCooldown && !_heated);
    }

    bool checkRateOfFire(){
        return Time.timeSinceLevelLoad - _lastFired >= firePeriodInSeconds;
    }

    bool checkAmmo(){
        return !limitedAmmo || (limitedAmmo && ammo > 0);
    }

    public bool checkIfCanFire(){
        if(checkRateOfFire() && checkAmmo() && checkCooldown()){
            _lastFired = Time.timeSinceLevelLoad;
            if(hasCooldown){
                currentHeat++;
            }
            if(currentHeat >= heatLimit){
                _heated = true;
            }
            if(limitedAmmo){
                ammo--;
            }
            return true;
        }else{
            return false;
        }
    }

    public GameObject getTarget(){
        return gameObject.transform.parent.gameObject.GetComponent<GunControl>().getTarget();
    }
    
    public float getLastFired(){
        return _lastFired;
    }

    public abstract void fire();
}
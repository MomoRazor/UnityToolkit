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

    private GameObject _target;
    public int currentHeat = 0;  
    public float _lastFired = 0f;
    public bool _heated = false;

    public bool isFiring = false;
    public bool isShootingBlanks = false;

    SpriteRenderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        if(hideGun && renderer){
            renderer.enabled = false;
        }
    }

    void Update(){
        
        if (isFiring)
        {
            isFiring = false;
        }
        if (isShootingBlanks)
        {
            isShootingBlanks = false;
        }

        // heat coloring
        if (hasCooldown)
        {
            float heatColorLevel = ((float)currentHeat / (float)heatLimit) * -0.5f + 1;
            renderer.color = new Color(1, heatColorLevel, heatColorLevel);
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
        if (checkRateOfFire() && checkAmmo() && !checkCooldown()){
            isShootingBlanks = true;
            isFiring = false;
        }
        
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
            isFiring = true;
            isShootingBlanks = false;
            return true;
        }else{
            isFiring = false;
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
using UnityEngine;
using System.Collections;

public class PlayerGunControl : GunControl
{
    AudioSource audio;
    public AudioClip machinegunSound;
    public AudioClip shotgunSound;
    public AudioClip blankSound;

    public GameObject target;
    private int _selectedGun = 0;
    GameObject player;
    private bool isRecoiling = false;
    private bool canRecoil = true;
    private float recoilTimer = 0f;

    public GameObject minigunObject;
    private MachineGun minigun;   

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        audio = gameObject.GetComponent<AudioSource>();

        minigun = minigunObject.GetComponent<MachineGun>();
    }
    
    void Update(){
        Transform current = gameObject.transform.GetChild(_selectedGun);
        SpriteRenderer currentRenderer = current.GetComponent<SpriteRenderer>();
        GameObject muzzle = current.GetChild(0).gameObject;

        Vector3 difference = transform.position - target.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(rotationZ < -90 || rotationZ > 90){
            transform.localScale = new Vector3(-1, 1, 1);
            //currentRenderer.flipX = false;
            //muzzleRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180+rotationZ);
        }else{
            transform.localScale = new Vector3(1, 1, 1);
            //currentRenderer.flipX = true;
            //muzzleRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }


        if (Input.GetMouseButton(0)){
            current.GetComponent<Gun>().fire();
            
            // recoil
            if (transform.GetChild(_selectedGun).gameObject.GetComponent<Gun>().isFiring)
            {
                if (_selectedGun == 1)
                {
                    audio.volume = 0.15f;
                    audio.PlayOneShot(shotgunSound);
                }
                else
                {
                    audio.volume = 0.15f;
                    audio.PlayOneShot(machinegunSound);
                }

                if (canRecoil)
                {
                    isRecoiling = true;
                    canRecoil = false;
                    recoilTimer = 0.05f;
                    gameObject.transform.position += (difference / 200f);
                    muzzle.SetActive(true);
                }
            }
            else if (transform.GetChild(_selectedGun).gameObject.GetComponent<Gun>().isShootingBlanks)
            {
                audio.volume = 0.5f;
                audio.PlayOneShot(blankSound);
            }
        }

        if (isRecoiling)
        {
            if (recoilTimer > 0)
            {
                recoilTimer -= Time.deltaTime;             
            }
            else
            {
                isRecoiling = false;             
                recoilTimer = 0.05f;
                gameObject.transform.localPosition = new Vector3(0f, -0.4f, 0f);
                muzzle.SetActive(false);
            }
        }

        if (!canRecoil && !isRecoiling)
        {
            if (recoilTimer > 0)
            {
                recoilTimer -= Time.deltaTime;
            }
            else
            {
                canRecoil = true;
                recoilTimer = 0.05f;
            }
           
        }


        if (Input.GetKeyDown(KeyCode.E)){
            current.gameObject.SetActive(false);
            if(_selectedGun < gameObject.transform.childCount-1){
                _selectedGun++;
            }else {
                _selectedGun = 0;
            }
            gameObject.transform.GetChild(_selectedGun).gameObject.SetActive(true);
        }else if(Input.GetKeyDown(KeyCode.Q)){
            current.gameObject.SetActive(false);
            if(_selectedGun > 0){
                _selectedGun--;
            }else {
                _selectedGun = gameObject.transform.childCount-1;
            }
            gameObject.transform.GetChild(_selectedGun).gameObject.SetActive(true);
        }

        float zValue;
        if ((target.transform.position.y - player.transform.position.y) > 0)
        {
            zValue = 3;
        }
        else{
            zValue = -3;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, zValue);

        // minigun cooldown fix
        if (minigun.hasCooldown)
        {
            if (Time.timeSinceLevelLoad - minigun._lastFired >= minigun.cooldownPeriod)
            {
                minigun._lastFired = Time.timeSinceLevelLoad;
                if (minigun.currentHeat > 0)
                {
                    minigun.currentHeat--;
                }
                else
                {
                    minigun._heated = false;
                }
            }
        }
    }

    public override GameObject getTarget()
    {
        return target;
    }

    private IEnumerator RecoilCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localPosition = new Vector3(0f, -0.4f, 0f);
    }
}

using UnityEngine;

public class PlayerGunControl : GunControl
{
    public GameObject target;
    private int _selectedGun = 0;
    // GameObject player;

    // void Start()
    // {
    //     player = GameObject.FindWithTag("Player");
    // }
    
    void Update(){
        Transform current = gameObject.transform.GetChild(_selectedGun);
        SpriteRenderer currentRenderer = current.GetComponent<SpriteRenderer>();

        Vector3 difference = transform.position - target.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if(rotationZ < -90 || rotationZ > 90){
            currentRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180+rotationZ);
        }else{
            currentRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }

        
        if(Input.GetMouseButton(0)){
            current.GetComponent<Gun>().fire();
        }
        
        if(Input.GetKeyDown(KeyCode.E)){
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
        return target;
    }
}

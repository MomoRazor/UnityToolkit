using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private MachineGun _gun;

    void Start() {
        _gun = gameObject.GetComponent<MachineGun>();
    }

    void Update(){
        if(Input.GetMouseButton(0)){
            _gun.fire();
        }
    }
}

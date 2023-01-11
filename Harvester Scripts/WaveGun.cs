using UnityEngine;
using System.Collections;

public class WaveGun : Gun
{
    public float spread = 20f;
    public int repeat = 15;

    private float _currentDegree = 0f;
    private bool _down = true;
    private int _step = 3;

    public override void fire()
    {

        if(base.checkIfCanFire()){
            IEnumerator coroutine = FireWave();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator FireWave()
    {
        for(int i = 0; i < repeat; i++){
            Vector3 direction = Vector3.Normalize(new Vector3(getTarget().transform.position.x,getTarget().transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));

            Vector3 axis = Vector3.Cross(direction, Vector3.up);
            GameObject bullet = Instantiate(Projectile, transform.position + (direction * bulletStartOffset), new Quaternion(0,0,0,0));
            
            Quaternion rotation = Quaternion.AngleAxis(_currentDegree,axis);
            Vector3 newDirection = rotation * direction;
            
            bullet.GetComponent<Bullet>().setRange(range);
            bullet.GetComponent<Bullet>().setDirection(newDirection);
            bullet.GetComponent<Bullet>().setDamage(damage);

            if(_down){
                if(_currentDegree <= -spread){
                    _down = false;
                    _currentDegree = _currentDegree + _step;
                }else{
                    _currentDegree = _currentDegree - _step;
                }
            }else{
                if(_currentDegree >= spread){
                    _down = true;
                    _currentDegree = _currentDegree - _step;
                }else{
                    _currentDegree = _currentDegree + _step;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
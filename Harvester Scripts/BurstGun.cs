using UnityEngine;
using System.Collections;
 
public class BurstGun : Gun
{
    public int repeat = 5;

    public override void fire()
    {
        if(base.checkIfCanFire()){
            IEnumerator coroutine = FireBurst();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator FireBurst()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(getTarget().transform.position.x,getTarget().transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));

        for(int i = 0; i < repeat; i++){
            GameObject bullet = Instantiate(Projectile, transform.position + (direction * bulletStartOffset), new Quaternion(0,0,0,0));
            bullet.GetComponent<Bullet>().setRange(range);
            bullet.GetComponent<Bullet>().setDirection(direction);
            bullet.GetComponent<Bullet>().setDamage(damage);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
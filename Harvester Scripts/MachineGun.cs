using UnityEngine;

public class MachineGun : Gun
{
    public override void fire()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(getTarget().transform.position.x,getTarget().transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));
        if(base.checkIfCanFire()){
            GameObject bullet = Instantiate(Projectile, transform.position + (direction * bulletStartOffset), new Quaternion(0,0,0,0));

            bullet.GetComponent<Bullet>().setRange(range);
            bullet.GetComponent<Bullet>().setDirection(direction);
            bullet.GetComponent<Bullet>().setDamage(damage);
        }
    }
}
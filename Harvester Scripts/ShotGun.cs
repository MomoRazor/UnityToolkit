using UnityEngine;

public class ShotGun : Gun
{
    public int waveSize = 3;
    public float spread = 20f;

    public override void fire()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(getTarget().transform.position.x,getTarget().transform.position.y, 0f) - new Vector3(transform.position.x,transform.position.y, 0f));
        
        Vector3 axis = Vector3.Cross(direction, Vector3.up);
        if(base.checkIfCanFire()){
            float fullSpread = spread * 2;
            float step = fullSpread / waveSize;


            for(int i = 0; i < waveSize; i++){
                GameObject bullet = Instantiate(Projectile, transform.position + (direction * bulletStartOffset), new Quaternion(0,0,0,0));

                Quaternion rotation = Quaternion.AngleAxis(-spread+(step*i),axis);
                Vector3 newDirection = rotation * direction;
                bullet.GetComponent<Bullet>().setRange(range);
                bullet.GetComponent<Bullet>().setDirection(newDirection);
                bullet.GetComponent<Bullet>().setDamage(damage);

            }
        

        }
    }
}
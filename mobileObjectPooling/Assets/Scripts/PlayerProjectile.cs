using UnityEngine;

public class PlayerProjectile : Projectile
{

    protected override void Update()
    {
        base.Update();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            EventsManager.OnProjectileRecycle?.Invoke(objectPoolRef, this);
        }
    }
}

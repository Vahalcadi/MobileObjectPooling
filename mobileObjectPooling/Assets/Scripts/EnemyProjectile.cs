using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void Update()
    {
        base.Update();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}

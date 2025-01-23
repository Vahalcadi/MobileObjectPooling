using CustomUnityLibrary;
using System;
public static class EventsManager
{
    public static Action<ObjectPool<Projectile>, Projectile> OnProjectileRecycle => RecycleProjectiles;
    public static Action<ObjectPool<Enemy>, Enemy> OnEnemyRecycle;
    private static void RecycleProjectiles(ObjectPool<Projectile> projectilePool, Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool.RecycleObject(projectile);
    }
}

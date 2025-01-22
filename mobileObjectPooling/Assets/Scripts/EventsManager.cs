using System;
using CustomUnityLibrary;
public static class EventsManager
{
    public static Action<ObjectPool<Projectile>,Projectile> OnProjectileRecycle => RecycleProjectiles;

    private static void RecycleProjectiles(ObjectPool<Projectile> projectilePool, Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool.RecycleObject(projectile);
    }
}

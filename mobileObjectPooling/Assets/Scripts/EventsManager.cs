using CustomUnityLibrary;
using System;
public static class EventsManager
{
    public static Action<ObjectPool<Projectile>, Projectile> OnProjectileRecycle => RecycleProjectiles;
    public static Action<ObjectPool<Enemy>, Enemy> OnEnemyRecycle;
    public static Action OnPlayerKilled;
    public static Action OnBossKilled;

    private static void RecycleProjectiles(ObjectPool<Projectile> projectilePool, Projectile projectile)
    {
        projectilePool.RecycleObject(projectile);
        projectile.gameObject.SetActive(false);
    }
}

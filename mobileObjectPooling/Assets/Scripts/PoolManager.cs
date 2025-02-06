using CustomUnityLibrary;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [SerializeField] private Projectile[] projectiles;

    [HideInInspector] public ObjectPool<Projectile> projectilePool { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        projectilePool = new ObjectPool<Projectile>(projectiles);

        foreach (var projectile in projectiles)
            projectile.SetObjectPool(projectilePool);
    }
}

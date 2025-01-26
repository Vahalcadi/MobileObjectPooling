using CustomUnityLibrary;
using UnityEngine;

public class EnemyBoss : Enemy
{
    [SerializeField] private Transform projectileSpawnerRight;
    [SerializeField] private Transform projectileSpawnerLeft;

    public ObjectPool<Projectile> projectilePoolRight;
    public ObjectPool<Projectile> projectilePoolLeft;

    [SerializeField] private Projectile[] projectilesRight;
    [SerializeField] private Projectile[] projectilesLeft;

    [SerializeField] private float fireRateRightCannon;
    [SerializeField] private float fireRateLeftCannon;
    private float fireRateRightCannonTimer;
    private float fireRateLeftCannonTimer;


    public override void Start()
    {
        base.Start();

        projectilePoolRight = new ObjectPool<Projectile>(projectilesRight);
        projectilePoolLeft = new ObjectPool<Projectile>(projectilesLeft);

        foreach (var projectile in projectilesRight)
            projectile.SetObjectPool(projectilePoolRight);

        foreach (var projectile in projectilesLeft)
            projectile.SetObjectPool(projectilePoolLeft);
    }


    public override void CanShoot()
    {
        base.CanShoot();

        fireRateRightCannonTimer -= Time.deltaTime;
        fireRateLeftCannonTimer -= Time.deltaTime;

        if (fireRateRightCannonTimer < 0)
            ShootRight();

        if (fireRateLeftCannonTimer < 0)
            ShootLeft();
    }


    private void ShootRight()
    {
        Projectile projectile = projectilePoolRight.UseObject();
        projectile.gameObject.transform.position = projectileSpawnerRight.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawnerRight.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);

        fireRateRightCannonTimer = Random.Range(fireRateRightCannon / 2f, fireRateRightCannon * 1.5f);
    }

    private void ShootLeft()
    {
        Projectile projectile = projectilePoolLeft.UseObject();
        projectile.gameObject.transform.position = projectileSpawnerLeft.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawnerLeft.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);

        fireRateLeftCannonTimer = Random.Range(fireRateLeftCannon / 2f, fireRateLeftCannon * 1.5f); ;

    }

    private void OnDisable()
    {
        EventsManager.OnBossKilled?.Invoke();
    }
}

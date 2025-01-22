using CustomUnityLibrary;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Transform projectileSpawner;

    [SerializeField] private float fireRate;
    private float fireRateTimer;
    public ObjectPool<Projectile> projectilePool;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    private float dir = 1;


    [SerializeField] private float changeSpeedTime;
    private float speedTimer;

    public float Speed { get; set; }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        Speed = speed;
        projectilePool = new ObjectPool<Projectile>(projectiles);

        foreach (var projectile in projectiles)
            projectile.SetObjectPool(projectilePool);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CanShoot();
        ChangeSpeed();
    }

    public virtual void CanShoot()
    {
        fireRateTimer -= Time.deltaTime;
        if (fireRateTimer < 0)
            Shoot();
    }

    public virtual void ChangeSpeed()
    {
        rb.linearVelocityX = dir * Speed;

        speedTimer -= Time.deltaTime;

        if (speedTimer < 0)
        {
            Speed = Random.Range(speed / 2, speed * 1.5f);
            speedTimer = changeSpeedTime;
        }
    }

    public virtual void Shoot()
    {
        Projectile projectile = projectilePool.UseObject();
        projectile.gameObject.transform.position = projectileSpawner.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawner.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);
        fireRateTimer = fireRate;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            dir = -dir;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
}

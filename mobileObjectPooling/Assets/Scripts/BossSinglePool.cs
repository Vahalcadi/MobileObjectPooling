using System;
using UnityEngine;

public class BossSinglePool : MonoBehaviour
{
    public static Action OnProjectileRecycle;

    [SerializeField] private Transform[] projectileSpawners;

    [SerializeField] private float fireRate;
    private float fireRateTimer;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private float MaxHP;
    [SerializeField] private float tollerance;

    public float CurrentHP { get; private set; }

    private float dir = 1;
    private float changeDirTime = 0.3f;
    private float changeDirTimeCounter;

    private float activationTime;

    [SerializeField] private float changeSpeedTime;
    private float speedTimer;

    public float Speed { get; set; }

    private float leftBorder;
    private float rightBorder;

    private Camera cam;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {

        cam = Camera.main;

        leftBorder = (cam.transform.position.x - (cam.orthographicSize * cam.aspect)) + tollerance;
        rightBorder = (cam.transform.position.x + (cam.orthographicSize * cam.aspect)) - tollerance;

        Speed = speed;


        UIManager.Instance.SetEnemyInfo(PoolManager.instance.projectilePool.TotalObjsCount, PoolManager.instance.projectilePool.TotalObjsCount, PoolManager.instance.projectilePool.UsedObjsCount);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        activationTime -= Time.deltaTime;

        if (activationTime > 0f)
            return;

        CanShoot();
        ChangeSpeed();
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);

        /*if (CurrentHP <= 0)
            EventsManager.OnEnemyRecycle?.Invoke(enemyPoolRef, this);*/

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
        changeDirTimeCounter -= Time.deltaTime;

        if (speedTimer < 0)
        {
            Speed = UnityEngine.Random.Range(speed / 2, speed * 1.5f);
            speedTimer = changeSpeedTime;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftBorder, rightBorder), transform.position.y);

        if ((transform.position.x <= leftBorder) || (transform.position.x >= rightBorder))
        {

            if (changeDirTimeCounter < 0f)
            {
                dir = -dir;
                changeDirTimeCounter = changeDirTime;
            }
        }
    }

    public virtual void Shoot()
    {
        foreach (Transform t in projectileSpawners)
        {
            Projectile projectile = PoolManager.instance.projectilePool.UseObject();
            projectile.gameObject.transform.position = t.transform.position;
            projectile.gameObject.transform.rotation = Quaternion.Euler(t.rotation.eulerAngles);

            projectile.gameObject.SetActive(true);
            fireRateTimer = fireRate;
        }

        UIManager.Instance.EnemyUpdateUsedObjects(PoolManager.instance.projectilePool.UsedObjsCount);
    }

    public void ProjectileRecycle()
    {
        UIManager.Instance.EnemyUpdateUsedObjects(PoolManager.instance.projectilePool.UsedObjsCount);
    }

    /*public void SetObjectPool(ObjectPool<Enemy> objPool)
    {
        enemyPoolRef = objPool;
    }*/

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnEnable()
    {
        OnProjectileRecycle += ProjectileRecycle;
        activationTime = UnityEngine.Random.Range(0.5f, 1.3f);
        CurrentHP = MaxHP;
    }

    private void OnDisable()
    {
        OnProjectileRecycle -= ProjectileRecycle;
    }
}



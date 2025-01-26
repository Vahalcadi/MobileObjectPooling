using CustomUnityLibrary;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action OnProjectileRecycle;

    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Transform projectileSpawner;

    [SerializeField] private float fireRate;
    private float fireRateTimer;
    public ObjectPool<Projectile> projectilePool;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private float MaxHP;
    [SerializeField] private Vector2 tollerance;

    private float leftBorder;
    private float rightBorder;

    private float bottomBoundary;
    private float upBoundary;

    private Camera cam;
    public float CurrentHP { get; private set; }

    private bool isInvincible;
    private float invincibilityTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        leftBorder = (cam.transform.position.x - (cam.orthographicSize * cam.aspect)) + tollerance.x;
        rightBorder = (cam.transform.position.x + (cam.orthographicSize * cam.aspect)) - tollerance.y;

        bottomBoundary = cam.transform.position.y - cam.orthographicSize;
        upBoundary = cam.transform.position.y + cam.orthographicSize;

        projectilePool = new ObjectPool<Projectile>(projectiles);

        foreach (var projectile in projectiles)
            projectile.SetObjectPool(projectilePool);

        UIManager.Instance.SetPlayerInfo(projectilePool.TotalObjsCount, projectilePool.TotalObjsCount, projectilePool.UsedObjsCount);
    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        fireRateTimer -= Time.deltaTime;

        if (invincibilityTimer < 0)
            isInvincible = false;

        if (InputManager.Instance.Shoot() && fireRateTimer < 0)
            Shoot();

        rb.linearVelocity = new Vector2(InputManager.Instance.Move().x, -InputManager.Instance.Move().z) * speed;

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftBorder, rightBorder), Mathf.Clamp(transform.position.y, bottomBoundary, upBoundary));
    }

    public void Shoot()
    {
        Projectile projectile = projectilePool.UseObject();
        projectile.gameObject.transform.position = projectileSpawner.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawner.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);
        fireRateTimer = fireRate;

        UIManager.Instance.PlayerUpdateUsedObjects(projectilePool.UsedObjsCount);
    }

    public void ProjectileRecycle()
    {
        UIManager.Instance.PlayerUpdateUsedObjects(projectilePool.UsedObjsCount);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
        UIManager.Instance.UpdatePlayerHP((int)CurrentHP);
        invincibilityTimer = 0.5f;

        if (CurrentHP == 0)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        invincibilityTimer = 1f;
        CurrentHP = MaxHP;
        UIManager.Instance.UpdatePlayerHP((int)CurrentHP);
        OnProjectileRecycle += ProjectileRecycle;
    }

    private void OnDisable()
    {
        OnProjectileRecycle -= ProjectileRecycle;
        EventsManager.OnPlayerKilled?.Invoke();
    }
}

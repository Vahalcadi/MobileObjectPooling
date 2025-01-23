using CustomUnityLibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Transform projectileSpawner;

    [SerializeField] private float fireRate;
    private float fireRateTimer;
    public ObjectPool<Projectile> projectilePool;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private float MaxHP;
    [SerializeField] private float tollerance;

    private float leftBorder;
    private float rightBorder;

    private Camera cam;
    public float CurrentHP { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        leftBorder = (cam.transform.position.x - (cam.orthographicSize * cam.aspect)) + tollerance;
        rightBorder = (cam.transform.position.x + (cam.orthographicSize * cam.aspect)) - tollerance;

        projectilePool = new ObjectPool<Projectile>(projectiles);
        CurrentHP = MaxHP;
        foreach (var projectile in projectiles)
            projectile.SetObjectPool(projectilePool);
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer -= Time.deltaTime;
        if (InputManager.Instance.Shoot() && fireRateTimer < 0)
            Shoot();

        rb.linearVelocityX = InputManager.Instance.MoveX() * speed;

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftBorder, rightBorder), transform.position.y);
    }

    public void Shoot()
    {
        Projectile projectile = projectilePool.UseObject();
        projectile.gameObject.transform.position = projectileSpawner.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawner.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);
        fireRateTimer = fireRate;

    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
}

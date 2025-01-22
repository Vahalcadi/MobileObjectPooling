using CustomUnityLibrary;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Transform projectileSpawner;

    [SerializeField] private float fireRate;
    private float fireRateTimer;
    public ObjectPool<Projectile> projectilePool;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePool = new ObjectPool<Projectile>(projectiles);
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer -= Time.deltaTime;
        if (InputManager.Instance.Shoot() && fireRateTimer < 0)
            Shoot();

        rb.linearVelocityX = InputManager.Instance.MoveX() * speed;
    }

    public void Shoot()
    {
        Projectile projectile = projectilePool.UseObject();
        projectile.gameObject.transform.position = projectileSpawner.transform.position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(projectileSpawner.rotation.eulerAngles);

        projectile.gameObject.SetActive(true);
        fireRateTimer = fireRate;

    }

    public void RecycleProjectiles(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool.RecycleObject(projectile);
    }

    private void OnEnable()
    {
        EventsManager.OnProjectileRecycle += RecycleProjectiles;
    }

    private void OnDisable()
    {
        EventsManager.OnProjectileRecycle -= RecycleProjectiles;
    }
}

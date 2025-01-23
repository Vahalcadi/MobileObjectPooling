using CustomUnityLibrary;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Collider2D cd;
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;
    [SerializeField] protected float damage;
    protected float lifetimeTimer;

    protected ObjectPool<Projectile> objectPoolRef;


    [Header("Cosine Pattern")]
    [SerializeField] protected bool isCosinusoidal;
    [SerializeField] private float frequency = 2;
    [SerializeField] private float amplitude = 2;
    [SerializeField] private float constant = 2;
    [Range(-1, 1)][SerializeField] private int direction;


    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer < 0)
            EventsManager.OnProjectileRecycle?.Invoke(objectPoolRef, this);

        if (isCosinusoidal)
        {
            float x = amplitude * Mathf.Cos((frequency * time) + constant);

            rb.linearVelocity = new Vector2(x * direction, -speed);
        }
        else
            rb.linearVelocity = transform.up * speed;
    }

    protected virtual void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
            EventsManager.OnProjectileRecycle?.Invoke(objectPoolRef, this);

        if (collision.CompareTag("DeadZone"))
            EventsManager.OnProjectileRecycle?.Invoke(objectPoolRef, this);

    }

    public void SetObjectPool(ObjectPool<Projectile> projectilePool)
    {
        objectPoolRef = projectilePool;
    }


    protected virtual void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    protected virtual void OnDisable()
    {
        time = 0;
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Collider2D cd;

    [SerializeField] private float lifetime;
    private float lifetimeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer <= Time.deltaTime)
            EventsManager.OnProjectileRecycle?.Invoke(this);

        rb.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
            EventsManager.OnProjectileRecycle?.Invoke(this);
    }


    private void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    private void OnDisable()
    {

    }
}

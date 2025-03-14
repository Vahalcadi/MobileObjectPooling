using CustomUnityLibrary;
using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ObjectPool<Enemy> enemyStraightPool;
    private ObjectPool<Enemy> enemyCosinePool;
    private ObjectPool<Enemy> enemyBossPool;

    [SerializeField] private int playerLives;

    [SerializeField] private Enemy[] enemiesStraight;
    [SerializeField] private Enemy[] enemiesCosine;
    [SerializeField] private Enemy[] enemiesBoss;

    [SerializeField] private GameObject SpawnArea;
    private Collider2D SpawnAreaCollider;

    [SerializeField] private int maxEnemiesCount;
    private int enemyCount;

    public int CurrentPlayerLives { get; set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        CurrentPlayerLives = playerLives;
        UIManager.Instance.UpdatePlayerLives(CurrentPlayerLives);

        enemyCount = 0;

        SpawnAreaCollider = SpawnArea.GetComponent<Collider2D>();

        enemyStraightPool = new ObjectPool<Enemy>(enemiesStraight);
        enemyCosinePool = new ObjectPool<Enemy>(enemiesCosine);
        enemyBossPool = new ObjectPool<Enemy>(enemiesBoss);

        foreach (var obj in enemiesStraight)
            obj.SetObjectPool(enemyStraightPool);

        foreach (var obj in enemiesCosine)
            obj.SetObjectPool(enemyCosinePool);

        foreach (var obj in enemiesBoss)
            obj.SetObjectPool(enemyBossPool);

        GenerateEnemy(enemyStraightPool);
    }
    private void GenerateEnemy(ObjectPool<Enemy> enemyPool)
    {
        Enemy enemy = enemyPool.UseObject();

        float x = Random.Range(-SpawnAreaCollider.bounds.extents.x, SpawnAreaCollider.bounds.extents.x) + SpawnArea.transform.position.x;
        float y = Random.Range(-SpawnAreaCollider.bounds.extents.y, SpawnAreaCollider.bounds.extents.y) + SpawnArea.transform.position.y;

        enemy.transform.position = new Vector2(x, y);
        enemy.transform.gameObject.SetActive(true);

        enemyCount++;
    }

    private void RecycleEnemies(ObjectPool<Enemy> enemyPool, Enemy enemy)
    {
        enemy.transform.gameObject.SetActive(false);
        enemyPool.RecycleObject(enemy);

        if (enemyCount < maxEnemiesCount)
        {
            int random = Random.Range(0, 2);

            if (random == 0)
                GenerateEnemy(enemyStraightPool);
            else
                GenerateEnemy(enemyCosinePool);
        }
        else
        {
            GenerateEnemy(enemyBossPool);
            enemyCount = 0;
        }

    }

    private void BossKilled()
    {
        CurrentPlayerLives = playerLives;
    }

    private void PlayerKilled()
    {
        
        if (CurrentPlayerLives > 0)
            StartCoroutine(RespawnPlayer());
        else
            UIManager.Instance.OpenEndGameScreen();

        CurrentPlayerLives = Mathf.Clamp(CurrentPlayerLives - 1, 0, playerLives);
        UIManager.Instance.UpdatePlayerLives(CurrentPlayerLives);
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);

        PlayerManager.Instance.Player.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        EventsManager.OnPlayerKilled += PlayerKilled;
        EventsManager.OnEnemyRecycle += RecycleEnemies;
        EventsManager.OnBossKilled += BossKilled;
    }

    private void OnDisable()
    {
        EventsManager.OnBossKilled -= BossKilled;
        EventsManager.OnPlayerKilled -= PlayerKilled;
        EventsManager.OnEnemyRecycle -= RecycleEnemies;
    }
}

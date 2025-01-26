using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject DebugPanel;
    [SerializeField] private PoolInfoUI PlayerInfo;
    [SerializeField] private PoolInfoUI EnemyInfo;
    [SerializeField] private GameObject EndGameScreen;
    [SerializeField] private TextMeshProUGUI playerHP;
    [SerializeField] private TextMeshProUGUI playerLives;

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        DebugPanel.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenCloseDebug()
    {
        DebugPanel.SetActive(!DebugPanel.activeSelf);
    }

    public void OpenEndGameScreen()
    {
        EndGameScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Close()
    {
        Application.Quit();
    }

    public void UpdatePlayerHP(int hp)
    {
        playerHP.text = $"HP: {hp}";
    }

    public void UpdatePlayerLives(int lives)
    {
        playerLives.text = $"Lives: {lives}";
    }

    public void SetPlayerInfo(int totalObjs, int poolSize, int usedObjs)
    {
        PlayerInfo.SetUpPoolInfo(totalObjs, poolSize, usedObjs);
    }

    public void SetEnemyInfo(int totalObjs, int poolSize, int usedObjs)
    {
        EnemyInfo.SetUpPoolInfo(totalObjs, poolSize, usedObjs);
    }

    public void EnemyUpdateUsedObjects(int usedObjects)
    {
        EnemyInfo.UpdateUsedObjects(usedObjects);
    }

    public void PlayerUpdateUsedObjects(int usedObjects)
    {
        PlayerInfo.UpdateUsedObjects(usedObjects);
    }
}

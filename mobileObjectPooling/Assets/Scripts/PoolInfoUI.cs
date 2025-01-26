using TMPro;
using UnityEngine;

public class PoolInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TotalObjs;
    [SerializeField] private TextMeshProUGUI PoolSize;
    [SerializeField] private TextMeshProUGUI UsedObjs;

    private void Start()
    {
    }

    public void SetUpPoolInfo(int totalObjs, int poolSize, int usedObjs)
    {
        TotalObjs.text = $"Total Objs:  {totalObjs}";
        PoolSize.text = $"Pool Size:    {poolSize}";
        UsedObjs.text = $"Used Objs:    {usedObjs}";
    }

    public void UpdateUsedObjects(int usedObjs)
    {
        UsedObjs.text = $"Used Objs:    {usedObjs}";
    }
}

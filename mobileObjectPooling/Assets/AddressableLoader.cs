using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
    public static AddressableLoader Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else Instance = this;

    }

    private void Start()
    {
    }

    public void LoadAndInstantiate(string address)
    {
        Addressables.LoadAssetAsync<GameObject>(address).Completed += OnAssetLoaded;
    }

    private void OnAssetLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(handle.Result);
        else
            Debug.LogError($"Errore nel caricamento dell'asset: {handle.DebugName}");
    }
}


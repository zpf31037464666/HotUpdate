using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    void Start()
    {
        PlayerItemData playerItemData = GameManager.instance.currentPlyaerItemData;

        Addressables.LoadAssetAsync<GameObject>(playerItemData.PrefabPath).Completed+=(e) =>
        {
            if (e.Status ==AsyncOperationStatus.Succeeded)
            {
                Instantiate(e.Result,spawnPos.position,Quaternion.identity);
            }
        };
    }


}

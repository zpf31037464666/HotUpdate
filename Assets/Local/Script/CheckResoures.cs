using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheckResoures : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      //  Addressables.LoadAssetAsync<GameObject>("UI").Completed+=OnAssetLoaded;
        Addressables.LoadAssetAsync<TextAsset>("HotUpdate.dll").Completed+=OnTextAssetLoaded;
    }

    private void OnTextAssetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Assembly assembly = Assembly.Load(handle.Result.bytes);
            Type type = assembly.GetType("Hello");
            type.GetMethod("Run").Invoke(null, null);
        }
    }

    private void OnAssetLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedAsset = handle.Result;
            Instantiate(loadedAsset);
        }
    }

}

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    //addressabel 加载对象
    public GameObject GetObject(string prefabPath)
    {
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("Prefab path is null or empty!");
            return null;
        }

        GameObject _object;
        if (!objectPool.ContainsKey(prefabPath) || objectPool[prefabPath].Count == 0)
        {
            // 如果对象池中没有对象，则实例化一个新的对象
            _object = Addressables.InstantiateAsync(prefabPath).WaitForCompletion(); // 确保同步等待加载完成
            InitializePool(prefabPath, _object);
        }
        else
        {
            _object = objectPool[prefabPath].Dequeue();
            if (_object == null)
            {
                // 如果从池中获取的对象为 null，则实例化一个新的对象
                _object = Addressables.InstantiateAsync(prefabPath).WaitForCompletion();
                InitializePool(prefabPath, _object);
            }
        }

        _object.SetActive(true);
        return _object;
    }
    public GameObject GetObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        GameObject _object;
        string prefabName = prefab.name;

        if (!objectPool.ContainsKey(prefabName) || objectPool[prefabName].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            InitializePool(prefabName, _object);
        }
        else
        {
            _object = objectPool[prefabName].Dequeue();
            if (_object == null)
            {
                _object = GameObject.Instantiate(prefab);
                InitializePool(prefabName, _object);
            }
        }

        _object.SetActive(true);
        return _object;
    }
    private void InitializePool(string name, GameObject _object)
    {
        if (pool == null)
        {
            pool = new GameObject("ObjectPool");
        }

        GameObject childPool = GameObject.Find(name + "Pool");
        if (!childPool)
        {
            childPool = new GameObject(name + "Pool");
            childPool.transform.SetParent(pool.transform);
        }
        _object.transform.SetParent(childPool.transform);
    }

    public void PushObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return;
        }

        string _name = prefab.name.Replace("(Clone)", string.Empty);

        if (!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }

        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}

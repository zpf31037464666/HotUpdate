using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            // as 强制转换类型
            instance=this as T;
        }
        else
        {
            Destroy(instance);
        }
    }

}
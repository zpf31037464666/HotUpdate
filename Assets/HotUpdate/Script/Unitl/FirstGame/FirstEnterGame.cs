using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnterGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            // 打印欢迎信息
            Debug.Log("欢迎光临!");

            TaskManager.instance.AddTask(0);
            TaskManager.instance.AddTask(1);

            // 设置为非第一次进入
            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save(); // 保存 PlayerPrefs
        }
    }
}

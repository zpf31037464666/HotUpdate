using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : PersistentSingleton<MessageManager>
{
    [SerializeField] private MessageUI messageUI;

    public void SendMeesage(string message, Action action = null)
    {
        GameObject clone=ObjectPool.Instance.GetObject(messageUI.gameObject);
        clone.transform.SetParent(transform,true);//使用世界坐标
        clone.GetComponent<MessageUI>().ShowMessage(message, action);   
    }

    //private void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        Debug.Log("Test meessage");
    //        SendMeesage("hello,world");
    //    }
    //}

}

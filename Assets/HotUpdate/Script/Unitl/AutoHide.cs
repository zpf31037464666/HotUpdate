using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHide : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Close", 1);
    }
   public void Close()
    {
        gameObject.SetActive(false);
    }
}

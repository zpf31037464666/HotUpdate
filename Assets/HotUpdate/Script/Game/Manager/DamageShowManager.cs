using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShowManager : PersistentSingleton<DamageShowManager>
{
    public DamageShow ordinaryDamageShow;
    public DamageShow redDamageShow;
    public void CreateDamage(int damage, Vector3 position)
    {
        GameObject clone = ObjectPool.Instance.GetObject(ordinaryDamageShow.gameObject);
        DamageShow damageShow = clone.GetComponent<DamageShow>();

        Vector3 randromPos = new Vector3(position.x+Random.Range(-0.5F, 0.5F), position.y+Random.Range(-0.5f, 0.5f), 0);
        damageShow.ShowDamage(damage, randromPos);
    }
    public void CreateDamage(int damage, Vector3 position,Color color)
    {
        GameObject clone = ObjectPool.Instance.GetObject(ordinaryDamageShow.gameObject);
        DamageShow damageShow = clone.GetComponent<DamageShow>();
        Vector3 randromPos = new Vector3(position.x+Random.Range(-0.5F, 0.5F), position.y+Random.Range(-0.5f, 0.5f), 0);
        damageShow.SetTextColor(color);
        damageShow.ShowDamage(damage, randromPos);
    }
    public void CreateDamage(string contect, Vector3 position)
    {
        GameObject clone = ObjectPool.Instance.GetObject(ordinaryDamageShow.gameObject);
        DamageShow damageShow = clone.GetComponent<DamageShow>();

        Vector3 randromPos = new Vector3(position.x+Random.Range(-0.5F, 0.5F), position.y+Random.Range(-0.5f, 0.5f), 0);
        damageShow.ShowDamage(contect, randromPos);
    }



    public void CreateRedDamage(int damage, Vector3 position)
    {
        GameObject clone = ObjectPool.Instance.GetObject(redDamageShow.gameObject);
        DamageShow damageShow = clone.GetComponent<DamageShow>();

        Vector3 randromPos = new Vector3(position.x+Random.Range(-0.5F, 0.5F), position.y+Random.Range(-0.5f, 0.5f), 0);
        damageShow.ShowDamage(damage, randromPos);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("Create red show");

    //        int damage = Random.Range(1, 5);

    //        CreateDamage(damage, Vector3.zero);
    //    }
    //}
}

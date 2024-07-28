using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShowManager : Singleton<DamageShowManager>
{
    public DamageShow ordinaryDamageShow;

    public void CreateDamage(int damage, Vector3 position)
    {
        GameObject clone = ObjectPool.Instance.GetObject(ordinaryDamageShow.gameObject);
        DamageShow damageShow = clone.GetComponent<DamageShow>();

        Vector3 randromPos = new Vector3(position.x+Random.Range(-0.5F, 0.5F), position.y+Random.Range(-0.5f, 0.5f), 0);
        damageShow.ShowDamage(damage, randromPos);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int damage = Random.Range(1, 5);

            CreateDamage(damage, Vector3.zero);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpinWeapon : MonoBehaviour
{
    public float radius;
    public float speed;
    public int number;

    [SerializeField] GameObject initSpinWeapon;
    [SerializeField] Transform spinWeaponGorup;



    public List<AutoRotationToPlayer> rotationObjectList;
    public List<Bullet> bulletList;

    private float currentRadius;// 存储旋转子物体的列表
    Coroutine bufferedUpdateCoroutine;

    private void Awake()
    {

        currentRadius = radius;

        AddSpinWeapon(initSpinWeapon);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddSpinWeapon(initSpinWeapon);
            AddSpinWeaponDamage(7);

        }

    }
    public void AddSpinWeaponDamage(int damage)
    {
        foreach (var bullet in bulletList)
        {
            bullet.AddBulletDamage(damage);
        }
    }

    void UpdateWeaponRange(float range)
    {
        radius=range;
        if (bufferedUpdateCoroutine !=null)
        {
            //停止协程 避免过多重复协程
            StopCoroutine(bufferedUpdateCoroutine);
        }
        bufferedUpdateCoroutine= StartCoroutine(BufferedUpdateRangeCoroutine(range));
    }
    void UpdateWeaponSpeed(float speed)
    {
        foreach (var ball in rotationObjectList)
        {
            ball.UpdateSpeed(speed);
        }
    }
    public void AddSpinWeapon(GameObject spinWeapon)
    {
        GameObject clone=   Instantiate(spinWeapon, spinWeaponGorup);
        rotationObjectList.Add(clone.GetComponent<AutoRotationToPlayer>());
        bulletList.Add(clone.GetComponent<Bullet>());
        UpdateWeaponSpeed(speed);
        UpdateWeaponRange(radius);
    }

    IEnumerator BufferedUpdateRangeCoroutine(float target)
    {
        var t = 0f;
        var previousAmount = currentRadius;
        while (t<1f)
        {
            t+=Time.deltaTime;
            currentRadius=Mathf.Lerp(previousAmount, target, t);

            foreach (var ball in rotationObjectList)
            {
                ball.UpdateRange(currentRadius);
            }

            yield return null;
        }
    }

}

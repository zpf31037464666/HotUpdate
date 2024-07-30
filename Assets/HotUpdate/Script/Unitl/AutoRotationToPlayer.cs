using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationToPlayer : MonoBehaviour
{
    public Transform target;  // 玩家角色的Transform组件
    public float rotationSpeed = 50.0f;  // 火球围绕玩家旋转的速度
    public float radius = 1.0f;  // 火球距离玩家的初始半径
    public bool isRotation=true;
    private float angle = 0f;

    void Awake()
    {
        Init();
    }
    public void Init()
    {
        target=FindObjectOfType<Player>().gameObject.transform;
        int numberOfFireballs = transform.parent.childCount;
        int myIndex = transform.GetSiblingIndex();

        Vector3 newPosition = CalculateCirclePosition(target.position, radius, numberOfFireballs, myIndex);
        transform.position = newPosition; // 设置小球的初始位置

        Debug.Log(gameObject.name+"位置"+transform.position+"第"+myIndex+"个，总共"+numberOfFireballs);
    }
    public void RestPos(Transform target)
    {
        this.target = target;
        int numberOfFireballs = transform.parent.childCount;
        int myIndex = transform.GetSiblingIndex();

        Vector3 newPosition = CalculateCirclePosition(target.position, radius, numberOfFireballs, myIndex);
        transform.position = newPosition; // 设置小球的初始位置

        Debug.Log(gameObject.name+"位置"+transform.position+"第"+myIndex+"个，总共"+numberOfFireballs);
    }
    void Update()
    {
        if (isRotation)
        {
            RotationToPayer();
        }
    }
    //围绕玩家旋转
    void RotationToPayer()
    {
        angle += rotationSpeed * Time.deltaTime;
        float angleInRadians = angle * Mathf.Deg2Rad;
        float x = target.position.x + radius * Mathf.Cos(angleInRadians);
        float y = target.position.y + radius * Mathf.Sin(angleInRadians);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void UpdatePos()
    {
        int numberOfFireballs = transform.parent.childCount;
        int myIndex = transform.GetSiblingIndex();
        Vector3 newPosition = CalculateCirclePosition(target.position, radius, numberOfFireballs, myIndex);
        transform.position = newPosition; // 设置小球的初始位置
    }
    //更新范围
    public void UpdateRange(float range)
    {
        radius = range;
        int numberOfFireballs = transform.parent.childCount;
        int myIndex = transform.GetSiblingIndex();
        Vector3 newPosition = CalculateCirclePosition(target.position, radius, numberOfFireballs, myIndex);
        transform.position = newPosition; // 设置小球的初始位置
    }
    public void UpdateSpeed(float speed)
    {
        rotationSpeed = speed;
        int numberOfFireballs = transform.parent.childCount;
        int myIndex = transform.GetSiblingIndex();
        Vector3 newPosition = CalculateCirclePosition(target.position, radius, numberOfFireballs, myIndex);
        transform.position = newPosition; // 设置小球的初始位置
    }

    // 计算小球在圆周上均匀分布的位置
    Vector3 CalculateCirclePosition(Vector3 center, float radius, int totalPoints, int currentIndex)
    {
        angle = (360.0f / totalPoints) * currentIndex;
        //Mathf.Deg2Rad 用于将角度转换为弧度。具体来说，它将角度值乘以 π/180
        float radians = angle * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(radians);
        float y = center.y + radius * Mathf.Sin(radians);

        return new Vector3(x, y, center.z);
    }
}

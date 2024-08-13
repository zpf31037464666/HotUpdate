using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacketBall : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    private float stayTime = 0f; // 停留时间
    private bool isCollidingWithWall = false; // 是否与墙壁碰撞

    private void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;

        // 如果与墙壁碰撞且停留时间超过 1 秒，则反转方向
        if (isCollidingWithWall && stayTime >= 0.1)
        {
            direction = -direction; // 反转方向
            stayTime = 0f; // 重置停留时间
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 计算反弹方向
        direction = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;

        // 设置为与墙壁碰撞
        isCollidingWithWall = true;
        stayTime = 0f; // 重置停留时间
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 增加停留时间
        stayTime += Time.deltaTime;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 退出碰撞时重置状态
        if (isCollidingWithWall)
        {
            isCollidingWithWall = false; // 不再与墙壁碰撞
            stayTime = 0f; // 重置停留时间
        }
    }
}

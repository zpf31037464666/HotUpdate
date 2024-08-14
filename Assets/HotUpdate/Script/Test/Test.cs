using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform muzzlePos; // 激光发射点
    public int maxReflections = 3; // 最大反射次数
    public float maxLength = 100f; // 激光最大长度

    protected Vector2 mousePos; // 鼠标位置
    protected Vector2 direction; // 激光方向

    private LineRenderer laser;
    public float hitInterval = 0.1f;
    public bool isCanHit = true;

    private void Awake()
    {
        laser = muzzlePos.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Filp();
        Fire();
    }

    private void Fire()
    {
        laser.enabled = true;
        laser.positionCount = 1; // 初始化 LineRenderer 的位置数量为 1
        laser.SetPosition(0, muzzlePos.position); // 设置起始位置为发射点

        int reflectionCount = 0; // 反射计数
        Vector2 currentPosition = muzzlePos.position; // 从发射点开始

        int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        direction = transform.right;

        for (int i = 0; i < maxReflections; i++)
        {
            // 先检测敌人
            RaycastHit2D[] hitsEnemies = Physics2D.RaycastAll(currentPosition, direction, maxLength, enemyLayerMask);
            Debug.DrawLine(currentPosition, currentPosition + direction * maxLength, Color.red, 0.3f);
            bool hitWall = false;

            // 检测敌人
            foreach (var hitEnemy in hitsEnemies)
            {
                Debug.Log(hitEnemy.collider.gameObject.name + " 击中了");

                if (hitEnemy.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && isCanHit)
                {
                    Debug.Log("射线击中敌人");
                    enemy.TakeDamage(1);
                }
            }

            if (isCanHit)
            {
                StartCoroutine(nameof(HitIntervalCoroutine));
            }

            // 然后检测墙壁
            RaycastHit2D hitWallDetection = Physics2D.Raycast(currentPosition, direction, maxLength, wallLayerMask);
            if (hitWallDetection.collider != null)
            {
                Debug.Log(hitWallDetection.collider.gameObject.name + " 击中了");
                direction = Vector2.Reflect(direction, hitWallDetection.normal); // 计算反射方向
                currentPosition = hitWallDetection.point; // 将当前位置设置为墙壁的碰撞点
                hitWall = true;

                reflectionCount++;
                laser.positionCount = reflectionCount + 1; // 更新 LineRenderer 的位置数量
                laser.SetPosition(reflectionCount, currentPosition); // 设置反射点位置
            }

            // 如果没有击中墙壁，延伸激光
            if (!hitWall)
            {
                currentPosition += direction * maxLength;

                reflectionCount++;
                laser.positionCount = reflectionCount + 1; // 更新 LineRenderer 的位置数量
                laser.SetPosition(reflectionCount, currentPosition); // 设置延伸点位置
                break; // 退出循环
            }
        }
    }

    protected virtual void Filp()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized; // 计算方向
        transform.right = dir; // 使发射器朝向方向
    }

    IEnumerator HitIntervalCoroutine()
    {
        isCanHit = false;
        yield return new WaitForSeconds(hitInterval);
        isCanHit = true;
    }
}

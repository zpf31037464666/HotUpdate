using UnityEngine;
[System.Serializable]
public class WeaponInfo
{
    public int id;
    public string name;
    public int damage = 10;
    public int backForce;//击退力
    public float speed;
    public float interval;
    public float fireRate;
    public float vampire;//吸血率

    public float baseCriticalRate =0f; // 基础暴击率
    public float criticalEffect = 2f; // 暴击效果
}
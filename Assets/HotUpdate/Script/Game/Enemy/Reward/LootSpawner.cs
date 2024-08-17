using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;
    public void Spaw(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.Spaw(position+Random.insideUnitCircle);//insideUnitCircle在单位圆范围内 取一个 随机坐标。范围为1
        }
    }
}
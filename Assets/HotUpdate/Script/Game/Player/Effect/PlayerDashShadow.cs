using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashShadow : MonoBehaviour
{
    public GameObject shadowObject;

    public float shadowInterval = 0.1f; // 残影间隔时间
    public Color shadowColor = new Color(1f, 1f, 1f, 0.5f); // 残影颜色及透明度

    private float lastShadowTime; // 上一个残影生成时间
    private SpriteRenderer characterSprite; // 人物的SpriteRenderer组件

    PlayerMovement playerMovement;
    private void Awake()
    {
        characterSprite = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        //冲锋状态下
        if (playerMovement.isDashing)
        {
            CreateShadow();
        }
    }

    void CreateShadow()
    {
        // 控制生成残影的时间间隔
        if (Time.time - lastShadowTime > shadowInterval)
        {
            // 创建新的GameObject用于显示残影

            GameObject clone = ObjectPool.Instance.GetObject(shadowObject);
            //  clone.transform.position=gameObject.transform.position;


            SpriteRenderer shadowRenderer = clone.GetComponent<SpriteRenderer>();
            shadowRenderer.sortingOrder = 7;//图层显示
            shadowRenderer.sprite = characterSprite.sprite; // 使用人物的sprite
            shadowRenderer.color = shadowColor; // 设置残影的颜色及透明度
            clone.transform.position = characterSprite.transform.position;


            StartCoroutine(DelayDestoryCorout(clone));

            lastShadowTime = Time.time;
        }
    }
    IEnumerator DelayDestoryCorout(GameObject shadowPrefab)
    {

        SpriteRenderer shadowSpriteRenderer = shadowPrefab.GetComponent<SpriteRenderer>();
        Color shadowColor = shadowPrefab.GetComponent<SpriteRenderer>().color;
        float duration = 1f;

        while (duration>=0f)
        {
            duration-=Time.deltaTime/duration;
            shadowColor.a=duration;
            shadowSpriteRenderer.color = shadowColor;
            yield return null;
        }

        ObjectPool.Instance.PushObject(shadowPrefab);

    }
}

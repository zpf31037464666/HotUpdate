using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriticalDamageShow : DamageShow
{
    public Vector3 targetScale = new Vector3(2, 2, 2); // 将物体放大到两倍
    public override void ShowDamage(int damage, Vector3 position)
    {
        transform.position = position;
        canvas.enabled=true;
        damageText.text = damage.ToString()+"!";
        Vector3 localPositon = transform.position;
        transform.DOMove(localPositon+ movement, perisistTime).SetEase(Ease.OutQuad);
        // 同时缩放物体
        transform.DOScale(targetScale, perisistTime).SetEase(Ease.OutQuad).OnComplete(()=>
        {
            Original();
        });

    }
}

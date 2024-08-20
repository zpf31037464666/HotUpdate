using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DamageShow : MonoBehaviour
{
    public TMP_Text damageText;
    public Canvas canvas;
    public Vector3 movement;
    public float perisistTime;

    private float moveDistance = 1f; // 向上移动的距离
    private float moveDuration = 0.2f;  // 移动的时间
    private float waitDuration = 0.3f; // 停留的时间
    private float downMoveDuration = 0.2f; // 向下移动的时间

    public virtual void ShowDamage(int damage, Vector3 position)
    {
        transform.position = position;
        canvas.enabled=true;
        damageText.text = damage.ToString();

        transform.localScale = Vector3.zero;
        // 创建一个序列
        Sequence sequence = DOTween.Sequence();
        // 向上移动并缩放
        sequence.Append(transform.DOMoveY(transform.position.y + moveDistance, moveDuration)) // 向上移动
                .Join(transform.DOScale(Vector3.one, moveDuration)) // 缩放到1
                .AppendInterval(waitDuration) // 停留
                .Append(transform.DOMoveY(transform.position.y - moveDistance, downMoveDuration)) // 向下移动
                .Join(transform.DOScale(Vector3.zero, downMoveDuration)); // 缩放到0

        // 启动序列
        sequence.Play();

        //   UIEffect.Instance.SetUIMove(gameObject, transform.position+ movement, perisistTime, () => Original());
    }
    public virtual void ShowDamage(string contect, Vector3 position)
    {
        transform.position = position;
        canvas.enabled=true;
        damageText.text = contect;
        Vector3 localPositon = transform.position;

        UIEffect.Instance.SetUIMove(gameObject, localPositon+ movement, perisistTime, () => Original());
    }

    public virtual void SetTextColor(Color color)
    {
        damageText.color = color;
    }
    public void Original()
    {
        canvas.enabled=false;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        damageText.color = Color.white;
        ObjectPool.Instance.PushObject(gameObject);
    }
}

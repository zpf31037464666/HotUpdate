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
    public virtual void ShowDamage(int damage, Vector3 position)
    {
        transform.position = position;
        canvas.enabled=true;
        damageText.text = damage.ToString();
        Vector3 localPositon = transform.position;
        UIEffect.Instance.SetUIMove(gameObject, localPositon+ movement, perisistTime, () => Original());
    }
    public void Original()
    {
        canvas.enabled=false;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        ObjectPool.Instance.PushObject(gameObject);
    }
}

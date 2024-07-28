using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DamageShow : MonoBehaviour
{
    [SerializeField] TMP_Text damageText;
    [SerializeField] Canvas canvas;
    public Vector3 movement;
    public float perisistTime;


    public void ShowDamage(int damage, Vector3 position)
    {
        transform.position = position;
        ShowDamage(damage);

    }
    public void ShowDamage(int damage)
    {
        canvas.enabled=true;
        damageText.text = damage.ToString();
        Vector3 localPositon = transform.position;
        UIEffect.Instance.SetUIMove(gameObject, localPositon+ movement, perisistTime, () => Original());
    }
    public void Original()
    {
        canvas.enabled=false;
        gameObject.transform.localPosition = Vector3.zero;

        ObjectPool.Instance.PushObject(gameObject);
    }

}

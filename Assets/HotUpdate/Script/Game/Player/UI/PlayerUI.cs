using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text hpText;
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImagefront;
    [Header("Dash")]
    [SerializeField] Image dashImage;
    [SerializeField] Image hurtImage;
    [Header("Buff")]
    [SerializeField] BuffItem buffItem;
    [SerializeField] Transform buffItemGroup;
    public List<GameObject> iconList = new List<GameObject>();

    public Button dashButton;
    public Joystick joystick;

    float fillSpeed = 0.5f;//填充速度
    bool isDelayFill = true;//是否填充
    float DelayFillTime = 0.5f;  //填充等待时间
    private float currentFillAmount;
    private float targetFillAmount;
    float t;

    float previousFillAmount;

    private float duration = 1f;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    protected  void Awake()
    {
        waitForDelayFill =new WaitForSeconds(DelayFillTime);
    }
    private void Start()
    {
        SetNameText("张三");
    }
    private void OnEnable()
    {
        Player.OnChangeHealthEvent+=onTakeDamageEvent;
        Player.OnHurtEvent+=onHurtEvent;

        PlayerBuffHandle.OnBuffEntry+=BuffEntry;
        PlayerBuffHandle.OnBuffUpdate+=BuffUpdate;
        PlayerBuffHandle.OnBuffExit+=BuffExit;
    }



    private void OnDisable()
    {
        Player.OnChangeHealthEvent-=onTakeDamageEvent;
        Player.OnHurtEvent-=onHurtEvent;

        PlayerBuffHandle.OnBuffEntry-=BuffEntry;
        PlayerBuffHandle.OnBuffUpdate-=BuffUpdate;
        PlayerBuffHandle.OnBuffExit-=BuffExit;
    }
    #region Event
    private void onTakeDamageEvent(Player player)
    {
        SetHealth(player.Health, player.MaxHealth);
    }
    private void onHurtEvent(Player player)
    {
        Hurt();
    }

    private void BuffEntry(BuffHandle handle)
    {
        var clone = Instantiate(buffItem.gameObject, buffItemGroup);
        iconList.Add(clone);
        UpdateBuffIcons(handle);
    }
    private void BuffUpdate(BuffHandle handle)
    {
        UpdateBuffIcons(handle);
    }
    private void BuffExit(BuffHandle handle)
    {
        // 删除最后一个元素并销毁
        if (iconList.Count > 0)
        {
            int lastIndex = iconList.Count - 1;
            GameObject objToDestroy = iconList[lastIndex];
            iconList.RemoveAt(lastIndex);
            Destroy(objToDestroy);
        }
        UpdateBuffIcons(handle);
    }
    // 提取更新 Buff 图标的逻辑
    private void UpdateBuffIcons(BuffHandle handle)
    {
        int buffCount = handle.buffList.Count;

        for (int i = 0; i < iconList.Count; i++)
        {
            if (i < buffCount) // 确保不越界
            {
                BuffItem buff = iconList[i].GetComponent<BuffItem>();
                buff.UpdateStackText(handle.buffList[i].curStack);
                buff.SetIconImage(handle.buffList[i].info.showSprite);
                buff.UpdateCoolTime(handle.buffList[i].duationTimer / handle.buffList[i].buffData.duration);
            }
        }
    }

    #endregion
    public void SetDashImageFill(float value)
    {
        dashImage.fillAmount = value;
    }
    public void SetNameText(string name)
    {
        nameText.text = name;
    }
    public void SetHealth(float currentValue, float maxValue)
    {
        hpText.text= currentValue.ToString()+"/"+maxValue.ToString();
        UpdateHpState(currentValue, maxValue);
    }

    //初始化血量的值
    public void InitializeHP(float currentValue, float maxValue)
    {
        currentFillAmount=currentValue/maxValue;
        targetFillAmount=currentValue;
        fillImageBack.fillAmount=currentFillAmount;
        fillImagefront.fillAmount=currentFillAmount;
    }
    //数值持续更新
    private void UpdateHpState(float currentValue, float maxValue)
    {
        targetFillAmount=currentValue/maxValue;
        if (bufferedFillingCoroutine !=null)
        {
            //停止协程 避免过多重复协程
            StopCoroutine(bufferedFillingCoroutine);
        }
        //玩家血量减少时
        if (currentFillAmount>targetFillAmount)
        {
            fillImagefront.fillAmount=targetFillAmount;
            bufferedFillingCoroutine =StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        //玩家血量增加时
        if (currentFillAmount<targetFillAmount)
        {
            fillImageBack.fillAmount=targetFillAmount;
            bufferedFillingCoroutine =  StartCoroutine(BufferedFillingCoroutine(fillImagefront));
        }
    }
    public void Hurt()
    {
        StartCoroutine(HurtCoroutine());
    }
    //缓冲增加血量
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (isDelayFill)
        {
            yield return waitForDelayFill;
        }
        t=0f;
        previousFillAmount=currentFillAmount;
        while (t<1f)
        {
            t+=Time.fixedDeltaTime*fillSpeed;
            currentFillAmount=Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount=currentFillAmount;
            yield return null;
        }
    }
    IEnumerator HurtCoroutine()
    {
        hurtImage.gameObject.SetActive(true);
        Color originalColor = hurtImage.color;
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            hurtImage.color = newColor;
            yield return null;
        }
        Color finalColor = originalColor;
        finalColor.a = 0f;
        hurtImage.color = finalColor;
    }
}

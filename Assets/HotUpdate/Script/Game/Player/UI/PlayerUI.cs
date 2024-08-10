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
    [Header("MP")]
    [SerializeField] Image mpImage;
    [SerializeField] TMP_Text mpText;
    [Header("Level")]
    [SerializeField] Image levelImage;

    [Header("Dash")]
    [SerializeField] Image dashImage;
    [SerializeField] Image hurtImage;
    [Header("Buff")]
    [SerializeField] BuffItem buffItem;
    [SerializeField] Transform buffItemGroup;
    public List<GameObject> iconList = new List<GameObject>();

    [Header("Skill")]
    [SerializeField] SkillItem skillItem;
    [SerializeField] Transform skillItemGroup;
    public List<SkillItem> skillItemList = new List<SkillItem>();

    [Header("MoveButton")]
    public Button dashButton;
    public Joystick joystick;

    float fillSpeed = 0.5f;//填充速度
    bool isDelayFill = true;//是否填充
    float DelayFillTime = 0.5f;  //填充等待时间
    private float currentFillAmount;
    private float targetFillAmount;
    float t;

    //生成技能的间距和半径
    int skillNumber = 3;
    int currentNumer = 1;
    private float radius = 300f;
    private float offAngle = 30f;

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
        Player.OnChangeHealthEvent+=TakeDamageEvent;
        Player.OnHurtEvent+=HurtEvent;
        Player.OnChangeExpEvent+=ChangeExpEvent;
        Player.OnChangeLevelEvent+=ChangeLevelEvent;
        Player.OnChangeMpEvent+=ChangeMpEvent;




        PlayerBuffHandle.OnBuffEntry+=BuffEntry;
        PlayerBuffHandle.OnBuffUpdate+=BuffUpdate;
        PlayerBuffHandle.OnBuffExit+=BuffExit;


        PlayerSkill.OnSkillAddEvent+=SkillAddEvent;
        PlayerSkill.OnSkillRemoveEvent+=SkillRemoveEvent;
        PlayerSkill.OnSkillUpdateEvent+=SkillUpdateEvent;
    }


    private void OnDisable()
    {
        Player.OnChangeHealthEvent-=TakeDamageEvent;
        Player.OnHurtEvent-=HurtEvent;

        Player.OnChangeExpEvent-=ChangeExpEvent;
        Player.OnChangeLevelEvent-=ChangeLevelEvent;
        Player.OnChangeMpEvent-=ChangeMpEvent;


        PlayerBuffHandle.OnBuffEntry-=BuffEntry;
        PlayerBuffHandle.OnBuffUpdate-=BuffUpdate;
        PlayerBuffHandle.OnBuffExit-=BuffExit;


        PlayerSkill.OnSkillAddEvent-=SkillAddEvent;
        PlayerSkill.OnSkillRemoveEvent-=SkillRemoveEvent;
        PlayerSkill.OnSkillUpdateEvent-=SkillUpdateEvent;

    }
    #region Event
    private void TakeDamageEvent(Player player)
    {
        SetHealth(player.Health, player.MaxHealth);
    }
    private void HurtEvent(Player player)
    {
        Hurt();
    }

    private void ChangeMpEvent(Player player)
    {
        SetMP(player.CurrentMp, player.MaxMP);
    }

    private void ChangeLevelEvent(Player player)
    {

    }

    private void ChangeExpEvent(Player player)
    {
        SetLevelImage(player.CurrentExp/ player.RequiteExp);
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

    private void SkillAddEvent(Skill skill)
    {
        GameObject clone = Instantiate(skillItem.gameObject, skillItemGroup, false);
        SkillItem cloneItme = clone.GetComponent<SkillItem>();
        cloneItme.SetAction(skill.Apply);
        skillItemList.Add(cloneItme);

        //环形排列
        clone.transform.position=CalculateCirclePosition(skillItemGroup.transform.position, radius, skillNumber, currentNumer);
        currentNumer++;

    }
    private void SkillRemoveEvent(Skill skill)
    {
        Debug.Log("暂时移除技能");

    }
    private void SkillUpdateEvent(List<Skill> list)
    {
        // 确保 skillItemList 的大小与 list 的大小一致
        for (int i = 0; i < list.Count; i++)
        {
            skillItemList[i].SetIcon(list[i].info.showSprite);
            skillItemList[i].UpdateCoolDown(list[i].coolDownTime / list[i].SkillData.CoolDownTime);
            skillItemList[i].UpdateCoolDownText(list[i].coolDownTime);
            skillItemList[i].DisableImage(!list[i].IsUse);
        }
    }


    #endregion
    public void SetMP(float currentMp, float maxMp)
    {
        mpText.text= currentMp.ToString()+"/"+maxMp.ToString();
        mpImage.fillAmount= currentMp/maxMp;
    }
    public void SetLevelImage(float value)
    {
        levelImage.fillAmount = value;
    }
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



    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Debug.Log("测试生成技能");

    //        GameObject clone= Instantiate(skillItem.gameObject,skillItemGroup,false);
    //        clone.transform.position=CalculateCirclePosition(skillItemGroup.transform.position, radius, skillNumber, currentNumer);

    //        currentNumer++;
    //    }
    //}
    Vector3 CalculateCirclePosition(Vector3 center, float radius, int totalPoints, int currentIndex)
    {
        float angle = (120 / totalPoints) * currentIndex;
        angle+=offAngle;
        //Mathf.Deg2Rad 用于将角度转换为弧度。具体来说，它将角度值乘以 π/180
        float radians = angle * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(radians);
        float y = center.y + radius * Mathf.Sin(radians);

        return new Vector3(x, y, center.z);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text hpText;
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImagefront;

    [SerializeField] Image dashImage;
    //[SerializeField] Image hurtImage;
    //[SerializeField] private Text scoreText;

    //[SerializeField] Transform lifeTransform;
    //[SerializeField] GameObject lifeObjects;


    public Button dashButton;
    public Joystick joystick;


    [SerializeField] float fillSpeed = 0.5f;//填充速度
    [SerializeField] bool isDelayFill = true;//是否填充
    [SerializeField] float DelayFillTime = 0.5f;  //填充等待时间

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


    public void SetDashImageFill(float value)
    {
        dashImage.fillAmount = value;
    }

    public void SetNameText(string name)
    {
        nameText.text = name;
    }
    //public void SetScoreText(int score)
    //{
    //    scoreText.text=score.ToString();
    //}
    //public void SetLifeNumber(int number)
    //{
    //    foreach (Transform child in lifeTransform)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    for (int i = 0; i < number; i++)
    //    {
    //        Instantiate(lifeObjects, lifeTransform);
    //    }
    //}

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
    public void UpdateHpState(float currentValue, float maxValue)
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
    //public void Hurt()
    //{
    //    StartCoroutine(HurtCoroutine());
    //}
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
    //IEnumerator HurtCoroutine()
    //{
    //    hurtImage.gameObject.SetActive(true);
    //    Color originalColor = hurtImage.color;
    //    for (float t = 0; t < 1; t += Time.deltaTime / duration)
    //    {
    //        Color newColor = originalColor;
    //        newColor.a = Mathf.Lerp(1f, 0f, t);
    //        hurtImage.color = newColor;
    //        yield return null;
    //    }
    //    Color finalColor = originalColor;
    //    finalColor.a = 0f;
    //    hurtImage.color = finalColor;
    //}
}

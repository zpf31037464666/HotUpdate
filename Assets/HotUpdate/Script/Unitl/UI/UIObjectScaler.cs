using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIObjectScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] private float multiply = 1.2f;
    [SerializeField] private float duration = 0.5f;
    void Start()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * multiply, duration);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration);
    }
}

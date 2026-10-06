using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject arrow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrow.SetActive(true);

        RectTransform arrowRect = arrow.GetComponent<RectTransform>();
        RectTransform buttonRect = GetComponent<RectTransform>();

       arrowRect.position = buttonRect.TransformPoint(
    new Vector3(
        -buttonRect.rect.width / 2 - 10f,
        0f,
        0f
    )
);
 }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrow.SetActive(false);
    }
}
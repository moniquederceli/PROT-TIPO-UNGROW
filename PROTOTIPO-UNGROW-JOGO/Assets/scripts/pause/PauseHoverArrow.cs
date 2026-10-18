
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseHoverArrow : MonoBehaviour
{
    public GameObject arrow;

    private RectTransform buttonRect;
    private RectTransform arrowRect;
    private bool isHovering = false;

    void Awake()
    {
        buttonRect = GetComponent<RectTransform>();

        if (arrow != null)
        {
            arrowRect = arrow.GetComponent<RectTransform>();
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        if (arrow == null || buttonRect == null ||
            Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        bool hoveringNow =
            RectTransformUtility.RectangleContainsScreenPoint(
                buttonRect,
                mousePosition,
                null
            );

        if (hoveringNow == isHovering)
            return;

        isHovering = hoveringNow;
        arrow.SetActive(isHovering);

        if (isHovering && arrowRect != null)
        {
            arrowRect.position = buttonRect.TransformPoint(
                new Vector3(
                    -buttonRect.rect.width / 2 - -3f,
                    -10f,
                    0f
                )
            );
        }
    }
}

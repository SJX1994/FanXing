using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Rules_Visualization_ButtonListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovering = false;
    public bool IsHovering => isHovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        // Debug.Log("Mouse is hovering over the button");
        // 在这里执行鼠标悬停时的操作
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        // Debug.Log("Mouse is no longer hovering over the button");
        // 在这里执行鼠标移出时的操作
    }
}
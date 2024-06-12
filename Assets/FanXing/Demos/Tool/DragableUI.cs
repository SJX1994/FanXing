using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public enum DragState
    {
        None,
        Dragging,
        EndDrag,
    }
    public DragState DragState_Current = DragState.None;
    public bool CanDrag = true;
    private RectTransform rectTransform;
 
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector3 originalPosition;
 

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 保存原始位置
        originalPosition = rectTransform.localPosition;
        DragState_Current = DragState.None;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanDrag) return;
        // 确保UI元素在最前面
        rectTransform.SetAsLastSibling();
        DragState_Current = DragState.Dragging;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;
        // 计算新位置
        Vector2 newPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out newPosition
        );

        // 更新UI元素位置
        rectTransform.localPosition = newPosition;
        DragState_Current = DragState.Dragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;
        // 将UI元素移回原位
        rectTransform.localPosition = originalPosition;
        DragState_Current = DragState.EndDrag;
    }
}
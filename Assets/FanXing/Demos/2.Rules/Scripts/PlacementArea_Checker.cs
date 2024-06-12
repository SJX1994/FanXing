using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
namespace FanXing.Demos.Rules
{
    public class PlacementArea_Checker : MonoBehaviour
    {
        [SerializeField]
        DragableUI dragableUI;
        [SerializeField]
        GameObject objectToPlace;
        bool canPlace = false;
        Vector3 creatPosition;
        bool isCreated = false;
        void Start()
        {
            
        }
        void Update()
        {
            if(isCreated)return;
            if(dragableUI.DragState_Current == DragableUI.DragState.Dragging)
            {
                PerformRaycast();
            }else if(dragableUI.DragState_Current == DragableUI.DragState.EndDrag)
            {
                if(canPlace)
                {
                    Instantiate(objectToPlace, creatPosition, Quaternion.identity);
                    TemporaryStorage.CurrentCharacter++;
                    isCreated = true;
                    dragableUI.CanDrag = false;
                    canPlace = false;
                    gameObject.SetActive(false);
                }
                PlaceObjectDisplay(false);
            }else if(dragableUI.DragState_Current == DragableUI.DragState.None)
            {
                PlaceObjectDisplay(false);
            }

        }
        void PerformRaycast()
        {
            RectTransform rectTransform = dragableUI.GetComponent<RectTransform>();
            // 将UI元素的本地坐标转换为世界坐标
            // 获取UI元素的屏幕坐标范围
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // 计算UI元素在世界坐标中的中心点
            Vector3 center = (corners[0] + corners[2]) / 2f;

            // 创建射线
            Ray ray = Camera.main.ScreenPointToRay(center);
            RaycastHit hit;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 0.1f);
            // 检测射线是否击中世界坐标下的物体
            if (!Physics.Raycast(ray, out hit))return;
            if (hit.collider.gameObject.name == "PlacementArea")
            {
                creatPosition = hit.point;
                PlaceObjectDisplay(true);
            }else
            {
                PlaceObjectDisplay(false);
            }
            // 检测到击中物体
            // Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        void PlaceObjectDisplay(bool canPlace)
        {
            if(canPlace)
            {
                Image image = transform.GetComponent<Image>();
                var tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;
                transform.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }else
            {
                Image image = transform.GetComponent<Image>();
                var tempColor = image.color;
                tempColor.a = 0.5f;
                image.color = tempColor;
                transform.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            }
            this.canPlace = canPlace;
            
        }
    }
}

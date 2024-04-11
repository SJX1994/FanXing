using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_BeenSelected : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer_BeenSelected;
        [SerializeField] int lineRenderer_segments = 20;
        float radius = 0f;
        Tween tween_radius = null;
        float alpha = 0;
        Tween tween_alpha = null;
        float duration = 0.2f;
        MaterialPropertyBlock propertyBlock;
        void Start()
        {
            lineRenderer_BeenSelected.gameObject.SetActive(false);
            TemporaryStorage.OnBuoyStateChanged += (state) =>
            {
                if(state != OperateLayer_Buoy.State.Action)
                {
                    OnCanceled();
                }
            };
        }
        void Update()
        {
            
        }
        void OnDestroy()
        {
            tween_radius?.Kill();
            tween_alpha?.Kill();
        }
        public void OnSelected()
        {
            lineRenderer_BeenSelected.gameObject.SetActive(true);
            PropertyBlock_Alpha(0);
            tween_radius = DOVirtual.Float(
            9f, // 起始值
            3f, // 结束值
            duration, // 动画持续时间
            (f)=>
            {
                radius = f;
                CreateSelfCircleShape_Display(radius);
            } // 更新值的回调函数
            );
            tween_alpha = DOVirtual.Float(
            0f, // 起始值
            1f, // 结束值
            duration, // 动画持续时间
            (f)=>
            {
                alpha = f;
                PropertyBlock_Alpha(alpha);
            } // 更新值的回调函数
            ).OnComplete(()=>
            {
                PropertyBlock_Alpha(1);
            });
            
        }
        public void OnCanceled()
        {
            PropertyBlock_Alpha(1);
            tween_radius = DOVirtual.Float(
            3f, // 起始值
            9f, // 结束值
            duration, // 动画持续时间
            (f)=>
            {
                radius = f;
                CreateSelfCircleShape_Display(radius);
            } // 更新值的回调函数
            );
            tween_alpha = DOVirtual.Float(
            1f, // 起始值
            0f, // 结束值
            duration, // 动画持续时间
            (f)=>
            {
                alpha = f;
                PropertyBlock_Alpha(alpha);
            } // 更新值的回调函数
            ).OnComplete(()=>
            {
                PropertyBlock_Alpha(0);
                lineRenderer_BeenSelected.gameObject.SetActive(false);
            });
        }
        
        void CreateSelfCircleShape_Display(float radius)
        {
            
            lineRenderer_BeenSelected.positionCount = lineRenderer_segments + 1;
            lineRenderer_BeenSelected.loop = true;

            float x;
            float z;
            float angle = 20f;
            for (int i = 0; i < (lineRenderer_segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                lineRenderer_BeenSelected.SetPosition(i, new Vector3(x, 1.6f, z) + transform.position);
                angle += 360f / lineRenderer_segments;
            }
        }
        void PropertyBlock_Alpha(float SetFloat)
        {
            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
            lineRenderer_BeenSelected.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Alpha", SetFloat);
            lineRenderer_BeenSelected.SetPropertyBlock(propertyBlock);
        }
        
    }
}


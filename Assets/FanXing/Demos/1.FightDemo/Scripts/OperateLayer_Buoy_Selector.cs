using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class OperateLayer_Buoy_Selector : MonoBehaviour
    {
        [SerializeField]Transform selector;
        
        public void UpdateSelector()
        {
            RaycastHit hit;
            if (!Physics.Raycast(selector.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                TemporaryStorage.InvokeOnHideUnitDescription();
            }else
            {
                TemporaryStorage.InvokeOnBuoyStateSelected(hit.collider.gameObject);
            // Debug.Log(hit.point);
            }
            
        }
    }
}

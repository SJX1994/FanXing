using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.FightDemo
{
    public class OperateKeyboard : MonoBehaviour
    {
        [SerializeField] KeyCode confirmKey;
        [SerializeField] KeyCode cancelKey;
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(confirmKey))
            {
                TemporaryStorage.InvokeOnConfirmKeyPressed();
            }else if(Input.GetKeyDown(cancelKey))
            {
                TemporaryStorage.InvokeOnCancelKeyPressed();
            }
        }
    }
}

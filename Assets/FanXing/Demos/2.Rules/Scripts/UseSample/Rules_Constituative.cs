using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.Demos.Rules
{
    // 不可被修改的规则
    public class Rules_Constituative : MonoBehaviour
    {
        // 1. 单位具有可量化、可修改的属性。
        // 2. 空间：都作用于2维平面。设计师和玩家 利用点线面 进行博弈。
        // 3. 时间：游戏时间 本身是连续的，但可以被操作所打断成 离散的时间。
        void Awake()
        {
            Events.Reset();
            TemporaryStorage.Reset();
        }
        void OnDestroy()
        {
            Events.Reset();
            TemporaryStorage.Reset();    
        }
    }
}


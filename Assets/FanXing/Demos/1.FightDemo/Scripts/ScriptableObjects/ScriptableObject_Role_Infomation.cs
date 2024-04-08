

using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "Role_Infomation_", menuName = "ScriptableObjects/Role_Infomation", order = 1)]
    public class ScriptableObject_Role_Infomation : ScriptableObject
    {
        // 机甲类型
        public enum RoleType
        {
            // 近身格斗
            CloseCombat,
            // 万能
            Universal,
            // 远距离
            LongDistance,
            // 飞行
            Flight,
            Null
        }
        public RoleType roleType;
        
    }
}

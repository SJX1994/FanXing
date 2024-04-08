

using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "Action_", menuName = "ScriptableObjects/Action", order = 1)]
    public class ScriptableObject_Action : ScriptableObject
    {
        public bool IsUnlocked = false;
        public string ActionName = "Action";
        // 技能有效范围标签
        public enum ActionType
        {
            AgainstAir,
            AgainstGround,
            AgainstAll
        }
        public ActionType actionType = ActionType.AgainstAll;
        // 是否可以移动释放技能
        public bool CanMove = false;
        // 技能种类
        [System.Flags]
        public enum ActionCategory
        {
            None = 0,
            Shield = 000001, // 护盾
            ArmorPiercing = 000010, // 穿甲
            CallSupport = 000100, // 召唤支援
            Penetrate = 001000, // 穿透
            ETC = 010000 // 其他?
        }
        public ActionCategory actionCategory = ActionCategory.Shield;
        public GameObject SupportPrefab;
        public int damage;
        public int shield;
        public int armorPiercing;
        public float cooldown;
    }
}

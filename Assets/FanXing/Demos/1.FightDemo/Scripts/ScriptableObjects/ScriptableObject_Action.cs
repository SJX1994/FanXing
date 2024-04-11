

using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "Action_", menuName = "ScriptableObjects/Action", order = 1)]
    public class ScriptableObject_Action : ScriptableObject
    {
        public bool IsUnlocked = false;
        public string ActionName = "Action";
        public string Description = "Description";
        public Sprite Icon; // 技能图标
        public int Cost; // 消耗
        public int Range; // 范围
        public enum Action
        {
            // 扇形范围技能
            FanShape,
            // 直线范围技能
            LineShape,
            // 单体技能
            SingleTarget,
            // 点对点技能
            PointToPoint,
            // 圆形范围技能
            CircleShape,
            // 方形范围技能
            SquareShape,
            // 全图技能
            WholeMap
        }
        public Action action = Action.SingleTarget;
        // 技能有效范围标签
        public enum ActionType
        {
            AgainstAir,
            AgainstGround,
            AgainstAll
        }
        public ActionType actionType = ActionType.AgainstAll;
        public bool CanMove = false; // 是否可以在释放技能前移动
        public GameObject SupportPrefab; // 支援单位
        public int damagePercentage; // 伤害百分比
        public int shield; // 护盾
        public int armorPiercing; // 穿甲
        public float cooldown; // 冷却
    }
}

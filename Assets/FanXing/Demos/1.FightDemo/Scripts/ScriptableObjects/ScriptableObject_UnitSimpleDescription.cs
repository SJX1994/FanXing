

using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "UnitSimpleDescription_", menuName = "ScriptableObjects/UnitSimpleDescription", order = 1)]
    public class ScriptableObject_UnitSimpleDescription : ScriptableObject
    {
        public TemporaryStorage.UnitName selectName;
        public TemporaryStorage.UnitType selectType;
        public TemporaryStorage.UnitCamp selectCamp;
        public int HP_max;
        public int HP_current;
        public Sprite Profile;
    }
}
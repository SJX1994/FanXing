using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace FanXing.FightDemo
{
    public class UI_UnitSimpleDescription : MonoBehaviour
    {
        [SerializeField] Image image_Profile;
        [SerializeField] Image image_Profile_Fram;
        [SerializeField] Image image_background;
        [SerializeField] TextMeshProUGUI text_Name;
        [SerializeField] TextMeshProUGUI text_Type;
        [SerializeField] TextMeshProUGUI text_Camp;
        [SerializeField] Slider slider_HP;
        [SerializeField] Color color_Camp_Enemy;
        [SerializeField] Color color_Camp_Friend;
        public void UpdateDescription(ScriptableObject_UnitSimpleDescription scriptableObject_UnitSimpleDescription)
        {
            slider_HP.value = (float)scriptableObject_UnitSimpleDescription.HP_current / scriptableObject_UnitSimpleDescription.HP_max;
            image_Profile.sprite = scriptableObject_UnitSimpleDescription.Profile;
            image_Profile_Fram.color = scriptableObject_UnitSimpleDescription.selectCamp == TemporaryStorage.UnitCamp.Enemy ? color_Camp_Enemy : color_Camp_Friend;
            image_Profile_Fram.color = new Color(image_Profile.color.r, image_Profile.color.g, image_Profile.color.b, 0.7f);
            text_Name.text = scriptableObject_UnitSimpleDescription.selectName.ToString();
            text_Type.text = scriptableObject_UnitSimpleDescription.selectType.ToString();
            text_Camp.text = scriptableObject_UnitSimpleDescription.selectCamp.ToString();
            image_background.color = scriptableObject_UnitSimpleDescription.selectCamp == TemporaryStorage.UnitCamp.Enemy ? color_Camp_Enemy : color_Camp_Friend;
            image_background.color = new Color(image_background.color.r, image_background.color.g, image_background.color.b, 0.3f);
        }

    }
}


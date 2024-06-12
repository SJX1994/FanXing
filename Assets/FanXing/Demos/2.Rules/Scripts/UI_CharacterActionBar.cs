using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace FanXing.Demos.Rules
{
public class UI_CharacterActionBar : MonoBehaviour
{
    [SerializeField]
    Image[] actionBars; // 角色行动条的UI Image组件数组
    [SerializeField]
    float[] durations; // 每个角色的进度条持续时间
    [SerializeField]
    RectTransform[] characterIcons; // 角色头像的RectTransform组件数组
    [SerializeField]
    float iconHeight = 50.0f; // 头像悬置的高度

    private float[] actionBarProgress; // 每个角色的进度值
    private float[] actionBarTimers; // 每个角色的计时器
    private List<int> sortedIndices; // 按照进度排序的索引列表
    private ScriptableObject_UnitAttributes[] role_attributes;
    private void Start()
    {
        Events.OnInitProssesBar += Init;
        Events.OnWhoProssesBar_Reset += ResetActionBar;
    }
    private void Init(ScriptableObject_UnitAttributes[] scriptableObject_UnitAttributes,Sprite[] characterIcons,float[] durations)
    {
        // 初始化进度值和计时器
        actionBarProgress = new float[actionBars.Length];
        actionBarTimers = new float[actionBars.Length];
        role_attributes = new ScriptableObject_UnitAttributes[scriptableObject_UnitAttributes.Length];
        sortedIndices = new List<int>();
        for (int i = 0; i < actionBars.Length; i++)
        {
            actionBars[i].fillAmount = 0.0f;
            actionBarProgress[i] = 0.0f;
            actionBarTimers[i] = 0.0f;
            this.characterIcons[i].transform.GetComponent<Image>().sprite = characterIcons[i];
            this.durations[i] = durations[i];
            role_attributes[i] = scriptableObject_UnitAttributes[i];
            sortedIndices.Add(i);
        }
    }

    private void Update()
    {
        if(TemporaryStorage.GameTimeIsRunning == false)return;
        // 更新每个角色的进度条
        for (int i = 0; i < actionBars.Length; i++)
        {
            actionBarTimers[i] += Time.deltaTime;
            actionBarProgress[i] = Mathf.Clamp01(actionBarTimers[i] / durations[i]);
            actionBars[sortedIndices[i]].fillAmount = actionBarProgress[sortedIndices[i]];

            // 确定头像悬置的位置
            float barWidth = actionBars[sortedIndices[i]].rectTransform.rect.width;
            float iconX = actionBars[sortedIndices[i]].transform.position.x + actionBarProgress[sortedIndices[i]] * barWidth;
            float iconY = actionBars[sortedIndices[i]].transform.position.y + iconHeight;
            characterIcons[sortedIndices[i]].position = new Vector3(iconX, iconY, 0);

            // 进度条达到1后的处理逻辑
            if (actionBarProgress[sortedIndices[i]] >= 1.0f)
            {
                // 例如：角色可以执行行动

                Debug.Log($"Character {sortedIndices[i]} can act!");
                Events.InvokeWhoProssesBar_Action(role_attributes[sortedIndices[i]]);

                // 重置进度和计时器
                actionBarTimers[sortedIndices[i]] = 0.0f;
            }
            // 根据进度值对actionBars进行排序
            SortActionBars();
        }
        
    }
    private void ResetActionBar(ScriptableObject_UnitAttributes scriptableObject_UnitAttributes)
    {
        ResetActionBar_string(scriptableObject_UnitAttributes.UnitName);
    }
    // 重置进度和计时器
    private void ResetActionBar_string(string unitName)
    {
        for (int i = 0; i < actionBars.Length; i++)
        {
            if (role_attributes[i].UnitName == unitName)
            {
                actionBarTimers[i] = 0.0f;
                actionBarProgress[i] = 0.0f;
                actionBars[i].fillAmount = 0.0f;
                break;
            }
        }
    }
    private void SortActionBars()
    {
        // 对actionBarProgress进行排序,并更新sortedIndices
        sortedIndices.Sort((a, b) => actionBarProgress[b].CompareTo(actionBarProgress[a]));
        // 更新UI Image组件的显示顺序
        for (int i = 0; i < actionBars.Length; i++)
        {
            actionBars[sortedIndices[i]].transform.SetSiblingIndex(i);
        }
    }
}
}
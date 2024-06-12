using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class UnitBehavior : MonoBehaviour
    {
        [SerializeField]
        protected ScriptableObject_UnitAttributes attributes;
        [SerializeField]
        protected TextMeshPro textMeshPro;
        string[] names = new string[] { "A", "B", "C", "D", "E" };
        public virtual void DisplayAttributes()
        {
            attributes = Instantiate(attributes);
            attributes.TextMeshPro = textMeshPro;
            // attributes.textMeshPro.text = attributes.UnitName + names[Random.Range(0,names.Length)] + "\n HP:" + attributes.Health + "\n Atk:" + attributes.Attack;
            attributes.TextMeshPro.text = attributes.UnitName + names[Random.Range(0,names.Length)] + "\n HP:" + Random.Range(0,attributes.Health) + "\n Atk:" + Random.Range(0,attributes.Attack);
        }
    }
}
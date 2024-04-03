using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
namespace FanXing.FightDemo
{
    public class FightLayer_Roles : MonoBehaviour
    {
        [SerializeField] List<FightLayer_Roles_Role> roles = new();
        private ScriptableObject_UnitSimpleDescription temp_unitSimpleDescription;
        private ScriptableObject_UI_Manager_DisplayOptions temp_UI_Manager_DisplayOptions;
        void Start()
        {
            roles = GetComponentsInChildren<FightLayer_Roles_Role>().ToList();
            TemporaryStorage.OnBuoySelectedObject += (go) =>
            {
                GameObject selectTestGameObject = go;
                ShowRoleDescription(selectTestGameObject);
                FightLayer_Roles_Role targetRole = go.transform.parent.GetComponent<FightLayer_Roles_Role>();
                targetRole = roles.Find(role => role == targetRole);
                if(!targetRole)return;
                TemporaryStorage.BuoySelectingObject = targetRole.gameObject;
                
            };
            TemporaryStorage.OnConfirmKeyPressed += () =>
            {
                if(TemporaryStorage.BuoySelectingObject == null)return;
                TemporaryStorage.BuoySelectedObject = TemporaryStorage.BuoySelectingObject;
                TemporaryStorage.BuoySelectingObject = null;
                temp_UI_Manager_DisplayOptions = TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().selectTester.GetUI_Manager_DisplayOptions();
                TemporaryStorage.InvokeOnRestBuoyPosition(TemporaryStorage.BuoySelectedObject.transform.position);
                TemporaryStorage.InvokeOnShow_UI_Manager(TemporaryStorage.BuoySelectedObject,temp_UI_Manager_DisplayOptions);
                TemporaryStorage.InvokeOnMoveFinish(TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role_Move>());
            };
        }
        void ShowRoleDescription(GameObject go)
        {
            if(!go.TryGetComponent(out FightLayer_Roles_Role_SelectTester selectTest))return;
            temp_unitSimpleDescription = selectTest.GetUnitSimpleDescription();
            TemporaryStorage.InvokeOnShowUnitDescription(go,temp_unitSimpleDescription);
        }
        
    }
}

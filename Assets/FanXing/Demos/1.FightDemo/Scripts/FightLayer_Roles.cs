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
        private bool operating = false;
        
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
                TemporaryStorage.CurrentBuoy_RoleType = TemporaryStorage.BuoySelectingObject.GetComponent<FightLayer_Roles_Role>().roleInfo.RoleType;
                TemporaryStorage.BuoySelectedObject = TemporaryStorage.BuoySelectingObject;
                TemporaryStorage.BuoySelectingObject = null;
                temp_UI_Manager_DisplayOptions = TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().selectTester.GetUI_Manager_DisplayOptions();
                if(TemporaryStorage.BuoySelectedObject.TryGetComponent(out FightLayer_Roles_Role_Move_Flight roleMove))
                {
                    roleMove.m_From_Flight = roleMove.transform.position;
                    roleMove.m_Destination_Flight = TemporaryStorage.BuoyPosition;
                    roleMove.m_Destination_Flight.y = roleMove.transform.position.y;
                }
                TemporaryStorage.InvokeOnRestBuoyPosition(TemporaryStorage.BuoySelectedObject.transform.position);
                TemporaryStorage.InvokeOnShow_UI_Manager(TemporaryStorage.BuoySelectedObject,temp_UI_Manager_DisplayOptions);
                TemporaryStorage.InvokeOnMoveFinish(TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role_Move_Ground>());
            };
            TemporaryStorage.OnCancelKeyPressed += () =>
            {
                if(!TemporaryStorage.BuoySelectingObject || !TemporaryStorage.BuoySelectedObject)return;
                TemporaryStorage.InvokeOnRestBuoyPosition(TemporaryStorage.BuoySelectedObject.transform.position);
                TemporaryStorage.InvokeOnMoveFinish(TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role_Move_Ground>());
                TemporaryStorage.InvokeOnMoveFinish(TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role_Move_Flight>());
            };
            TemporaryStorage.OnCoolDown += CoolDown;
            TemporaryStorage.OnOperating += (b) =>
            {
                operating = b;
            };
        }
        void CoolDown(FightLayer_Roles_Role_Action action, float time)
        {
            if(!action.transform.TryGetComponent(out FightLayer_Roles_Role role))return;
            if(roles.Find(r => r == role) == null)return;
            StartCoroutine(role.Countdown(time));
        }
        void ShowRoleDescription(GameObject go)
        {
            if(!go.TryGetComponent(out FightLayer_Roles_Role_SelectTester selectTest))return;
            temp_unitSimpleDescription = selectTest.GetUnitSimpleDescription();
            TemporaryStorage.InvokeOnShowUnitDescription(go,temp_unitSimpleDescription);
        }
        
    }
}

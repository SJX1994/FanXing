using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Info : MonoBehaviour
    {
        [SerializeField] ScriptableObject_Role_Infomation Role_Infomation;
        [SerializeField] FightLayer_Roles_Role_SelectTester SelectTester;
        public ScriptableObject_Role_Infomation.RoleType RoleType
        {
            get
            {
                return Role_Infomation.roleType;
            }
        }
    }
}


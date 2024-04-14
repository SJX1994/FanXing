using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_ColdDown : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer_ColdDown;
        MaterialPropertyBlock propertyBlock;
        public void PropertyBlock_Progress(float SetFloat , float MaxTime)
        {
            float Normalized_SetFloat = Math.Remap(SetFloat, 0, MaxTime, 0, 1);

            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
            spriteRenderer_ColdDown.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Progress", Normalized_SetFloat);
            spriteRenderer_ColdDown.SetPropertyBlock(propertyBlock);
        }
    }
}


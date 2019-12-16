using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace RoR2Plugin
{
    // TODO: EffectAPI docs
    public static class EffectAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static System.Boolean RegisterNewEffect( GameObject prefab )
        {
            List<GameObject> effects = EffectManager.instance.GetFieldValue<List<GameObject>>("effectPrefabsList");
            Dictionary<GameObject, System.UInt32> effectLookup = EffectManager.instance.GetFieldValue<Dictionary<GameObject, System.UInt32>>("effectPrefabToIndexMap");

            if( !prefab )
            {
                return false;
            }

            System.Int32 index = effects.Count;

            effects.Add( prefab );
            effectLookup.Add( prefab, (System.UInt32)index );
            return true;
        }
    }
}

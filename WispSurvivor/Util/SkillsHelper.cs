using UnityEngine;
using R2API.Utils;
using System;
using System.Collections.Generic;

namespace WispSurvivor.Util
{
    public static class SkillsHelper
    {
        public static bool AddSkill(Type t )
        {
            if(t == null || !t.IsSubclassOf( typeof(EntityStates.EntityState) ) || t.IsAbstract )
            {
                Debug.Log("Type is not based on a State or is null");
                return false;
            }

            var stateTab = typeof(EntityStates.EntityState).Assembly.GetType("EntityStates.StateIndexTable");

            Type[] id2State = stateTab.GetFieldValue<Type[]>("stateIndexToType");
            string[] name2Id = stateTab.GetFieldValue<string[]>("stateIndexToTypeName");
            Dictionary<Type, short> state2Id = stateTab.GetFieldValue<Dictionary<Type, short>>("stateTypeToIndex");
            int ogNum = id2State.Length;

            if (ogNum + 1 > short.MaxValue)
            {
                Debug.LogError("There are more entitystates than there are indexes (max indexes is 32767) Try uninstalling some mods?");
            }

            Array.Resize<Type>(ref id2State, ogNum + 1);
            Array.Resize<string>(ref name2Id, ogNum + 1);
            Debug.Log("SkillType: " + t.FullName + " added to table with index: " + ogNum.ToString());
            id2State[ogNum] = t;
            name2Id[ogNum] = t.FullName;
            state2Id[t] = (short)ogNum;

            stateTab.SetFieldValue<Type[]>("stateIndexToType", id2State);
            stateTab.SetFieldValue<string[]>("stateIndexToTypeName", name2Id);
            stateTab.SetFieldValue<Dictionary<Type, short>>("stateTypeToIndex" , state2Id);

            return true;
        }
    }
}

using R2API.Utils;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Acrid.Helpers
{
    public static class SkillsHelper
    {
        public static Boolean AddSkill( Type t )
        {
            if( t == null || !t.IsSubclassOf( typeof( EntityStates.EntityState ) ) || t.IsAbstract )
            {
                Debug.Log( "Type is not based on a State or is null" );
                return false;
            }
            Type stateTab = typeof(EntityStates.EntityState).Assembly.GetType("EntityStates.StateIndexTable");
            Type[] id2State = stateTab.GetFieldValue<Type[]>("stateIndexToType");
            String[] name2Id = stateTab.GetFieldValue<String[]>("stateIndexToTypeName");
            Dictionary<Type, Int16> state2Id = stateTab.GetFieldValue<Dictionary<Type, Int16>>("stateTypeToIndex");
            Int32 ogNum = id2State.Length;
            if( ogNum + 1 > Int16.MaxValue ) Debug.LogError( "There are more entitystates than there are indexes (max indexes is 32767) Try uninstalling some mods?" );
            Array.Resize<Type>( ref id2State, ogNum + 1 );
            Array.Resize<String>( ref name2Id, ogNum + 1 );
            Debug.Log( "SkillType: " + t.FullName + " added to table with index: " + ogNum.ToString() );
            id2State[ogNum] = t;
            name2Id[ogNum] = t.FullName;
            state2Id[t] = (Int16)ogNum;
            stateTab.SetFieldValue<Type[]>( "stateIndexToType", id2State );
            stateTab.SetFieldValue<String[]>( "stateIndexToTypeName", name2Id );
            stateTab.SetFieldValue<Dictionary<Type, Int16>>( "stateTypeToIndex", state2Id );
            return true;
        }

        public static Boolean AddSkillDef( SkillDef s )
        {
            if( !s ) return false;
            SkillCatalog.getAdditionalSkillDefs += ( list ) =>
            {
                list.Add( s );
                Debug.Log( "SkillDef: " + s.skillNameToken + " added to SkillCatalog" );
            };
            return true;
        }

        public static Boolean AddSkillFamily( SkillFamily sf )
        {
            if( !sf ) return false;
            SkillCatalog.getAdditionalSkillFamilies += ( list ) =>
            {
                list.Add( sf );
                Debug.Log( "SkillFamily added to SkillCatalog" );
            };
            return true;
        }

    }
}

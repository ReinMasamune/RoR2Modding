namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using EntityStates;

    using MonoMod.RuntimeDetour;

    using RoR2;
    using RoR2.Skills;

    using UnityEngine;

    // TODO: Docs for SkillsCore
    // TODO: Userprofile fixes for SkillsCore (If not fixed in next ror2 update)
    /// <summary>
    /// 
    /// </summary>
    public static class SkillsCore
    {
        /// <summary>
        /// 
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public static void AddSkill( Type type )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( SkillsCore ) );
            }

            if( addedSkillTypes.Contains( type ) )
            {
                return;
            }

            if( !type.IsSubclassOf( typeof( EntityState ) ) )
            {
                throw new ArgumentException( "Type must derive from EntityState" );
            }

            if( type.Assembly == ror2Assembly )
            {
                return;
            }

            Type[] idToState = stateIndexToType.Get();
            String[] idToName = stateIndexToTypeName.Get();
            Dictionary<Type, Int16> stateToId = stateTypeToIndex.Get();

            Int32 ind = idToState.Length;
            Array.Resize<Type>( ref idToState, ind + 1 );
            Array.Resize<String>( ref idToName, ind + 1 );
            idToState[ind] = type;
            idToName[ind] = type.AssemblyQualifiedName;
            stateToId[type] = (Int16)ind;

            stateIndexToType.Set( idToState );
            stateIndexToTypeName.Set( idToName );

            _ = addedSkillTypes.Add( type );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillDef"></param>
        public static void AddSkillDef( SkillDef skillDef )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( SkillsCore ) );
            }

            if( addedSkillDefs.Contains( skillDef ) )
            {
                return;
            }

            if( skillDef == null )
            {
                throw new ArgumentNullException( "skillDef" );
            }

            SkillCatalog.getAdditionalSkillDefs += ( list ) => list.Add( skillDef );
            _ = addedSkillDefs.Add( skillDef );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillFamily"></param>
        public static void AddSkillFamily( SkillFamily skillFamily )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( SkillsCore ) );
            }

            if( addedSkillFamilies.Contains( skillFamily ) )
            {
                return;
            }

            if( skillFamily == null )
            {
                throw new ArgumentNullException( "skillFamily" );
            }

            SkillCatalog.getAdditionalSkillFamilies += ( list ) => list.Add( skillFamily );
            _ = addedSkillFamilies.Add( skillFamily );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="register"></param>
        /// <returns></returns>
        public static SerializableEntityStateType StateType<TState>( Boolean register = true ) where TState : EntityState
        {
            if( register )
            {
                AddSkill( typeof( TState ) );
            }

            return new SerializableEntityStateType( typeof( TState ) );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static SkillFamily CreateSkillFamily( SkillDef defaultSkill, params (SkillDef skill, String unlockable)[] variants )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            family.variants = new SkillFamily.Variant[variants.Length + 1];
            family.variants[0] = new SkillFamily.Variant
            {
                skillDef = defaultSkill,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node( defaultSkill.skillName, false ),
            };
            AddSkillDef( defaultSkill );

            for( Int32 i = 0; i < variants.Length; ++i )
            {
                (SkillDef skill, String unlockable) info = variants[i];
                SkillDef skill = info.skill;
                family.variants[i + 1] = new SkillFamily.Variant
                {
                    skillDef = skill,
                    unlockableName = info.unlockable,
                    viewableNode = new ViewablesCatalog.Node( skill.skillName, false ),
                };
                AddSkillDef( skill );
            }

            AddSkillFamily( family );
            return family;
        }

        /// <summary>
        /// Accessor for GenericSkill.SkillFamily
        /// </summary>
        public static Accessor<GenericSkill, SkillFamily> skillFamily { get; } = new Accessor<GenericSkill, SkillFamily>( "_skillFamily" );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static SkillFamily GetSkillFamily( this GenericSkill skill )
        {
            if( skill == null )
            {
                throw new ArgumentNullException( nameof( skill ) );
            }

            return skillFamily.Get( skill );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="family"></param>
        public static void SetSkillFamily( this GenericSkill skill, SkillFamily family ) => skillFamily.Set( skill, family );


        static SkillsCore()
        {
            ror2Assembly = typeof( EntityState ).Assembly;
            Type stateTableType = ror2Assembly.GetType("EntityStates.StateIndexTable");
            stateIndexToType = new StaticAccessor<Type[]>( stateTableType, "stateIndexToType" );
            stateIndexToTypeName = new StaticAccessor<String[]>( stateTableType, "stateIndexToTypeName" );
            stateTypeToIndex = new StaticAccessor<Dictionary<Type, Int16>>( stateTableType, "stateTypeToIndex" );

            Type type = typeof(SerializableEntityStateType);
            HookConfig cfg = default;
            cfg.Priority = Int32.MinValue;
            set_stateTypeHook = new Hook( type.GetMethod( "set_stateType", allFlags ), set_stateType, cfg );
            set_typeNameHook = new Hook( type.GetMethod( "set_typeName", allFlags ), set_typeName, cfg );
            loaded = true;
        }
        private static readonly Assembly ror2Assembly;
        private static readonly HashSet<Type> addedSkillTypes = new HashSet<Type>();
        private static readonly HashSet<SkillDef> addedSkillDefs = new HashSet<SkillDef>();
        private static readonly HashSet<SkillFamily> addedSkillFamilies = new HashSet<SkillFamily>();
        private static readonly StructAccessor<SerializableEntityStateType,String> typeName = new StructAccessor<SerializableEntityStateType, String>( "_typeName" );
        private static readonly StaticAccessor<Type[]> stateIndexToType;
        private static readonly StaticAccessor<String[]> stateIndexToTypeName;
        private static readonly StaticAccessor<Dictionary<Type,Int16>> stateTypeToIndex;
        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly Dictionary<String,Int16> nameToIndexCache = new Dictionary<String, Int16>();
        private static readonly Hook set_stateTypeHook;
        private static readonly Hook set_typeNameHook;
#pragma warning disable IDE1006 // Naming Styles
        private delegate void set_stateTypeDelegate( ref SerializableEntityStateType self, Type value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private delegate void set_typeNameDelegate( ref SerializableEntityStateType self, String value );
#pragma warning restore IDE1006 // Naming Styles


        private static readonly set_stateTypeDelegate set_stateType = new set_stateTypeDelegate( (ref SerializableEntityStateType self, Type value ) =>
        {
            if( value == null )
            {
                Log.Error( "Tried to set SerializableEntityStateType with a null type" );
                return;
            }
            Dictionary<Type, Int16> typeToId = stateTypeToIndex.Get();
            if( !typeToId.ContainsKey( value ) )
            {
                String name = value.AssemblyQualifiedName;
                if( value.IsSubclassOf( typeof(EntityState) ) )
                {
                    Log.Warning( String.Format("Unregistered type:\n{0}\nfound in SerializableEntityStateType, performing registration now.", name ) );
                    AddSkill( value );
                    if( !typeToId.ContainsKey( value ) )
                    {
                        Log.Error( String.Format( "Unable to register type:\n{0}", name ) );
                        return;
                    }
                } else
                {
                    Log.Error( String.Format("Tried to create SerializableEntityStateType for invalid type:\n{0}", name ));
                    return;
                }
            }
            typeName.Set( ref self, stateIndexToTypeName.Get()[typeToId[value]] );
        });
        private static readonly set_typeNameDelegate set_typeName = new set_typeNameDelegate( (ref SerializableEntityStateType self, String value ) =>
        {
            Type t = GetTypeFromName( value );
            if( t != null )
            {
                set_stateType( ref self, t );
            }
        });

        private static Type GetTypeFromName( String name )
        {
            Type[] types = stateIndexToType.Get();
            if( nameToIndexCache.ContainsKey( name ) )
            {
                return types[nameToIndexCache[name]];
            } else
            {
                RebuildNameToIndexCache();

                if( nameToIndexCache.ContainsKey( name ) )
                {
                    return types[nameToIndexCache[name]];
                } else
                {
                    Log.Error( String.Format( "Could not find type for name:\n{0}", name ) );
                    return null;
                }
            }
        }

        private static void RebuildNameToIndexCache()
        {
            String[] names = stateIndexToTypeName.Get();
            for( Int16 i = 0; i < names.Length; ++i )
            {
                nameToIndexCache[names[i]] = i;
            }
        }
    }
}

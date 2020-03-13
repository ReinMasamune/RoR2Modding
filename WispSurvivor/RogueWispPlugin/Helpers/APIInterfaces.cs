#if R2API
using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R2API;
using UnityEngine;

namespace GeneralPluginStuff
{
    internal static class APIInterfaces
    {
        const BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

        public static DirectorCard GetCard( this DirectorAPI.DirectorCardHolder holder )
        {
            return directorAPI_card?.GetValue( holder );
        }

        public static void SetCard( this DirectorAPI.DirectorCardHolder holder, DirectorCard card )
        {
            directorAPI_card?.SetValue( holder, card );
        }

        public static DirectorAPI.MonsterCategory GetMonsterCategory( this DirectorAPI.DirectorCardHolder holder )
        {
            if( directorAPI_monsterCat != null )
            {
                return directorAPI_monsterCat.GetValue( holder );
            } else return default;
        }

        public static void SetMonsterCategory( this DirectorAPI.DirectorCardHolder holder, DirectorAPI.MonsterCategory monstCat )
        {
            directorAPI_monsterCat?.SetValue( holder, monstCat );
        }

        public static DirectorAPI.InteractableCategory GetInteractableCategory( this DirectorAPI.DirectorCardHolder holder )
        {
            if( directorAPI_interactableCat != null )
            {
                return directorAPI_interactableCat.GetValue( holder );
            } else return default;
        }

        public static void SetInteractableCategory( this DirectorAPI.DirectorCardHolder holder, DirectorAPI.InteractableCategory interCat )
        {
            directorAPI_interactableCat?.SetValue( holder, interCat );
        }

        public static SkinDef[] GetBaseSkins( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinBaseSkins?.GetValue( info );
        }

        public static void SetBaseSkins( this LoadoutAPI.SkinDefInfo info, SkinDef[] skins )
        {
            loadoutAPI_skinBaseSkins?.SetValue( info, skins );
        }

        public static SkinDef.GameObjectActivation[] GetObjectActivations( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinObjectActivations?.GetValue( info );
        }

        public static void SetObjectActivations( this LoadoutAPI.SkinDefInfo info, SkinDef.GameObjectActivation[] activs )
        {
            loadoutAPI_skinObjectActivations?.SetValue( info, activs );
        }

        public static Sprite GetIcon( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinIcon.GetValue( info );
        }

        public static void SetIcon( this LoadoutAPI.SkinDefInfo info, Sprite sprite )
        {
            loadoutAPI_skinIcon?.SetValue( info, sprite );
        }

        public static SkinDef.MeshReplacement[] GetMeshReplacements( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinMeshReplacements?.GetValue( info );
        }

        public static void SetMeshReplacements( this LoadoutAPI.SkinDefInfo info, SkinDef.MeshReplacement[] meshes )
        {
            loadoutAPI_skinMeshReplacements?.SetValue( info, meshes );
        }

        public static String GetName( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinName.GetValue( info );
        }

        public static void SetName( this LoadoutAPI.SkinDefInfo info, String name )
        {
            loadoutAPI_skinName?.SetValue( info, name );
        }

        public static String GetNameToken( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinNameToken?.GetValue( info );
        }

        public static void SetNameToken( this LoadoutAPI.SkinDefInfo info, String name )
        {
            loadoutAPI_skinNameToken?.SetValue( info, name );
        }

        public static CharacterModel.RendererInfo[] GetRendererInfos( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinRendererInfos?.GetValue( info );
        }

        public static void SetRendererInfos( this LoadoutAPI.SkinDefInfo info, CharacterModel.RendererInfo[] infos )
        {
            loadoutAPI_skinRendererInfos?.SetValue( info, infos );
        }

        public static GameObject GetRootObject( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinRootObject?.GetValue( info );
        }

        public static void SetRootObject( this LoadoutAPI.SkinDefInfo info, GameObject root )
        {
            loadoutAPI_skinRootObject?.SetValue( info, root );
        }

        public static String GetUnlockableName( this LoadoutAPI.SkinDefInfo info )
        {
            return loadoutAPI_skinUnlockableName?.GetValue( info );
        }

        public static void SetUnlockableName( this LoadoutAPI.SkinDefInfo info, String name )
        {
            loadoutAPI_skinUnlockableName?.SetValue( info, name );
        }

        private static Accessor<DirectorAPI.DirectorCardHolder,DirectorCard> directorAPI_card;
        private static Accessor<DirectorAPI.DirectorCardHolder,DirectorAPI.InteractableCategory> directorAPI_interactableCat;
        private static Accessor<DirectorAPI.DirectorCardHolder,DirectorAPI.MonsterCategory> directorAPI_monsterCat;

        private static Accessor<LoadoutAPI.SkinDefInfo,SkinDef[]> loadoutAPI_skinBaseSkins;
        private static Accessor<LoadoutAPI.SkinDefInfo,SkinDef.GameObjectActivation[]> loadoutAPI_skinObjectActivations;
        private static Accessor<LoadoutAPI.SkinDefInfo,Sprite> loadoutAPI_skinIcon;
        private static Accessor<LoadoutAPI.SkinDefInfo,SkinDef.MeshReplacement[]> loadoutAPI_skinMeshReplacements;
        private static Accessor<LoadoutAPI.SkinDefInfo,String> loadoutAPI_skinName;
        private static Accessor<LoadoutAPI.SkinDefInfo,String> loadoutAPI_skinNameToken;
        private static Accessor<LoadoutAPI.SkinDefInfo,CharacterModel.RendererInfo[]> loadoutAPI_skinRendererInfos;
        private static Accessor<LoadoutAPI.SkinDefInfo,GameObject> loadoutAPI_skinRootObject;
        private static Accessor<LoadoutAPI.SkinDefInfo,String> loadoutAPI_skinUnlockableName;
        static APIInterfaces()
        {
            HashSet<String> temp = new HashSet<String>();

            temp.Add( "card" );
            temp.Add( "Card" );
            directorAPI_card = new Accessor<DirectorAPI.DirectorCardHolder, DirectorCard>( temp );
            temp.Clear();

            temp.Add( "interactableCategory" );
            temp.Add( "InteractableCategory" );
            directorAPI_interactableCat = new Accessor<DirectorAPI.DirectorCardHolder, DirectorAPI.InteractableCategory>( temp );
            temp.Clear();

            temp.Add( "monsterCategory" );
            temp.Add( "MonsterCategory" );
            directorAPI_monsterCat = new Accessor<DirectorAPI.DirectorCardHolder, DirectorAPI.MonsterCategory>( temp );
            temp.Clear();

            temp.Add( "baseSkins" );
            temp.Add( "BaseSkins" );
            loadoutAPI_skinBaseSkins = new Accessor<LoadoutAPI.SkinDefInfo, SkinDef[]>( temp );
            temp.Clear();

            temp.Add( "gameObjectActivations" );
            temp.Add( "GameObjectActivations" );
            loadoutAPI_skinObjectActivations = new Accessor<LoadoutAPI.SkinDefInfo, SkinDef.GameObjectActivation[]>( temp );
            temp.Clear();

            temp.Add( "icon" );
            temp.Add( "Icon" );
            loadoutAPI_skinIcon = new Accessor<LoadoutAPI.SkinDefInfo, Sprite>( temp );
            temp.Clear();

            temp.Add( "meshReplacements" );
            temp.Add( "MeshReplacements" );
            loadoutAPI_skinMeshReplacements = new Accessor<LoadoutAPI.SkinDefInfo, SkinDef.MeshReplacement[]>( temp );
            temp.Clear();

            temp.Add( "name" );
            temp.Add( "Name" );
            loadoutAPI_skinName = new Accessor<LoadoutAPI.SkinDefInfo, String>( temp );
            temp.Clear();

            temp.Add( "nameToken" );
            temp.Add( "NameToken" );
            loadoutAPI_skinNameToken = new Accessor<LoadoutAPI.SkinDefInfo, String>( temp );
            temp.Clear();

            temp.Add( "rendererInfos" );
            temp.Add( "RendererInfos" );
            loadoutAPI_skinRendererInfos = new Accessor<LoadoutAPI.SkinDefInfo, CharacterModel.RendererInfo[]>( temp );
            temp.Clear();

            temp.Add( "rootObject" );
            temp.Add( "RootObject" );
            loadoutAPI_skinRootObject = new Accessor<LoadoutAPI.SkinDefInfo, GameObject>( temp );
            temp.Clear();

            temp.Add( "unlockableName" );
            temp.Add( "UnlockableName" );
            loadoutAPI_skinUnlockableName = new Accessor<LoadoutAPI.SkinDefInfo, String>( temp );
            temp.Clear();
        } 
        private class Accessor<TObj, TVal>
        {
            public TVal GetValue( TObj obj )
            {
                if( this.getFunc != null ) return this.getFunc( obj );
                return default;
            }
            public void SetValue( TObj obj, TVal value )
            {
                if( this.setAct == null ) return;
                this.setAct( obj, value );
            }

            private Func<TObj,TVal> getFunc;
            private Action<TObj,TVal> setAct;

            public Accessor( HashSet<String> possibleNames )
            {
                this.getFunc = CreateReader<TObj, TVal>( typeof(TObj), possibleNames )?.Compile();
                this.setAct = CreateWriter<TObj, TVal>( typeof(TObj), possibleNames )?.Compile();
            }
        }
        private static Expression<Action<TObj,TVal>> CreateWriter<TObj,TVal>( Type type, HashSet<String> names )
        {
            var method = type.CheckMethod( names );
            var property = type.CheckProperty( names );
            var field = type.CheckField( names );

            var param1 = Expression.Parameter( typeof( TObj ), "Object" );
            var param2 = Expression.Parameter( typeof( TVal ), "Value" );

            if( method == null )
            {
                if( property != null )
                {
                    method = property.SetMethod;
                } else if( field != null )
                {
                    var fExp = Expression.Field( param1, field );
                    return Expression.Lambda<Action<TObj, TVal>>( Expression.Assign( fExp, param2 ), param1, param2 );
                } else
                {
                    return null;
                }
            }

            return Expression.Lambda<Action<TObj, TVal>>( Expression.Call( param1, method, param2 ) );
        }
        private static Expression<Func<TObj,TVal>> CreateReader<TObj,TVal>( Type type, HashSet<String> names )
        {
            var method = type.CheckMethod( names );
            var property = type.CheckProperty( names );
            var field = type.CheckField( names );
            var param = Expression.Parameter( typeof( TObj ), "Object" );
            if( method == null )
            {
                if( property != null )
                {
                    method = property.GetMethod;
                } else if( field != null )
                {
                    var p2 = Expression.Field( param, field );
                    return Expression.Lambda<Func<TObj, TVal>>( p2, param );
                } else
                {
                    return null;
                }
            }


            return Expression.Lambda<Func<TObj, TVal>>( Expression.Call( param, method ) );
        }
        private static Expression<TMethod> CreateMethodWrapper<TMethod>( Type type, HashSet<String> names )
        {
            var method = type.CheckMethod( names );
            if( method != null )
            {
                var paramInfos = method.GetParameters();

                Int32 startIndex = 0;

                ParameterExpression instanceParameter = null;
                if( !method.IsStatic )
                {
                    var curParam = paramInfos[startIndex++];
                    instanceParameter = Expression.Parameter( curParam.ParameterType, curParam.Name );
                }
                var parameters = new ParameterExpression[paramInfos.Length - startIndex];
                for( Int32 i = startIndex; i < paramInfos.Length; ++i )
                {
                    var curParam = paramInfos[i];
                    parameters[i - startIndex] = Expression.Parameter( curParam.ParameterType, curParam.Name );
                }

                return Expression.Lambda<TMethod>( Expression.Call( (instanceParameter == null ? Expression.Constant( null ) as Expression : instanceParameter as Expression), method, parameters) );
            }
            return null;
        }
        private static FieldInfo CheckField( this Type type, HashSet<String> names )
        {
            FieldInfo ret = null;
            foreach( String name in names )
            {
                ret = type.GetField( name, allFlags );
                if( ret != null ) break;
            }
            return ret;
        }
        private static PropertyInfo CheckProperty( this Type type, HashSet<String> names )
        {
            PropertyInfo ret = null;
            foreach( String name in names )
            {
                ret = type.GetProperty( name, allFlags );
                if( ret != null ) break;
            }
            return ret;
        }
        private static MethodInfo CheckMethod( this Type type, HashSet<String> names )
        {
            MethodInfo ret = null;
            foreach( String name in names )
            {
                ret = type.GetMethod( name, allFlags );
                if( ret != null ) break;
            }
            return ret;
        }
    }
}
#endif
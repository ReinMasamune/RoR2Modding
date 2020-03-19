#if MATEDITOR
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal abstract class PropertyWrapper<TMenu>
    {
        static PropertyWrapper()
        {

        }

        internal static PropertyWrapper<TMenu> CreatePropertyWrapper( PropertyInfo property, MenuAttribute settings )
        {
            Type type = property.PropertyType;

            if( type == typeof( Single ) )
            {
                return new SingleWrapper<TMenu>( property, settings );
            } else if( type == typeof( Boolean ) )
            {
                return new BooleanWrapper<TMenu>( property, settings );
            } else if( type == typeof( Int32 ) )
            {
                return new Int32Wrapper<TMenu>( property, settings );
            } else if( type == typeof( Vector2 ) )
            {
                return new Vector2Wrapper<TMenu>( property, settings );
            } else if( type == typeof( Vector3 ) )
            {
                return new Vector3Wrapper<TMenu>( property, settings );
            } else if( type == typeof( Vector4 ) )
            {
                return new Vector4Wrapper<TMenu>( property, settings );
            } else if( type == typeof( Color ) )
            {
                return new ColorWrapper<TMenu>( property, settings );
            } else if( type == typeof( MaterialBase.TextureData ) )
            {
                return new TextureWrapper<TMenu>( property, settings );
            } else if( type == typeof( MaterialBase.ScaleOffsetTextureData ) )
            {
                return new TextureWrapper<TMenu>( property, settings );
            } else if( type == typeof( EliteIndex ) )
            {
                return new EnumWrapper<TMenu, EliteIndex>( property, settings );
            } else if( type == typeof( StandardMaterial.RampInfo ) )
            {
                return new EnumWrapper<TMenu, StandardMaterial.RampInfo>( property, settings );
            } else if( type == typeof( StandardMaterial.DecalLayer ) )
            {
                return new EnumWrapper<TMenu, StandardMaterial.DecalLayer>( property, settings );
            } else if( type == typeof( StandardMaterial.CullMode ) )
            {
                return new EnumWrapper<TMenu, StandardMaterial.CullMode>( property, settings );
            } else if( type == typeof( StandardMaterial.PrintDirection ) )
            {
                return new EnumWrapper<TMenu, StandardMaterial.PrintDirection>( property, settings );
            } else if( type.BaseType == typeof( Enum ) )
            {
                var t = typeof(EnumWrapper<,>).MakeGenericType( typeof(TMenu), type );
                var constructor = t.GetConstructor( BindingFlags.Instance | BindingFlags.NonPublic, default, new Type[]
                {
                    typeof(PropertyInfo),
                    typeof(MenuAttribute),
                }, default );
                return constructor.Invoke( new System.Object[]
                {
                    property,
                    settings
                } ) as PropertyWrapper<TMenu>;
            } else
            {
                Main.LogE( "Unhandled type:" + type.ToString() );
                return null;
            }

            return null;
        }

        internal Func<TMenu,TValue> CompileGetter<TValue>( MethodInfo method )
        {
            var instanceParameter = Expression.Parameter( typeof(TMenu), "instance" );
            var body = Expression.Call( instanceParameter, method );
            var lambda = Expression.Lambda<Func<TMenu,TValue>>(body, instanceParameter);
            return lambda.Compile();
        }

        internal Action<TMenu,TValue> CompileSetter<TValue>( MethodInfo method )
        {
            var instanceParameter = Expression.Parameter( typeof(TMenu), "instance" );
            var valueParameter = Expression.Parameter( typeof(TValue), "value");
            var body = Expression.Call( instanceParameter, method, valueParameter );
            var lambda = Expression.Lambda<Action<TMenu,TValue>>( body, instanceParameter, valueParameter );
            return lambda.Compile();
        }


        internal abstract void Draw( TMenu instance );
        
    }
}
#endif
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public class Accessor<TValue>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Accessor( Type instanceType, String name )
        {
            var instanceParam = Expression.Parameter(typeof(System.Object), "instance" );
            var instanceConv = Expression.Convert( instanceParam, instanceType );
            var valueParam = Expression.Parameter( typeof( TValue ), "value" );
            var type = instanceType;
            var allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var memberArray = type.GetMember( name, MemberTypes.Property | MemberTypes.Field, allFlags );
            MemberInfo member = null;
            if( memberArray.Length == 1 )
            {
                member = memberArray[0];
            } else
            {
                MemberInfo fieldMem = null;
                MemberInfo propMem = null;
                for( Int32 i = 0; i < memberArray.Length; ++i )
                {
                    var mem = memberArray[i];
                    if( mem.MemberType == MemberTypes.Property )
                    {
                        propMem = mem;
                    }
                    if( mem.MemberType == MemberTypes.Field )
                    {
                        fieldMem = mem;
                    }
                }
                if( fieldMem != null )
                {
                    member = fieldMem;
                } else if( propMem != null )
                {
                    member = propMem;
                } else
                {
                    throw new MissingMemberException( type.AssemblyQualifiedName, name );
                }
            }
            var memberType = member.GetType();
            if( member.MemberType == MemberTypes.Field )
            {
                var field = member as FieldInfo;
                var info = memberType.GetField( "attrs", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
                var val = (FieldAttributes)info.GetValue( member );
                var temp = val;
                val &= ~FieldAttributes.InitOnly;
                info.SetValue( member, val );

                var fieldExpr = Expression.Field( instanceParam, field );
                var assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.Set = Expression.Lambda<AccessorSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                this.Get = Expression.Lambda<AccessorGetDelegate>( fieldExpr, instanceParam ).Compile();
                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                var propExpr = Expression.Property( instanceParam, prop );
                if( prop.CanWrite )
                {
                    var assignExpr = Expression.Assign( propExpr, valueParam );
                    this.Set = Expression.Lambda<AccessorSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                }
                if( prop.CanRead )
                {
                    this.Get = Expression.Lambda<AccessorGetDelegate>( propExpr, instanceParam ).Compile();
                }
            } else
            {
                throw new MissingMemberException( type.AssemblyQualifiedName, name );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccessorGetDelegate Get { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public AccessorSetDelegate Set { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate TValue AccessorGetDelegate( System.Object instance );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void AccessorSetDelegate( System.Object, TValue value );
    }

}

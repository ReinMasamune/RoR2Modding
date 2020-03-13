using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticAccessor<TValue>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public StaticAccessor( Type type, String name )
        {
            var valueParam = Expression.Parameter( typeof(TValue), "value" );        
            var allFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
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

                var fieldExpr = Expression.Field(null,field);
                var assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.Set = Expression.Lambda<StaticAccessorSetDelegate>( assignExpr, valueParam ).Compile();
                this.Get = Expression.Lambda<StaticAccessorGetDelegate>( fieldExpr ).Compile();
                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                var propExpr = Expression.Property(null, prop );
                if( prop.CanWrite )
                {
                    var assignExpr = Expression.Assign( propExpr, valueParam );
                    this.Set = Expression.Lambda<StaticAccessorSetDelegate>( assignExpr, valueParam ).Compile();
                }
                if( prop.CanRead )
                {
                    this.Get = Expression.Lambda<StaticAccessorGetDelegate>( propExpr ).Compile();
                }
            } else
            {
                throw new MissingMemberException( type.AssemblyQualifiedName, name );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public StaticAccessorGetDelegate Get;
        /// <summary>
        /// 
        /// </summary>
        public StaticAccessorSetDelegate Set;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate TValue StaticAccessorGetDelegate();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public delegate void StaticAccessorSetDelegate( TValue value );
    }

}

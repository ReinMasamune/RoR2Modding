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
    public class StructAccessor<TInstance, TValue> where TInstance : struct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public StructAccessor( String name )
        {
            var miscLabel = Expression.Label();
            var instanceParam = Expression.Parameter(typeof(System.Object), "instance" );
            var unboxedInstance = Expression.Unbox( instanceParam, typeof(TInstance) );
            var valueParam = Expression.Parameter( typeof( TValue ), "value" );
            var type = typeof( TInstance );
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


                var fieldExpr = Expression.Field( unboxedInstance, field );
                var assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.internalSet = Expression.Lambda<StructInternalSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                this.internalGet = Expression.Lambda<StructInternalGetDelegate>( fieldExpr, instanceParam ).Compile();

                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                var propExpr = Expression.Property( unboxedInstance, prop );
                if( prop.CanWrite )
                {
                    var assignExpr = Expression.Assign( propExpr, valueParam );
                    this.internalSet = Expression.Lambda<StructInternalSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                }
                if( prop.CanRead )
                {
                    this.internalGet = Expression.Lambda<StructInternalGetDelegate>( propExpr, instanceParam ).Compile();
                }
            } else
            {
                throw new MissingMemberException( type.AssemblyQualifiedName, name );
            }






            if( this.internalGet != null )
            {
                this.Get = new StructAccessorGetDelegate( ( ref TInstance instance ) =>
                {
                    System.Object inst = instance;
                    var val = this.internalGet( inst );
                    instance = (TInstance)inst;
                    return val;
                } );
            }

            if( this.internalSet != null )
            {
                this.Set = new StructAccessorSetDelegate( ( ref TInstance instance, TValue value ) =>
                {
                    System.Object inst = instance;
                    this.internalSet( inst, value );
                    instance = (TInstance)inst;
                } );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public StructAccessorGetDelegate Get;
        /// <summary>
        /// 
        /// </summary>
        public StructAccessorSetDelegate Set;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate TValue StructAccessorGetDelegate( ref TInstance instance );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public delegate void StructAccessorSetDelegate( ref TInstance instance, TValue value );


        private delegate TValue StructInternalGetDelegate( System.Object instance );
        private delegate void StructInternalSetDelegate( System.Object instance, TValue value );
        private StructInternalGetDelegate internalGet;
        private StructInternalSetDelegate internalSet;
    }

}

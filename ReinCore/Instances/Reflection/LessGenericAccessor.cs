namespace ReinCore
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class Accessor<TValue>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="name"></param>
        public Accessor( Type instanceType, String name )
        {
            ParameterExpression instanceParam = Expression.Parameter(typeof(System.Object), "instance" );
            UnaryExpression instanceConv = Expression.Convert( instanceParam, instanceType );
            ParameterExpression valueParam = Expression.Parameter( typeof( TValue ), "value" );
            Type type = instanceType;
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            MemberInfo[] memberArray = type.GetMember( name, MemberTypes.Property | MemberTypes.Field, allFlags );
            MemberInfo member;
            if( memberArray.Length == 1 )
            {
                member = memberArray[0];
            } else
            {
                MemberInfo fieldMem = null;
                MemberInfo propMem = null;
                for( Int32 i = 0; i < memberArray.Length; ++i )
                {
                    MemberInfo mem = memberArray[i];
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
            Type memberType = member.GetType();
            if( member.MemberType == MemberTypes.Field )
            {
                var field = member as FieldInfo;
                FieldInfo info = memberType.GetField( "attrs", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
                var val = (FieldAttributes)info.GetValue( member );
                FieldAttributes temp = val;
                val &= ~FieldAttributes.InitOnly;
                info.SetValue( member, val );

                MemberExpression fieldExpr = Expression.Field( instanceConv, field );
                BinaryExpression assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.Set = Expression.Lambda<AccessorSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                this.Get = Expression.Lambda<AccessorGetDelegate>( fieldExpr, instanceParam ).Compile();
                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                MemberExpression propExpr = Expression.Property( instanceConv, prop );
                if( prop.CanWrite )
                {
                    BinaryExpression assignExpr = Expression.Assign( propExpr, valueParam );
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
#pragma warning disable IDE1006 // Naming Styles
        public AccessorGetDelegate Get { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// 
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public AccessorSetDelegate Set { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

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
        public delegate void AccessorSetDelegate( System.Object instance, TValue value );
    }

}

namespace ReinCore
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A wrapper for a get and set method on a particular class. Only works on Mono
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance you are accessing a field of</typeparam>
    /// <typeparam name="TValue">The type of the member you are accessing</typeparam>
    [Obsolete("unneeded",true)]
    public class Accessor<TInstance, TValue> where TInstance : class
    {
        /// <summary>
        /// Creates an accessor for a member of the type
        /// </summary>
        /// <param name="name">The name of the member</param>
        public Accessor( String name )
        {
            ParameterExpression instanceParam = Expression.Parameter(typeof(TInstance), "instance" );
            ParameterExpression valueParam = Expression.Parameter( typeof( TValue ), "value" );
            Type type = typeof( TInstance );
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

                MemberExpression fieldExpr = Expression.Field( instanceParam, field );
                BinaryExpression assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.Set = Expression.Lambda<AccessorSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                this.Get = Expression.Lambda<AccessorGetDelegate>( fieldExpr, instanceParam ).Compile();
                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                MemberExpression propExpr = Expression.Property( instanceParam, prop );
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
        /// The Get method for the accessed member
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public AccessorGetDelegate Get { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The Set method for the accessed member
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public AccessorSetDelegate Set { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The siguature of the Get method
        /// </summary>
        /// <param name="instance">The instance to get from</param>
        /// <returns></returns>
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate TValue AccessorGetDelegate( TInstance instance );

        /// <summary>
        /// The signature of the Set method
        /// </summary>
        /// <param name="instance">The instance to set</param>
        /// <param name="value">The value to set</param>
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void AccessorSetDelegate( TInstance instance, TValue value );
    }

}

namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    [Obsolete( "unneeded", true )]
    public class StaticAccessor<TValue>
    {
        public StaticAccessor( Type type, String name )
        {
            ParameterExpression valueParam = Expression.Parameter( typeof(TValue), "value" );
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
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

                MemberExpression fieldExpr = Expression.Field(null,field);
                BinaryExpression assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.Set = Expression.Lambda<StaticAccessorSetDelegate>( assignExpr, valueParam ).Compile();
                this.Get = Expression.Lambda<StaticAccessorGetDelegate>( fieldExpr ).Compile();
                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                MemberExpression propExpr = Expression.Property(null, prop );
                if( prop.CanWrite )
                {
                    BinaryExpression assignExpr = Expression.Assign( propExpr, valueParam );
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
#pragma warning disable IDE1006 // Naming Styles
        public StaticAccessorGetDelegate Get;
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public StaticAccessorSetDelegate Set;
#pragma warning restore IDE1006 // Naming Styles

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

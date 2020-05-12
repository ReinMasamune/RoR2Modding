namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class StructAccessor<TInstance, TValue> where TInstance : struct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StructAccessor( String name )
        {
            LabelTarget miscLabel = Expression.Label();
            ParameterExpression instanceParam = Expression.Parameter(typeof(System.Object), "instance" );
            UnaryExpression unboxedInstance = Expression.Unbox( instanceParam, typeof(TInstance) );
            ParameterExpression valueParam = Expression.Parameter( typeof( TValue ), "value" );
            Type type = typeof( TInstance );
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            MemberInfo[] memberArray = type.GetMember( name, MemberTypes.Property | MemberTypes.Field, allFlags );
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


                MemberExpression fieldExpr = Expression.Field( unboxedInstance, field );
                BinaryExpression assignExpr = Expression.Assign( fieldExpr, valueParam );
                this.internalSet = Expression.Lambda<StructInternalSetDelegate>( assignExpr, instanceParam, valueParam ).Compile();
                this.internalGet = Expression.Lambda<StructInternalGetDelegate>( fieldExpr, instanceParam ).Compile();

                info.SetValue( member, temp );
            } else if( member.MemberType == MemberTypes.Property )
            {
                var prop = member as PropertyInfo;
                MemberExpression propExpr = Expression.Property( unboxedInstance, prop );
                if( prop.CanWrite )
                {
                    BinaryExpression assignExpr = Expression.Assign( propExpr, valueParam );
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
                    TValue val = this.internalGet( inst );
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
#pragma warning disable IDE1006 // Naming Styles
        public StructAccessorGetDelegate Get;
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public StructAccessorSetDelegate Set;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate TValue StructAccessorGetDelegate( ref TInstance instance );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public delegate void StructAccessorSetDelegate( ref TInstance instance, TValue value );


        private delegate TValue StructInternalGetDelegate( System.Object instance );
        private delegate void StructInternalSetDelegate( System.Object instance, TValue value );
        private readonly StructInternalGetDelegate internalGet;
        private readonly StructInternalSetDelegate internalSet;
    }

}

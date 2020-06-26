namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public struct JobDelegate<TDelegate> where TDelegate : System.Delegate
    {
        public JobDelegate( TDelegate function )
        {
            if( function == null )
            {
                throw new ArgumentNullException( nameof( function ) );
            }

            if( function.Target != null )
            {
                throw new ArgumentException( "Must be a static method", nameof( function ) );
            }

            MethodInfo method = function.Method;
            if( method == null )
            {
                throw new ArgumentNullException( nameof( function ) );
            }

            if( !method.IsStatic )
            {
                throw new ArgumentNullException( "Must be a static method", nameof( function ) );
            }

            this.methodHandle = method.MethodHandle;



        }
        public System.Object Invoke( params System.Object[] args ) => FastInvoker.Invoker( this.methodHandle, null, args ).result;
        private readonly RuntimeMethodHandle methodHandle;
    }

    internal static class FastInvoker
    {
        internal static void Initialize()
        {
            if( !initialized )
            {
                Type monoMethod = MethodBase.GetCurrentMethod().GetType();
                MethodInfo method = monoMethod.GetMethod("InternalInvoke", BindingFlags.NonPublic | BindingFlags.Instance );

                ConstructorInfo monoMethodConstructor = monoMethod.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, default, new[]{ typeof(RuntimeMethodHandle) }, default);

                MemberInfo exceptionMember = typeof(InvokerResult).GetMember("exception", MemberTypes.Method, BindingFlags.NonPublic | BindingFlags.Instance )[0];
                MemberInfo retMember = typeof(InvokerResult).GetMember("ret", MemberTypes.Method, BindingFlags.NonPublic | BindingFlags.Instance )[0];

                ParameterExpression instanceParam = Expression.Parameter(typeof(RuntimeMethodHandle), "handle" );
                ParameterExpression argsParam = Expression.Parameter(typeof(System.Object[]), "args" );
                ParameterExpression exceptionOut = Expression.Variable( typeof(Exception), "exception" );
                _ = Expression.Variable( typeof( System.Object ), "result" );
                NewExpression newMonoMethod = Expression.New(monoMethodConstructor, instanceParam );
                MethodCallExpression call = Expression.Call(newMonoMethod,method,argsParam,exceptionOut);
                NewExpression newExpr = Expression.New( typeof(InvokerResult));
                MemberAssignment resultBind = Expression.Bind( retMember, call );
                MemberAssignment exceptionBind = Expression.Bind( exceptionMember, exceptionOut );
                MemberInitExpression memberInit = Expression.MemberInit(newExpr,resultBind,exceptionBind);

                Invoker = Expression.Lambda<InvokerDelegate>( memberInit, instanceParam, argsParam ).Compile();
                initialized = true;
            }
        }
        internal delegate InvokerResult InvokerDelegate( RuntimeMethodHandle handle, System.Object instance, System.Object[] args );
        internal static InvokerDelegate Invoker { get; private set; }

        internal struct InvokerResult
        {
            internal System.Object result
            {
                get
                {
                    if( this.exception != null )
                    {
                        throw this.exception;
                    }

                    return this.ret;
                }
            }
            private readonly Exception exception;
            private readonly System.Object ret;
        }


        private static Boolean initialized = false;

    }
}

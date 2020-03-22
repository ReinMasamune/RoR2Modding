using System;
using BepInEx;
using BepInEx.Logging;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace ReinCore
{
    public struct JobDelegate<TDelegate> where TDelegate : System.Delegate
    {
        public JobDelegate( TDelegate function )
        {
            if( function == null ) throw new ArgumentNullException( nameof(function) );
            if( function.Target != null ) throw new ArgumentException( "Must be a static method", nameof(function) );
            var method = function.Method;
            if( method == null ) throw new ArgumentNullException( nameof( function ) );
            if( !method.IsStatic ) throw new ArgumentNullException( "Must be a static method", nameof( function ) );
            this.methodHandle = method.MethodHandle;



        }
        public System.Object Invoke( params System.Object[] args )
        {
            return FastInvoker.Invoker(this.methodHandle, null, args ).result;
        }
        private readonly RuntimeMethodHandle methodHandle;
    }

    internal static class FastInvoker
    {
        internal static void Initialize()
        {
            if( !initialized )
            {
                var monoMethod = MethodBase.GetCurrentMethod().GetType();
                var method = monoMethod.GetMethod("InternalInvoke", BindingFlags.NonPublic | BindingFlags.Instance );

                var monoMethodConstructor = monoMethod.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, default, new[]{ typeof(RuntimeMethodHandle) }, default);

                var exceptionMember = typeof(InvokerResult).GetMember("exception", MemberTypes.Method, BindingFlags.NonPublic | BindingFlags.Instance )[0];
                var retMember = typeof(InvokerResult).GetMember("ret", MemberTypes.Method, BindingFlags.NonPublic | BindingFlags.Instance )[0];

                var instanceParam = Expression.Parameter(typeof(RuntimeMethodHandle), "handle" );
                var argsParam = Expression.Parameter(typeof(System.Object[]), "args" );
                var exceptionOut = Expression.Variable( typeof(Exception), "exception" );
                var resultOut = Expression.Variable( typeof(System.Object), "result" );
                var newMonoMethod = Expression.New(monoMethodConstructor, instanceParam );
                var call = Expression.Call(newMonoMethod,method,argsParam,exceptionOut);
                var newExpr = Expression.New( typeof(InvokerResult));
                var resultBind = Expression.Bind( retMember, call );
                var exceptionBind = Expression.Bind( exceptionMember, exceptionOut );
                var memberInit = Expression.MemberInit(newExpr,resultBind,exceptionBind);

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
                    if( this.exception != null ) throw this.exception;
                    return this.ret;
                }
            }
            private Exception exception;
            private System.Object ret;
        }


        private static Boolean initialized = false;

    }
}

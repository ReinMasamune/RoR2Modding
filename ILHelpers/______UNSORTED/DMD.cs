namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Collections.Generic;

    using MonoMod.Cil;
    using MonoMod.Utils;

    using Object = System.Object;


    internal static class DMDTools
    {
        private static readonly HashSet<String> usedNames = new HashSet<String>();

        internal static String GenerateName( String nameText, Type returnType, params Type[] argTypes )
        {
            var retText = returnType?.FullName ?? "void";
            var argText = String.Join( "+_+", argTypes?.Select((t) => t.FullName) ?? new[] { "void" } );

            var name = $"{nameText}<>{retText}<>{argText}";
            while( usedNames.Contains( name ) )
            {
                name = $"{name}_";
            }
            return name;
        }

        internal static ILContext GetContext( this DynamicMethodDefinition dmd )
        {
            // TODO: Implement (need to consider both cecil and emit backend)
            return default;
        }
    }

    public abstract class DMDBase<TEmitDelegate, TReturnedDelegate> : IDMD<TReturnedDelegate>
        where TEmitDelegate : Delegate
        where TReturnedDelegate : Delegate
    {
        public TReturnedDelegate function { get; private set; }
        protected abstract void InvokeEmitter( ILContext context, TEmitDelegate emitter );

        protected DMDBase(String name, TEmitDelegate emitter)
        {
            var delSig = typeof(TReturnedDelegate).GetMethod( "Invoke" );
            var ret = this.returnType;
            var args = delSig.GetParameters()?.Select((p) => p.ParameterType)?.ToArray() ?? Array.Empty<Type>();

            using( var dmd = new DynamicMethodDefinition( DMDTools.GenerateName( name, ret, args ), ret, args ) )
            {
                this.InvokeEmitter( dmd.GetContext(), emitter );
                this.function = (TReturnedDelegate)dmd.Generate().CreateDelegate<TReturnedDelegate>();
            }
        }

        protected virtual Type returnType { get => s_returnType; }
        protected virtual Type[] argTypes { get => s_argTypes; }

        static DMDBase()
        {
            static Type GetParamType( ParameterInfo parameter ) => parameter.ParameterType;

            var method = typeof(TReturnedDelegate)?.GetMethod("Invoke");

            s_returnType = method?.ReturnType;
            s_argTypes = method?.GetParameters()?.Select( GetParamType )?.ToArray() ?? Array.Empty<Type>();
        }
        protected static Type s_returnType { get; }
        protected static Type[] s_argTypes { get; }
    }

    public class DirectDMD<TReturnedDelegate> : DMDBase<ILContext.Manipulator, TReturnedDelegate>
        where TReturnedDelegate : Delegate
    {
        public DirectDMD( String name, ILContext.Manipulator emitter ) : base( name, emitter ) { }

        protected sealed override void InvokeEmitter( ILContext context, ILContext.Manipulator emitter )
        {
            emitter( context );
        }
    }

    public class DirectDMD<TEmitDelegate, TReturnedDelegate> : DMDBase<TEmitDelegate, TReturnedDelegate>
        where TEmitDelegate : Delegate
        where TReturnedDelegate : Delegate
    {
        public DirectDMD( String name, TEmitDelegate emitter ) : base(name, emitter)
        {
            if( !DirectDMD<TEmitDelegate, TReturnedDelegate>.argsValid )
                throw new ArgumentException( $"{nameof( TEmitDelegate )} args do not match {nameof( TReturnedDelegate )}" );
        }

        protected override void InvokeEmitter( ILContext context, TEmitDelegate emitter )
        {
            // TODO: Use reflection to invoke any type of emitter
        }

        static DirectDMD()
        {
            // TODO: Check validity here
            argsValid = true;
        }
        private static readonly Boolean argsValid;
    }
}
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ClassLibrary3
{
    public interface ITestInterface
    {
        void DoShit();

        ITestInterface op_Explicit( System.Object obj )
        {
            Console.WriteLine( "Converting?" );
            var attempt = obj as ITestInterface;
            if( attempt != null ) return attempt;
            else
            {
                var t = obj.GetType();
                var t2 = typeof(FlexibleImplementation<>).MakeGenericType( t );
                var constr = t2.GetField( "constructor", BindingFlags.Static | BindingFlags.NonPublic );
                return (ITestInterface)Activator.CreateInstance( t2, obj );
            }
        }
    }

    public class FlexibleImplementation<TObject> : ITestInterface where TObject : ITestInterface
    {
        internal static ConstructorInfo constructor;
        private static DoShitDelegate _DoShit;
        static FlexibleImplementation()
        {
            var method = typeof(TObject).GetMethod( nameof(DoShit) );
            if( method == null ) throw new MissingMethodException();
            var param = Expression.Parameter( typeof( TObject ) );
            var body = Expression.Call( param, method );
            _DoShit = Expression.Lambda<DoShitDelegate>( body, param ).Compile(false);
            constructor = typeof( FlexibleImplementation<TObject> ).GetConstructor( new[] { typeof( TObject ) } );
        }

        public FlexibleImplementation( TObject obj )
        {
            this.backingObject = obj;
        }

        private TObject backingObject;

        private delegate void DoShitDelegate( TObject obj );


        public void DoShit()
        {
            _DoShit( this.backingObject );
        }
    }
}

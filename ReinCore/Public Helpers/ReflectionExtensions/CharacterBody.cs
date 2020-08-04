namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using RoR2;

    using UnityEngine.Networking;

    public static class _CharacterBody
    {
        //private delegate void SetBuffCountDelegate( CharacterBody body, BuffIndex buff, Int32 count );
        //private static readonly SetBuffCountDelegate _setBuffCount;

        static _CharacterBody()
        {
            //ParameterExpression instParam = Expression.Parameter( typeof(CharacterBody), "instance" );
            //ParameterExpression buffParam = Expression.Parameter( typeof(BuffIndex), "buff" );
            //ParameterExpression countParam = Expression.Parameter( typeof(Int32), "count" );

            //MethodInfo method = typeof(CharacterBody).GetMethod( "SetBuffCount", BindingFlags.NonPublic | BindingFlags.Instance );
            //MethodCallExpression body = Expression.Call( instParam, method, buffParam, countParam );

            //_setBuffCount = Expression.Lambda<SetBuffCountDelegate>( body, instParam, buffParam, countParam ).Compile();
        }

        [Obsolete("unneeded", true)]
        public static void _SetBuffCount( this CharacterBody body, BuffIndex buff, Int32 count )
        {
            if(NetworkServer.active)
            {
                body.SetBuffCount( buff, count );
            }
        }
    }
}

namespace ReinCore
{
    using System;

    using UnityEngine;

    public static class UnityHelpers
    {
        public static Boolean ObjectsSafe( params Func<UnityEngine.Object>[] objects )
        {
            foreach( var obj in objects )
            {
                if( obj == null ) return false;
                var temp = obj();
                if( !temp || temp == null ) return false;
            }

            return true;
        }
    }
}

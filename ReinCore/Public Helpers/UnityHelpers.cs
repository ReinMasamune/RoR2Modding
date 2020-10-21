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

    public struct UnityRef<T>
        where T : UnityEngine.Object
    {
        private T? _value;
        public T? value
        {
            get => this._value ? this._value : null;
            set => this._value = value;
        }

        internal UnityRef(T? val) => this._value = val;

        public static implicit operator T? (UnityRef<T> unityRef) => unityRef.value;
        public static implicit operator UnityRef<T> (T? unityObj) => new(unityObj);

        public override Int32 GetHashCode() => this._value?.GetInstanceID() ?? 0;
        public override String ToString() => this._value!.ToString();
        public override Boolean Equals(System.Object obj) => this._value!.Equals(obj);
        public static Boolean operator ==(UnityRef<T> a, UnityRef<T> b) => a._value == b._value;
        public static Boolean operator ==(T? a, UnityRef<T> b) => a == b._value;
        public static Boolean operator ==(UnityRef<T> a, T? b) => a._value == b;
        public static Boolean operator !=(UnityRef<T> a, UnityRef<T> b) => a._value != b._value;
        public static Boolean operator !=(T? a, UnityRef<T> b) => a != b._value;
        public static Boolean operator !=(UnityRef<T> a, T? b) => a._value != b;
    }

    public static class UnityRefXtn
    {
        public static T? Safe<T>(this T obj)
            where T : UnityEngine.Object
            => obj ? obj : null;

        public static UnityRef<T> SafeRef<T>(this T obj)
            where T : UnityEngine.Object
            => new(obj);
    }
}

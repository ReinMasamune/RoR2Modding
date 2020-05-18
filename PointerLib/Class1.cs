namespace PointerLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class Unsafe
    {
        [MethodImpl( MethodImplOptions.ForwardRef )]
        public static unsafe extern ref T AsRef<T>( void* ptr );

        [MethodImpl( MethodImplOptions.ForwardRef )]
        public static unsafe extern Int32 SizeOf<T>();
    }

    public struct SafePointer<T> : IList<T>, ICollection<T> where T : unmanaged
    {
        public unsafe SafePointer( in T* ptr, Int32 length, Int32 maxLength )
        {
            this._ptr = ptr;
            this.currentLength = length;
            this.maxLength = maxLength;
        }

        public Int32 Count { get => this.currentLength; }
        public Boolean IsReadOnly { get => false; }

        public T this[Int32 index] 
        { 
            get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Int32 IndexOf( T item ) => throw new NotImplementedException();
        public void Insert( Int32 index, T item ) => throw new NotImplementedException();
        public void RemoveAt( Int32 index ) => throw new NotImplementedException();
        public void Add( T item ) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public Boolean Contains( T item ) => throw new NotImplementedException();
        public void CopyTo( T[] array, Int32 arrayIndex ) => throw new NotImplementedException();
        public Boolean Remove( T item ) => throw new NotImplementedException();
        public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();




















        private readonly unsafe T* _ptr;
        private Int32 currentLength;
        private readonly Int32 maxLength;


    }
}

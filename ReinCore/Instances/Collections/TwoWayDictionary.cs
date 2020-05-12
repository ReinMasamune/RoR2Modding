namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TwoWayDictionary<TKey1,TKey2>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TwoWayDictionary()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.firstToSecond = new Dictionary<TKey1, TKey2>();
            this.secondToFirst = new Dictionary<TKey2, TKey1>();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TwoWayDictionary( Int32 capacity )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.firstToSecond = new Dictionary<TKey1, TKey2>( capacity );
            this.secondToFirst = new Dictionary<TKey2, TKey1>( capacity );
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TKey2 this[TKey1 index]
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.firstToSecond[index];
            set
            {
                this.Remove( index );
                this.Add( index, value );
            }
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TKey1 this[TKey2 index]
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.secondToFirst[index];
            set
            {
                this.Remove( index );
                this.Add( value, index );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Remove( TKey1 key )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( this.firstToSecond.TryGetValue( key, out TKey2 k2 ) )
            {
                _ = this.secondToFirst.Remove( k2 );
                _ = this.firstToSecond.Remove( key );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Remove( TKey2 key )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( this.secondToFirst.TryGetValue( key, out TKey1 k1 ) )
            {
                _ = this.firstToSecond.Remove( k1 );
                _ = this.secondToFirst.Remove( key );
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Add( TKey1 key1, TKey2 key2 )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.firstToSecond[key1] = key2;
            this.secondToFirst[key2] = key1;
        }





        private readonly Dictionary<TKey1,TKey2> firstToSecond;
        private readonly Dictionary<TKey2,TKey1> secondToFirst;
    }
}

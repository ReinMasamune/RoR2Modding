using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    public class TwoWayDictionary<TKey1,TKey2>
    {
        public TwoWayDictionary()
        {
            this.firstToSecond = new Dictionary<TKey1, TKey2>();
            this.secondToFirst = new Dictionary<TKey2, TKey1>();
        }

        public TwoWayDictionary( Int32 capacity )
        {
            this.firstToSecond = new Dictionary<TKey1, TKey2>( capacity );
            this.secondToFirst = new Dictionary<TKey2, TKey1>( capacity );
        }

        public TKey2 this[TKey1 index]
        {
            get => this.firstToSecond[index];
            set
            {
                this.Remove( index );
                this.Add( index, value );
            }
        }
        public TKey1 this[TKey2 index]
        {
            get => this.secondToFirst[index];
            set
            {
                this.Remove( index );
                this.Add( value, index );
            }
        }

        public void Remove( TKey1 key )
        {
            if( this.firstToSecond.TryGetValue( key, out var k2 ) )
            {
                this.secondToFirst.Remove( k2 );
                this.firstToSecond.Remove( key );
            }
        }

        public void Remove( TKey2 key )
        {
            if( this.secondToFirst.TryGetValue( key, out var k1 ) )
            {
                this.firstToSecond.Remove( k1 );
                this.secondToFirst.Remove( key );
            }
        }

        public void Add( TKey1 key1, TKey2 key2 )
        {
            this.firstToSecond[key1] = key2;
            this.secondToFirst[key2] = key1;
        }





        private Dictionary<TKey1,TKey2> firstToSecond;
        private Dictionary<TKey2,TKey1> secondToFirst;
    }
}

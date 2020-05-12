using System;
using System.Collections.Generic;

namespace Rein.RogueWispPlugin.Helpers
{
    public class AssociationMap<TKey, TValue>
    {
        public HashSet<TValue> this[TKey key]
        {
            get
            {
                return default;
            }
        }
        public TKey this[TValue value]
        {
            get
            {
                return default;
            }
        }

        public IEnumerable<TKey> keys
        {
            get => this.keyToValues.Keys;
        }

        public IEnumerable<TValue> values
        {
            get => this.valueToKey.Keys;
        }

        public IEnumerable<KeyValuePair<TValue, TKey>> valueKeys
        {
            get => this.valueToKey;
        }

        public Boolean ContainsKey( TKey key )
        {
            return this.keyToValues.ContainsKey( key );
        }
        public Boolean ContainsValue( TValue value )
        {
            return this.valueToKey.ContainsKey( value );
        }

        public void Add( TKey key, TValue value )
        {
            if( !this.keyToValues.ContainsKey( key ) )
            {
                this.keyToValues[key] = new HashSet<TValue>();
            }

            this.keyToValues[key].Add( value );
            this.valueToKey[value] = key;
        }

        public void RemoveValue( TValue value )
        {
            if( !this.valueToKey.ContainsKey( value ) )
            {
                throw new KeyNotFoundException( "The value was not found in the collection" );
            }
            var key = this.valueToKey[value];
            this.valueToKey.Remove( value );
            this.keyToValues[key].Remove( value );
            if( this.keyToValues[key].Count == 0 )
            {
                this.keyToValues.Remove( key );
            }
        }

        public void RemoveKey( TKey key )
        {
            if( !this.keyToValues.ContainsKey( key ) )
            {
                throw new KeyNotFoundException( "The key was not found in the collection" );
            }
            foreach( var value in this.keyToValues[key] )
            {
                this.valueToKey.Remove( value );
            }
            this.keyToValues.Remove( key );
        }


        private Dictionary<TKey, HashSet<TValue>> keyToValues = new Dictionary<TKey, HashSet<TValue>>();
        private Dictionary<TValue,TKey> valueToKey = new Dictionary<TValue, TKey>();
    }
}

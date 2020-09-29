namespace BakeTest.Mod
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    internal struct RentList<T> : IDisposable, IList<T>
    {
        private Rental<List<T>>.Rent item;
        internal List<T> list => this.item.item;
        private IList<T> ilist => this.item.item;
        public static implicit operator List<T>(RentList<T> rented) => rented.list;
        internal RentList(Rental<List<T>>.Rent item) => this.item = item;
        public void Dispose()
        {
            item.Dispose();
            item = default;
        }
        public T this[Int32 index] { get => this.list[index]; set => this.list[index] = value; }
        public Int32 Count => this.list.Count;
        public void Add(T item) => this.list.Add(item);
        public void AddRange(IEnumerable<T> collection) => this.list.AddRange(collection);
        public void Clear() => this.list.Clear();
        public Boolean Contains(T item) => this.list.Contains(item);
        public void CopyTo(T[] array, Int32 arrayIndex) => this.list.CopyTo(array, arrayIndex);
        // TODO: Add mappings for rest of list functions
        public List<T>.Enumerator GetEnumerator() => this.list.GetEnumerator();
        public Int32 IndexOf(T item) => this.list.IndexOf(item);
        public void Insert(Int32 index, T item) => this.list.Insert(index, item);
        public Boolean Remove(T item) => this.list.Remove(item);
        public void RemoveAt(Int32 index) => this.list.RemoveAt(index);
        Boolean ICollection<T>.IsReadOnly => this.ilist.IsReadOnly;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
    internal static class ListRental<T>
    {
        static ListRental() => Rental<List<T>>.onReturned += List_Ex.Clear;

        internal static RentList<T> GetRent() => new(Rental<List<T>>.GetRent());
    }

    internal static class Rental<T>
        where T : class, new()
    {
        internal static Rent GetRent() => new(GetNext());
        internal struct Rent : IDisposable
        {
            internal Rent(T item) => this.item = item;

            public void Dispose()
            {
                if(this.item is null) return;
                Return(this.item);
                this.item = null;
            }

            internal T item { get; private set; }
        }

        private static event Action<T> _onReturned;
        internal static event Action<T> onReturned
        {
            add
            {
                var invList = _onReturned?.GetInvocationList();
                if(invList is null || !invList.Contains(value)) _onReturned += value;
            }
            remove
            {
                var invList = _onReturned?.GetInvocationList();
                if(invList is not null && invList.Contains(value)) _onReturned += value;
            }
        }

        private static readonly Stack<T> buffer = new();
        private static T GetNext() => buffer.Count < 1 ? (new()) : buffer.Pop();
        private static void Return(T returned) => buffer.Push(_onReturned.Pipe(returned));
    }
}

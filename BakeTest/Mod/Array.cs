namespace BakeTest.Mod
{
    using System;

    internal struct Array<T>
    {
        internal enum Index : UInt32 { }

        internal readonly T[] _data;
        internal Array(Int32 length)
        {
            this._data = new T[length];
        }

        internal Index zero => 0u;
        internal Index length => (Index)this._data.Length;

        internal ref T this[Index index]
        {
            get => ref this._data[(Int32)index];
        }
    }

#region Logging
    #endregion
}

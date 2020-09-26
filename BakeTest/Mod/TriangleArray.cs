namespace BakeTest.Mod
{
    using System;

    internal struct TriangleArray
    {
        internal enum Index : UInt32 { }

        internal readonly Int32[] _data;
        internal TriangleArray(Int32[] data)
        {
            if(data.Length % 3 != 0) throw new InvalidOperationException("Invalid length");
            this._data = data;
        }

        internal Triangle this[Index index]
        {
            get
            {
                var start = (Int32)index * 3;
                return new Triangle(this._data[start++], this._data[start++], this._data[start++]);
            }
        }

        internal Index zero => 0u;
        internal Index length => (Index)(this._data.Length / 3);
    }
}

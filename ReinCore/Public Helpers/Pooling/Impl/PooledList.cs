namespace ReinCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;

    using UnityEngine;

    public class PooledList<T>
    {
        private Byte _po2Index;
        private Byte po2Index
        {
            get => this._po2Index;
            set
            {
                if(value > 31) throw new IndexOutOfRangeException();
                this._po2Index = value;
                var newArray = Po2ArrayPool<T>.GetArray(value);
                if(this.backingArray is not null)
                {
                    this.backingArray.CopyTo(newArray, 0);
                    Po2ArrayPool<T>.ReturnArray(this.backingArray);
                }
                this.backingArray = newArray;
                this.capacity = (UInt32)this.backingArray.LongLength;
            }
        }
        private T?[] backingArray;
        private UInt32 capacity { get; set; }
        private UInt32 count;

        private UInt32 CheckInd(UInt32 ind)
        {
            if(ind >= this.count) throw new IndexOutOfRangeException();
            return ind;
        }
        private void CheckSize(UInt32 newCount)
        {

        }

        public T? this[UInt32 index]
        {
            get => this.backingArray[this.CheckInd(index)];
            set => this.backingArray[this.CheckInd(index)] = value;
        }

        public void Append(T? item)
        {
            this.CheckSize(++this.count);
        }
    }

    internal static class Po2ArrayPool<T>
    {
        internal static T[] GetArray(Byte po2Num) => po2Num switch
        {
            0 => Pool<T[], Init_0, CleanArray>.item,
            1 => Pool<T[], Init_1, CleanArray>.item,
            2 => Pool<T[], Init_2, CleanArray>.item,
            3 => Pool<T[], Init_3, CleanArray>.item,
            4 => Pool<T[], Init_4, CleanArray>.item,
            5 => Pool<T[], Init_5, CleanArray>.item,
            6 => Pool<T[], Init_6, CleanArray>.item,
            7 => Pool<T[], Init_7, CleanArray>.item,
            8 => Pool<T[], Init_8, CleanArray>.item,
            9 => Pool<T[], Init_9, CleanArray>.item,
            10 => Pool<T[], Init_10, CleanArray>.item,
            11 => Pool<T[], Init_11, CleanArray>.item,
            12 => Pool<T[], Init_12, CleanArray>.item,
            13 => Pool<T[], Init_13, CleanArray>.item,
            14 => Pool<T[], Init_14, CleanArray>.item,
            15 => Pool<T[], Init_15, CleanArray>.item,
            16 => Pool<T[], Init_16, CleanArray>.item,
            17 => Pool<T[], Init_17, CleanArray>.item,
            18 => Pool<T[], Init_18, CleanArray>.item,
            19 => Pool<T[], Init_19, CleanArray>.item,
            20 => Pool<T[], Init_20, CleanArray>.item,
            21 => Pool<T[], Init_21, CleanArray>.item,
            22 => Pool<T[], Init_22, CleanArray>.item,
            23 => Pool<T[], Init_23, CleanArray>.item,
            24 => Pool<T[], Init_24, CleanArray>.item,
            25 => Pool<T[], Init_25, CleanArray>.item,
            26 => Pool<T[], Init_26, CleanArray>.item,
            27 => Pool<T[], Init_27, CleanArray>.item,
            28 => Pool<T[], Init_28, CleanArray>.item,
            29 => Pool<T[], Init_29, CleanArray>.item,
            30 => Pool<T[], Init_30, CleanArray>.item,
            31 => Pool<T[], Init_31, CleanArray>.item,
            _ => throw new IndexOutOfRangeException(),
        };

        internal static void ReturnArray(T[] array)
        {
            switch((UInt32)array.LongLength)
            {
                case 0b1u:
                    Pool<T[], Init_0, CleanArray>.item = array;
                    break;
                case 0b10u:
                    Pool<T[], Init_1, CleanArray>.item = array;
                    break;
                case 0b100u:
                    Pool<T[], Init_2, CleanArray>.item = array;
                    break;
                case 0b1000u:
                    Pool<T[], Init_3, CleanArray>.item = array;
                    break;
                case 0b10000u:
                    Pool<T[], Init_4, CleanArray>.item = array;
                    break;
                case 0b100000u:
                    Pool<T[], Init_5, CleanArray>.item = array;
                    break;
                case 0b1000000u:
                    Pool<T[], Init_6, CleanArray>.item = array;
                    break;
                case 0b10000000u:
                    Pool<T[], Init_7, CleanArray>.item = array;
                    break;
                case 0b100000000u:
                    Pool<T[], Init_8, CleanArray>.item = array;
                    break;
                case 0b1000000000u:
                    Pool<T[], Init_9, CleanArray>.item = array;
                    break;
                case 0b10000000000u:
                    Pool<T[], Init_10, CleanArray>.item = array;
                    break;
                case 0b100000000000u:
                    Pool<T[], Init_11, CleanArray>.item = array;
                    break;
                case 0b1000000000000u:
                    Pool<T[], Init_12, CleanArray>.item = array;
                    break;
                case 0b10000000000000u:
                    Pool<T[], Init_13, CleanArray>.item = array;
                    break;
                case 0b100000000000000u:
                    Pool<T[], Init_14, CleanArray>.item = array;
                    break;
                case 0b1000000000000000u:
                    Pool<T[], Init_15, CleanArray>.item = array;
                    break;
                case 0b10000000000000000u:
                    Pool<T[], Init_16, CleanArray>.item = array;
                    break;
                case 0b100000000000000000u:
                    Pool<T[], Init_17, CleanArray>.item = array;
                    break;
                case 0b1000000000000000000u:
                    Pool<T[], Init_18, CleanArray>.item = array;
                    break;
                case 0b10000000000000000000u:
                    Pool<T[], Init_19, CleanArray>.item = array;
                    break;
                case 0b100000000000000000000u:
                    Pool<T[], Init_20, CleanArray>.item = array;
                    break;
                case 0b1000000000000000000000u:
                    Pool<T[], Init_21, CleanArray>.item = array;
                    break;
                case 0b10000000000000000000000u:
                    Pool<T[], Init_22, CleanArray>.item = array;
                    break;
                case 0b100000000000000000000000u:
                    Pool<T[], Init_23, CleanArray>.item = array;
                    break;
                case 0b1000000000000000000000000u:
                    Pool<T[], Init_24, CleanArray>.item = array;
                    break;
                case 0b10000000000000000000000000u:
                    Pool<T[], Init_25, CleanArray>.item = array;
                    break;
                case 0b100000000000000000000000000u:
                    Pool<T[], Init_26, CleanArray>.item = array;
                    break;
                case 0b1000000000000000000000000000u:
                    Pool<T[], Init_27, CleanArray>.item = array;
                    break;
                case 0b10000000000000000000000000000u:
                    Pool<T[], Init_28, CleanArray>.item = array;
                    break;
                case 0b100000000000000000000000000000u:
                    Pool<T[], Init_29, CleanArray>.item = array;
                    break;
                case 0b1000000000000000000000000000000u:
                    Pool<T[], Init_30, CleanArray>.item = array;
                    break;
                case 0b10000000000000000000000000000000u:
                    Pool<T[], Init_31, CleanArray>.item = array;
                    break;
                default:
                    Log.Error("Invalid length, will be collected normally");
                    break;
            }
        }

        private struct CleanArray : ICleanItem<T[]>
        {
            public void CleanItem(T[] item)
            {
                for(UInt32 i = 0; i < item.LongLength; ++i) item[i] = default;
            }
        }
        private struct Init_0 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1u]; }
        private struct Init_1 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10u]; }
        private struct Init_2 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100u]; }
        private struct Init_3 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000u]; }
        private struct Init_4 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000u]; }
        private struct Init_5 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000u]; }
        private struct Init_6 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000u]; }
        private struct Init_7 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000u]; }
        private struct Init_8 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000u]; }
        private struct Init_9 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000u]; }
        private struct Init_10 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000u]; }
        private struct Init_11 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000u]; }
        private struct Init_12 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000u]; }
        private struct Init_13 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000u]; }
        private struct Init_14 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000u]; }
        private struct Init_15 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000u]; }
        private struct Init_16 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000u]; }
        private struct Init_17 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000000u]; }
        private struct Init_18 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000000u]; }
        private struct Init_19 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000000u]; }
        private struct Init_20 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000000000u]; }
        private struct Init_21 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000000000u]; }
        private struct Init_22 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000000000u]; }
        private struct Init_23 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000000000000u]; }
        private struct Init_24 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000000000000u]; }
        private struct Init_25 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000000000000u]; }
        private struct Init_26 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000000000000000u]; }
        private struct Init_27 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000000000000000u]; }
        private struct Init_28 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000000000000000u]; }
        private struct Init_29 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b100000000000000000000000000000u]; }
        private struct Init_30 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b1000000000000000000000000000000u]; }
        private struct Init_31 : IInitItem<T[]>
        { public T[] InitItem() => new T[0b10000000000000000000000000000000u]; }
    }
}

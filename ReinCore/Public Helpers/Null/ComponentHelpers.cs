namespace ReinCore
{
    using System;
    using System.ComponentModel;

    public readonly struct Some<T> : IOption<T>
    {
        internal Some(T val)
        {
            if(val is null) throw new ArgumentNullException(nameof(val));
            this._val = val;
        }
        private readonly T _val;

        public T value => this._val;
    }

    public readonly struct None<T> : IOption<T>
    {
        public T value => throw new NotImplementedException();
    }

    //Need patcher to make this sealed to prevent silly people from cheating.......
    public interface IOption<T>
    {
        T value { get; }
    }

    public static class Option
    {
        public static Some<T> Some<T>(this T val)
        {
            if(val is null) throw new ArgumentNullException(nameof(val));

            return new Some<T>(val);   
        }

        public static None<T> None<T>() => new();


        public static Boolean Some<TOpt, TVal>(this TOpt option, out TVal value)
            where TOpt : struct, IOption<TVal> => option switch       
        {
            Some<TVal> some => (value = some.value) is not null,
            None<TVal> none => (value = default) is not null && false,
        };
    }

    internal static class SyntaxTest
    {
        private static void Stuff<T1>(T1 val1)
            where T1 : struct, IOption<int>
        {
            if(val1.Some(out int i))
            {
                //i is valid
            }
        }
    }
}
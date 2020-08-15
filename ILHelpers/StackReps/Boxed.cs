namespace ILHelpers
{
    public sealed class Boxed<T>
        where T : struct
    {
        private readonly T stored;
        private Boxed(T item)
        {
            this.stored = item;
        }

        public static explicit operator T(Boxed<T> boxed)
        {
            return boxed.stored;
        }

        public static explicit operator Boxed<T>(T item)
        {
            return new Boxed<T>(item);
        }
    }
}

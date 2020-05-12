namespace ReinCore
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ICloneable<TObject> where TObject : class
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>The Clone</returns>
        TObject Clone();
    }
}

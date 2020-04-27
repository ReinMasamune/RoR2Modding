using System;
using System.Collections.Generic;
using System.Text;

namespace ReinCore
{
    public interface ICloneable<TObject> where TObject : class
    {
        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>The Clone</returns>
        TObject Clone();
    }
}

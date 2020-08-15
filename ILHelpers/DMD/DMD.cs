using System;
using System.Collections.Generic;
using System.Text;

namespace ILHelpers
{
    public static unsafe partial class DMD
    {
        #region External

        #endregion
        #region Internal
        private static readonly HashSet<String> usedNames = new HashSet<String>();
        private static String CheckName(String input)
        {
            input ??= $"UnnamedDMD|>{usedNames.Count}<|";
            if(!usedNames.Add(input)) if(!usedNames.Add(input = $"{input}|>{usedNames.Count}<|")) throw new ArgumentException("Error with name");
            return input;
        }
        #endregion
    }
}

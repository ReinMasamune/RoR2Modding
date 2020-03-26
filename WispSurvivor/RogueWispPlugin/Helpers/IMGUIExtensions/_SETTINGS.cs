#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers.IMGUI
{
    internal static class Settings
    {
        internal const Single widthPerChar = 9f;
        internal const Single defaultMinSpace = 5f;
    }

    internal interface IGUIMenuDrawer<TValue>
    {
        void ChangeValue( TValue value );
        void ChangeName( String name );
        void ChangeAction( Action<TValue> onChanged );
        void Draw();
    }
}
#endif
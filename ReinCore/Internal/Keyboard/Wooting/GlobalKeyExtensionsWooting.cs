namespace ReinCore.Wooting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class GlobalKeyExtensionsWooting
    {
        internal static (Byte row, Byte col) GetWootingKeyCoords( this GlobalKeys key ) => keyMap[key];

        internal static IList<GlobalKeys> GetAllWootingKeys() => keyMap.Keys.ToList();




        private static readonly Dictionary<GlobalKeys, (Byte row, Byte col)> keyMap = new Dictionary<GlobalKeys, (Byte row, Byte col)>
        {
            //Row 0
            { GlobalKeys.Esc, (0,0) },
            { GlobalKeys.F1, (0,2) },
            { GlobalKeys.F2, (0,3) },
            { GlobalKeys.F3, (0,4) },
            { GlobalKeys.F4, (0,5) },
            { GlobalKeys.F5, (0,6) },
            { GlobalKeys.F6, (0,7) },
            { GlobalKeys.F7, (0,8) },
            { GlobalKeys.F8, (0,9) },
            { GlobalKeys.F9, (0,10) },
            { GlobalKeys.F10, (0,11) },
            { GlobalKeys.F11, (0,12) },
            { GlobalKeys.F12, (0,13) },
            { GlobalKeys.PrintScreen, (0,14) },
            { GlobalKeys.PauseBreak, (0,15) },
            { GlobalKeys.Mode_ScrollLock, (0,16) },
            { GlobalKeys.A1, (0,17) },
            { GlobalKeys.A2, (0,18) },
            { GlobalKeys.A3, (0,19) },
            { GlobalKeys.Mode, (0,20) },

            //Row 1
            { GlobalKeys.Tilda, (1,0) },
            { GlobalKeys.N1, (1,1) },
            { GlobalKeys.N2, (1,2) },
            { GlobalKeys.N3, (1,3) },
            { GlobalKeys.N4, (1,4) },
            { GlobalKeys.N5, (1,5) },
            { GlobalKeys.N6, (1,6) },
            { GlobalKeys.N7, (1,7) },
            { GlobalKeys.N8, (1,8) },
            { GlobalKeys.N9, (1,9) },
            { GlobalKeys.N0, (1,10) },
            { GlobalKeys.Minus, (1,11) },
            { GlobalKeys.Equals, (1,12) },
            { GlobalKeys.Backspace, (1,13) },
            { GlobalKeys.Insert, (1,14) },
            { GlobalKeys.Home, (1,15) },
            { GlobalKeys.PageUp, (1,16) },
            { GlobalKeys.NumLock, (1,17) },
            { GlobalKeys.NumSlash, (1,18) },
            { GlobalKeys.NumMulti, (1,19) },
            { GlobalKeys.NumMinus, (1,20) },

            //Row2
            { GlobalKeys.Tab, (2,0) },
            { GlobalKeys.Q, (2,1) },
            { GlobalKeys.W, (2,2) },
            { GlobalKeys.E, (2,3) },
            { GlobalKeys.R, (2,4) },
            { GlobalKeys.T, (2,5) },
            { GlobalKeys.Y, (2,6) },
            { GlobalKeys.U, (2,7) },
            { GlobalKeys.I, (2,8) },
            { GlobalKeys.O, (2,9) },
            { GlobalKeys.P, (2,10) },
            { GlobalKeys.OpenBracket, (2,11) },
            { GlobalKeys.CloseBracket, (2,12) },
            { GlobalKeys.ANSI_Backslash, (2,13) },
            { GlobalKeys.Delete, (2,14) },
            { GlobalKeys.End, (2,15) },
            { GlobalKeys.PageDown, (2,16) },
            { GlobalKeys.Num7, (2,17) },
            { GlobalKeys.Num8, (2,18) },
            { GlobalKeys.Num9, (2,19) },
            { GlobalKeys.NumPlus, (2,20) },

            //Row3
            { GlobalKeys.CapsLock, (3,0) },
            { GlobalKeys.A, (3,1) },
            { GlobalKeys.S, (3,2) },
            { GlobalKeys.D, (3,3) },
            { GlobalKeys.F, (3,4) },
            { GlobalKeys.G, (3,5) },
            { GlobalKeys.H, (3,6) },
            { GlobalKeys.J, (3,7) },
            { GlobalKeys.K, (3,8) },
            { GlobalKeys.L, (3,9) },
            { GlobalKeys.SemiColon, (3,10) },
            { GlobalKeys.Apostophe, (3,11) },
            { GlobalKeys.ISO_Hash, (3,12) },
            { GlobalKeys.Enter, (3,13) },
            { GlobalKeys.Num4, (3,17) },
            { GlobalKeys.Num5, (3,18) },
            { GlobalKeys.Num6, (3,19) },

            //Row4
            { GlobalKeys.LeftShift, (4,0) },
            { GlobalKeys.ISO_Blackslash, (4,1) },
            { GlobalKeys.Z, (4,2) },
            { GlobalKeys.X, (4,3) },
            { GlobalKeys.C, (4,4) },
            { GlobalKeys.V, (4,5) },
            { GlobalKeys.B, (4,6) },
            { GlobalKeys.N, (4,7) },
            { GlobalKeys.M, (4,8) },
            { GlobalKeys.Comma, (4,9) },
            { GlobalKeys.Period, (4,10) },
            { GlobalKeys.Slash, (4,11) },

            { GlobalKeys.RightShift, (4,13) },

            { GlobalKeys.Up, (4,15) },

            { GlobalKeys.Num1, (4,17) },
            { GlobalKeys.Num2, (4,18) },
            { GlobalKeys.Num3, (4,19) },
            { GlobalKeys.NumEnter, (4,20) },

            //Row5
            { GlobalKeys.LeftCtrl, (5,0) },
            { GlobalKeys.LeftWin, (5,1) },
            { GlobalKeys.LeftAlt, (5,2) },



            { GlobalKeys.Space, (5,6) },



            { GlobalKeys.RightAlt, (5,10) },
            { GlobalKeys.RightWin, (5,11) },
            { GlobalKeys.Function, (5,12) },
            { GlobalKeys.RightControl, (5,13) },
            { GlobalKeys.Left, (5,14) },
            { GlobalKeys.Down, (5,15) },
            { GlobalKeys.Right, (5,16) },

            { GlobalKeys.Num0, (5,18) },
            { GlobalKeys.NumPeriod, (5,19) },
        };
    }

}

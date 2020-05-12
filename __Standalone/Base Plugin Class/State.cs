namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using BepInEx;

    public enum State
    {
        Failed = -1,
        Init = 0,
        Awake = 1,
        Enable = 2,
        Start = 3,
        Running = 4,
        Disabled = 5,
        Destroying = 6,
    }
}

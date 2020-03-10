using System;
using BepInEx;

namespace ReinCore
{
    [Flags]
    public enum ExecutionLevel
    {
        Debug = 1,
        Info = 2,
        Message = 4,
        Warning = 8,
        Error = 16,
        Fatal = 32,
        FindLogs = 64,
    }
}

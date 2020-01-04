namespace ModSync
{
    using BepInEx;
    using R2API.Utils;
    using System;

    public partial class Main
    {
        partial void LoadCheck()
        {
            this.canLoad = true;
        }
    }
}

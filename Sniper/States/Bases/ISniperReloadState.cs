namespace Sniper.States.Bases
{
    using Sniper.Enums;

    internal interface ISniperReloadState
    {
        ReloadTier reloadTier { get; set; }
    }
}

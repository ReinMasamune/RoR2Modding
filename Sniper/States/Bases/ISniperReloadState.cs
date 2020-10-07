namespace Rein.Sniper.States.Bases
{
    using Rein.Sniper.Enums;

    internal interface ISniperReloadState
    {
        ReloadTier reloadTier { get; set; }
    }
}

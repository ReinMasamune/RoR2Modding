namespace ReinGeneralFixes
{
    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        partial void QoLOvergrownPrinters() => this.Load += this.DoOvergrownPrinterEdits;

        private void DoOvergrownPrinterEdits()
        {
            ShopTerminalBehavior wildPrint = Resources.Load<GameObject>("Prefabs/NetworkedObjects/DuplicatorWild").GetComponent<ShopTerminalBehavior>();
            wildPrint.bannedItemTag = ItemTag.Any;
        }
    }
}

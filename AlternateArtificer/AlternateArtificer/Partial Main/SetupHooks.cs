using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void SetupHooks()
        {
            this.enable += this.Main_enable;
            this.disable += this.Main_disable;
        }

        private void Main_disable()
        {

        }
        private void Main_enable()
        {

        }
    }
}

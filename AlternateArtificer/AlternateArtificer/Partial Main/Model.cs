using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using RoR2;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void Model()
        {
            this.awake += this.Main_awake1;
        }

        private void Main_awake1()
        {

        }
    }
}

// TODO: Model skin
// TODO: Jetpack-removed mesh
// TODO: Adjust body scales and stuff

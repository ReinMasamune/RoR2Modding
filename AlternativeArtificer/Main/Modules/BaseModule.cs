namespace AlternativeArtificer.Main.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;

    using RoR2;

    using ReinCore;

    using UnityEngine;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    using Resources = Properties.Resources;
    using UnityResources = UnityEngine.Resources;
    using BF = System.Reflection.BindingFlags;
    using AlternativeArtificer.Tools;

    internal struct BaseModule : IModule<BaseModule>
    {
        public void _Construct() => throw new NotImplementedException();

        public void _AddHooks() => throw new NotImplementedException();
        public void _Awake() => throw new NotImplementedException();

        public void _End() => throw new NotImplementedException();
        public void _Init() => throw new NotImplementedException();
        public void _RemoveHooks() => throw new NotImplementedException();
        public void _Start() => throw new NotImplementedException();
    }
}

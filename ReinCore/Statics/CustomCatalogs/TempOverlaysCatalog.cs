using ReinCore;
[assembly: Catalog(typeof(TempOverlaysCatalog))]
namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;
    using UnityEngine.Networking;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    public sealed class TempOverlaysCatalog : Catalog<TempOverlaysCatalog, TempOverlayDef>
    {
        public override String guid => "Rein.TempOverlaysCatalog";
        protected internal override Int32 order => 0;
        protected override void FirstInitSetup()
        {
            HooksCore.RoR2.CharacterBody.FixedUpdate.On += this.FixedUpdate_On;
            base.FirstInitSetup();
        }
        private void FixedUpdate_On(HooksCore.RoR2.CharacterBody.FixedUpdate.Orig orig, CharacterBody self)
        {
            try { orig(self); } catch(Exception e) { Log.Error($"Caught exception in CharacterBody.FixedUpdate orig, {e}"); }
            var data = self.GetOrCreateDField(BodyData.handle);
            foreach(var def in TempOverlaysCatalog.EnumerateEntries())
            {
                if(def.entry!.index is not Index ind) continue;
                ref var curOverlay = ref data[ind];
                if(def.ShouldHaveOverlay(self))
                {
                    if(curOverlay is null)
                    {
                        curOverlay = self.AddComponent<TemporaryOverlay>();
                        def.CreateOverlay(curOverlay, self);
                        curOverlay.AddToCharacerModel(self.modelLocator.modelTransform.GetComponent<CharacterModel>());
                    }
                } else
                {
                    if(curOverlay is not null)
                    {
                        curOverlay.RemoveFromCharacterModel();
                        curOverlay.Destroy();
                        curOverlay = null;
                    }
                }
            }
        }
        private class BodyData : DynamicField<CharacterBody, BodyData>
        {
            private TemporaryOverlay[] overlays;
            public BodyData() => this.overlays = new TemporaryOverlay[TempOverlaysCatalog.count + 1];
            internal ref TemporaryOverlay this[TempOverlaysCatalog.Index ind] => ref this.overlays[(UInt64)ind];
        }
    }
    public abstract class TempOverlayDef : TempOverlaysCatalog.ICatalogDef
    {
        public TempOverlaysCatalog.Entry? entry { get; set; }
        public abstract String guid { get; }
        public abstract Boolean ShouldHaveOverlay(CharacterBody body);
        public abstract void CreateOverlay(TemporaryOverlay overlay, CharacterBody body);
    }
}
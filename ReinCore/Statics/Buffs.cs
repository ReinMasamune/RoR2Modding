namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MonoMod.Cil;

    using RoR2;

    using UnityEngine;

    public static class BuffsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static event BuffAddDelegate getAdditionalEntries;
        public delegate void BuffAddDelegate( List<BuffDef> buffList );

        static BuffsCore()
        {
            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
            HooksCore.RoR2.UI.BuffIcon.UpdateIcon.Il += UpdateIcon_Il;
            loaded = true;
        }

        private static Sprite EmittedDelegate1(Sprite cur, BuffDef inDef) => inDef is CustomSpriteBuffDef def ? def.sprite : cur;

        private static readonly MethodInfo image_set_sprite = typeof(UnityEngine.UI.Image)?.GetProperty(nameof(UnityEngine.UI.Image.sprite), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetSetMethod(true) ?? throw new MissingMemberException("No method set_sprite on image for bufficon");        
        private static void UpdateIcon_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(image_set_sprite))
            .LdLoc_(0)
            .CallDel_<Func<Sprite, BuffDef, Sprite>>(EmittedDelegate1);

        private static void EmittedAction()
        {
            var l = new List<BuffDef>();
            getAdditionalEntries?.Invoke(l);
            Int32 extras = l.Count;
            if(extras <= 0)
            {
                return;
            }
            BuffDef[] buffs = BuffCatalog.buffDefs;// buffDefs.Get();
            Int32 startPoint = buffs.Length;
            Array.Resize<BuffDef>(ref buffs, startPoint + extras);
            BuffCatalog.buffDefs = buffs;//buffDefs.Set( buffs );
            for(Int32 i = 0; i < extras; ++i)
            {
                BuffCatalog.RegisterBuff((BuffIndex)(i + startPoint), l[i]);
            }
        }

        private static void Init_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(BuffCatalog), nameof(BuffCatalog.modHelper)),
                x => x.MatchLdsflda(typeof(BuffCatalog), nameof(BuffCatalog.buffDefs))
            ).Move(1)
            .CallDel_<Action>(EmittedAction);
    }

    public class CustomSpriteBuffDef : BuffDef
    {
        public CustomSpriteBuffDef(Sprite sprite) => this.sprite = sprite;
        public readonly Sprite sprite;
    }
}
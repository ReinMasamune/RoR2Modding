using System;
using System.Collections.Generic;
using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace ReinCore
{
    public static class BuffsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static event BuffAddDelegate getAdditionalEntries;
        public delegate void BuffAddDelegate( List<BuffDef> buffList );

        static BuffsCore()
        {
            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
            RegisterBuff = (RegisterBuffDelegate)Delegate.CreateDelegate( typeof( RegisterBuffDelegate ), typeof( BuffCatalog ), "RegisterBuff" );
            loaded = true;
        }

        private delegate void RegisterBuffDelegate( BuffIndex buffIndex, BuffDef buff );
        private static RegisterBuffDelegate RegisterBuff;
        private static StaticAccessor<BuffDef[]> buffDefs = new StaticAccessor<BuffDef[]>( typeof(BuffCatalog), "buffDefs" );

        private static void Init_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After,
                x => x.MatchLdsfld( typeof( BuffCatalog ), nameof( BuffCatalog.modHelper ) ),
                x => x.MatchLdsflda( typeof( BuffCatalog ), "buffDefs" )
            );
            ++c.Index;
            c.EmitDelegate<Action>( () =>
            {
                var l = new List<BuffDef>();
                getAdditionalEntries?.Invoke( l );
                var extras = l.Count;
                if( extras <= 0 ) return;
                var buffs = buffDefs.Get();
                var startPoint = buffs.Length;
                Array.Resize<BuffDef>( ref buffs, startPoint + extras );
                buffDefs.Set( buffs );
                for( Int32 i = 0; i < extras; ++i ) RegisterBuff( (BuffIndex)(i + startPoint), l[i] );
            } );
        }
    }
}

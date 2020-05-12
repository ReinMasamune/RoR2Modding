namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using RoR2;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class BuffsCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event BuffAddDelegate getAdditionalEntries;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void BuffAddDelegate( List<BuffDef> buffList );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        static BuffsCore()
        {
            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
            RegisterBuff = (RegisterBuffDelegate)Delegate.CreateDelegate( typeof( RegisterBuffDelegate ), typeof( BuffCatalog ), "RegisterBuff" );
            loaded = true;
        }

        private delegate void RegisterBuffDelegate( BuffIndex buffIndex, BuffDef buff );
#pragma warning disable IDE1006 // Naming Styles
        private static readonly RegisterBuffDelegate RegisterBuff;
#pragma warning restore IDE1006 // Naming Styles
        private static readonly StaticAccessor<BuffDef[]> buffDefs = new StaticAccessor<BuffDef[]>( typeof(BuffCatalog), "buffDefs" );

        private static void Init_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdsfld( typeof( BuffCatalog ), nameof( BuffCatalog.modHelper ) ),
                x => x.MatchLdsflda( typeof( BuffCatalog ), "buffDefs" )
            );
            ++c.Index;
            _ = c.EmitDelegate<Action>( () =>
              {
                  var l = new List<BuffDef>();
                  getAdditionalEntries?.Invoke( l );
                  Int32 extras = l.Count;
                  if( extras <= 0 )
                  {
                      return;
                  }

                  BuffDef[] buffs = buffDefs.Get();
                  Int32 startPoint = buffs.Length;
                  Array.Resize<BuffDef>( ref buffs, startPoint + extras );
                  buffDefs.Set( buffs );
                  for( Int32 i = 0; i < extras; ++i )
                  {
                      RegisterBuff( (BuffIndex)( i + startPoint ), l[i] );
                  }
              } );
        }
    }
}

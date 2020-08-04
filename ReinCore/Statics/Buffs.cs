namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MonoMod.Cil;

    using RoR2;

    


    public static class BuffsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static event BuffAddDelegate getAdditionalEntries;
        public delegate void BuffAddDelegate( List<BuffDef> buffList );

        static BuffsCore()
        {
            Log.Warning( "BuffsCore loaded" );
            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
            //RegisterBuff = (RegisterBuffDelegate)Delegate.CreateDelegate( typeof( RegisterBuffDelegate ), typeof( BuffCatalog ), "RegisterBuff" );
            Log.Warning( "BuffsCore loaded" );
            loaded = true;
        }

        //private delegate void RegisterBuffDelegate( BuffIndex buffIndex, BuffDef buff );
        //private static readonly RegisterBuffDelegate RegisterBuff;
        //private static readonly StaticAccessor<BuffDef[]> buffDefs = new StaticAccessor<BuffDef[]>( typeof(BuffCatalog), "buffDefs" );

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

                  BuffDef[] buffs = BuffCatalog.buffDefs;// buffDefs.Get();
                  Int32 startPoint = buffs.Length;
                  Array.Resize<BuffDef>( ref buffs, startPoint + extras );
                  BuffCatalog.buffDefs = buffs;//buffDefs.Set( buffs );
                  for( Int32 i = 0; i < extras; ++i )
                  {
                      BuffCatalog.RegisterBuff( (BuffIndex)( i + startPoint ), l[i] );
                  }
              } );
        }
    }
}


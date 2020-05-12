namespace ReinGeneralFixes
{
    using System;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    internal partial class Main
    {
        partial void BalanceDeathMark()
        {
            this.Enable += this.Main_Enable5;
            this.Disable += this.Main_Disable5;
            this.FirstFrame += this.Main_FirstFrame;
        }

        private void Main_FirstFrame()
        {
            BuffDef bleed = BuffCatalog.GetBuffDef( BuffIndex.Bleeding );
            bleed.isDebuff = true;

            BuffDef fire1 = BuffCatalog.GetBuffDef( BuffIndex.OnFire );
            fire1.isDebuff = true;

            BuffDef poison = BuffCatalog.GetBuffDef( BuffIndex.Poisoned );
            poison.isDebuff = true;

            BuffDef blight = BuffCatalog.GetBuffDef( BuffIndex.Blight );
            blight.isDebuff = true;

            BuffDef mark = BuffCatalog.GetBuffDef( BuffIndex.DeathMark );
            mark.canStack = true;
        }

        private void Main_Disable5()
        {
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= this.OnHitEnemy_Il;
            try
            {
                BuffDef bleed = BuffCatalog.GetBuffDef( BuffIndex.Bleeding );
                bleed.isDebuff = false;

                BuffDef fire1 = BuffCatalog.GetBuffDef( BuffIndex.OnFire );
                fire1.isDebuff = false;

                BuffDef poison = BuffCatalog.GetBuffDef( BuffIndex.Poisoned );
                poison.isDebuff = false;

                BuffDef blight = BuffCatalog.GetBuffDef( BuffIndex.Blight );
                blight.isDebuff = false;
            } catch { }
        }
        private void Main_Enable5() => HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += this.OnHitEnemy_Il;

        private void OnHitEnemy_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            c.Index++;
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.Emit( OpCodes.Ldloc, 64 );
            _ = c.EmitDelegate<Func<CharacterBody, BuffIndex, Int32>>( ( body, index ) => body.HasBuff( index ) ? BuffCatalog.GetBuffDef( index ).canStack ? body.GetBuffCount( index ) : 10 : 0 );

            ILCursor c2 = c.Clone();
            _ = c2.GotoPrev( MoveType.AfterLabel, x => x.MatchLdcI4( 1 ), x => x.MatchStloc( 18 ), x => x.MatchBr( out _ ) );
            _ = c2.Remove();
            _ = c2.Emit( OpCodes.Ldloc_1 );
            _ = c2.EmitDelegate<Func<CharacterBody, Int32>>( ( body ) => body.GetBuffCount( BuffIndex.DeathMark ) );
            c2.Index++;
            _ = c2.Remove();


            _ = c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            c.Index++;
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldc_I4_0 );

            _ = c.GotoNext( MoveType.After, x => x.MatchLdloc( 18 ) );
            _ = c.Emit( OpCodes.Ldloc, 16 );
            _ = c.EmitDelegate<Func<Int32, Int32, Int32>>( ( count, itemCount ) => itemCount > count ? 0 : 1 );

            _ = c.GotoNext( MoveType.After, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 4 ) );
            c.Index--;
            _ = c.Remove();
            _ = c.Emit( OpCodes.Ldc_I4, 35 );

            _ = c.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( 0 ), x => x.MatchStloc( 67 ) );
            _ = c.Emit( OpCodes.Ldloc_1 );
            _ = c.Emit( OpCodes.Ldloc, 18 );
            _ = c.EmitDelegate<Action<CharacterBody, Int32>>( ( body, count ) =>
              {
                  for( Int32 i = 0; i < count; ++i )
                  {
                      body.RemoveBuff( BuffIndex.DeathMark );
                  }
              } );
        }
    }
}

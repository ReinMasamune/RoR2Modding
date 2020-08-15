namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using RoR2;

    public static class BulletFalloffCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public delegate Single FalloffDelegate( Single distance );
        public static BulletAttack.FalloffModel AddFalloffModel( FalloffDelegate falloffDelegate )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( FalloffDelegate ) );
            }

            delegates.Add( falloffDelegate );
            BulletAttack.FalloffModel ind = currentIndex++;

            if( hookApplied )
            {
                HooksCore.RoR2.BulletAttack.DefaultHitCallback.Il -= DefaultHitCallback_Il;
            }
            HooksCore.RoR2.BulletAttack.DefaultHitCallback.Il += DefaultHitCallback_Il;
            hookApplied = true;

            return ind;
        }



        static BulletFalloffCore()
        {
            //Log.Warning( "BulletFalloffCore loaded" );
            //Log.Warning( "BulletFalloffCore loaded" );
            loaded = true;
        }

        private static Boolean hookApplied = false;

        private static readonly BulletAttack.FalloffModel startingIndex = EnumExtensions.GetMax<BulletAttack.FalloffModel>();
        private static BulletAttack.FalloffModel currentIndex = startingIndex + 1;
        private static readonly List<FalloffDelegate> delegates = new List<FalloffDelegate>();

        private static void DefaultHitCallback_Il( ILContext il )
        {
            var cursor = new ILCursor( il );

            ILLabel[] baseLabels = null;
            ILLabel breakLabel = null;
            _ = cursor.GotoNext( MoveType.AfterLabel,
                x => x.MatchSwitch( out baseLabels ),
                x => x.MatchBr( out breakLabel )
            );

            Int32 origCases = baseLabels.Length;
            Array.Resize<ILLabel>( ref baseLabels, origCases + delegates.Count );

            _ = cursor.GotoLabel( breakLabel, MoveType.Before, false );

            FieldInfo field = typeof( BulletAttack.BulletHit ).GetField( nameof(BulletAttack.BulletHit.distance), BindingFlags.Instance | BindingFlags.Public );

            for( Int32 i = 0; i < delegates.Count; ++i )
            {
                Int32 caseInd = origCases + i;

                _ = cursor.Emit( OpCodes.Ldarg_1 );
                cursor.Index--;
                ILLabel label = cursor.MarkLabel();
                cursor.Index++;
                _ = cursor.Emit( OpCodes.Ldfld, field );
                _ = cursor.EmitDelegate<FalloffDelegate>( delegates[i] );
                _ = cursor.Emit( OpCodes.Stloc, 4 );
                _ = cursor.Emit( OpCodes.Br, breakLabel );

                baseLabels[caseInd] = label;
            }

            _ = cursor.GotoPrev( MoveType.AfterLabel, x => x.MatchSwitch( out _ ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Switch, baseLabels );
        }
    }
}

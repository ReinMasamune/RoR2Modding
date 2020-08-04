namespace ReinGeneralFixes
{
    using System;
    using System.Reflection;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;
    using RoR2.Projectile;

    internal partial class Main
    {
        partial void BalanceCrabs()
        {
            this.FirstFrame += this.Main_FirstFrame1;
            this.Enable += this.Main_Enable11;
        }

        private void Main_Enable11()
        {
            HooksCore.EntityStates.HermitCrab.FireMortar.FixedUpdate.Il += this.FixedUpdate_Il4;
        }

        private void FixedUpdate_Il4( ILContext il )
        {
            _ = new ILCursor( il )
                .GotoNext( MoveType.After, x => x.MatchLdsfld( typeof( EntityStates.HermitCrab.Burrowed ).GetField( "mortarCooldown" ) ) )
                .Emit( OpCodes.Ldarg_0 )
                .Emit( OpCodes.Ldfld, typeof( EntityStates.BaseState ).GetField( "attackSpeedStat", BindingFlags.NonPublic ) )
                .Emit( OpCodes.Div );
        }

        private void Main_FirstFrame1()
        {
            EntityStates.HermitCrab.FireMortar.mortarProjectilePrefab.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 3.0f;
            EntityStates.HermitCrab.FireMortar.mortarProjectilePrefab.GetComponent<ProjectileController>().procCoefficient = 3.0f;

            EntityStates.HermitCrab.FireMortar.minimumDistance = 0.0f;
        }
    }
}

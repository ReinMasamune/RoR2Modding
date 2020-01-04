

namespace AlternativeAcrid
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using MonoMod.Cil;
    using Mono.Cecil.Cil;
    using System.Reflection;
    using RoR2;
    using RoR2.Projectile;

    public partial class Main
    {
        private Single fullMult = 1.0f;
        private Single deadMult = 0.0f;
        private Single percentHealthMult = 0.0065f;
        private Single percentRatioCap = 10f;
        private Single baseDamageMult = 0.3f;

        private Type dotStack;
        private FieldInfo dotStack_damage;
        private FieldInfo dotStack_damageType;
        private FieldInfo dotStack_dotDef;

        private Type dotDef;
        private FieldInfo dotDef_interval;
        private FieldInfo dotDef_damageCoefficient;

        private void RemoveHooks()
        {
            IL.RoR2.DotController.AddDot -= this.DotController_AddDot;
            On.RoR2.HealthComponent.TakeDamage -= this.HealthComponent_TakeDamage;
        }

        private void AddHooks()
        {
            Debug.Log( "A" );
            dotStack = typeof( RoR2.DotController ).GetNestedType( "DotStack", BindingFlags.NonPublic );
            dotStack_damage = dotStack.GetField( "damage" );
            dotStack_damageType = dotStack.GetField( "damageType" );
            dotStack_dotDef = dotStack.GetField( "dotDef" );

            Debug.Log( "B" );
            dotDef = typeof( DotController ).GetNestedType( "DotDef", BindingFlags.NonPublic );
            dotDef_interval = dotDef.GetField( "interval" );
            dotDef_damageCoefficient = dotDef.GetField( "damageCoefficient" );

            IL.RoR2.DotController.AddDot += this.DotController_AddDot;
            On.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage;
        }


        private void HealthComponent_TakeDamage( On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo )
        {
            if( damageInfo.dotIndex == RoR2.DotController.DotIndex.Poison )
            {
                damageInfo.damage = self.combinedHealth * ( 1 - Mathf.Exp( -damageInfo.damage / self.fullCombinedHealth ) );
            }
            orig( self, damageInfo );
        }

        private void DotController_AddDot( MonoMod.Cil.ILContext il )
        {
            ILCursor c = new ILCursor(il);

            ILLabel[] labels = new ILLabel[3];

            c.GotoNext( MoveType.After, x => x.MatchSwitch( out labels ) );

            if( labels.Length < 3 || labels[2] == null )
            {
                return;
            }

            c.GotoLabel( labels[2], MoveType.Before );
            c.GotoNext( MoveType.After, x => x.MatchLdarg( 0 ) );
            var index1 = c.Index + 1;

            ILLabel label = labels[0];

            c.GotoNext( MoveType.Before, x => x.MatchBr( out label ) );
            //var index1 = c.Index - 4;

            if( label == labels[0] )
            {
                return;
            }
            c.GotoLabel( label, MoveType.After );
            c.GotoNext( MoveType.After, x => x.MatchBlt( out _ ) );

            c.Index += 0;
            Int32 dist = c.Index - index1;
            c.Index = index1;
            c.RemoveRange( dist);

            c.Emit( OpCodes.Ldloc, 5 );
            c.EmitDelegate<Action<HealthComponent, object>>( ( victim, stack ) =>
            {
                if( stack == null ) return;
                
                var stackTemp = Convert.ChangeType( stack, dotStack );
                var dotDefTemp = Convert.ChangeType( dotStack_dotDef.GetValue( stackTemp ), dotDef );

                Single damage = (Single)dotStack_damage.GetValue( stackTemp );

                Single baseDamage = damage / (Single)dotDef_damageCoefficient.GetValue( dotDefTemp );

                var flatPortion = baseDamage * baseDamageMult;
                var maxHpPortion = Mathf.Min( victim.fullCombinedHealth * percentHealthMult, flatPortion * percentRatioCap );
                
                dotStack_damage.SetValue( stackTemp, (object)((maxHpPortion + flatPortion) * (Single)dotDef_interval.GetValue( dotDefTemp )) );
                dotStack_damageType.SetValue( stackTemp, (object)(DamageType.NonLethal) );  
            } );

        }

    }
}

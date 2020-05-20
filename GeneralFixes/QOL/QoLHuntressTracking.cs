namespace ReinGeneralFixes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using EntityStates;
    using EntityStates.Huntress.HuntressWeapon;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using ReinCore;
    using RoR2;
    using RoR2.Skills;
    using UnityEngine;
    

    internal partial class Main
    {
        private static RaycastHit[] hitResultsBuffer = new RaycastHit[128];

        partial void QoLHuntressTracking()
        {
            this.Enable += this.Main_Enable10;
            this.Disable += this.Main_Disable10;
        }

        private void Main_Disable10()
        {

        }
        private void Main_Enable10()
        {
            HooksCore.RoR2.HuntressTracker.FixedUpdate.Il += this.FixedUpdate_Il3;
        }

        private void FixedUpdate_Il3( ILContext il )
        {
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo characterBody = typeof( HuntressTracker ).GetField( "characterBody", allFlags );
            FieldInfo trackingTarget = typeof( HuntressTracker ).GetField( "trackingTarget", allFlags );
            FieldInfo maxDistance = typeof( HuntressTracker ).GetField( "maxTrackingDistance", allFlags );
            FieldInfo trackerUpdateStopwatch = typeof(HuntressTracker).GetField( "trackerUpdateStopwatch", allFlags );
            FieldInfo trackerUpdateFrequency = typeof(HuntressTracker).GetField( "trackerUpdateFrequency", allFlags );
            FieldInfo indicator = typeof( HuntressTracker ).GetField( "indicator", allFlags );
            FieldInfo indicator_targetTransform = typeof( RoR2.Indicator ).GetField( "targetTransform", allFlags );
            FieldInfo hurtBox_group = typeof( HurtBox ).GetField( nameof( HurtBox.hurtBoxGroup ), allFlags );
            FieldInfo mainHurtBox = typeof(HurtBoxGroup).GetField( nameof(HurtBoxGroup.mainHurtBox), allFlags );

            //MethodInfo fixedDeltaTime = typeof(Time).GetProperty( "fixedDeltaTime", allFlags ).GetGetMethod();
            MethodInfo object_implicit = typeof(UnityEngine.Object).GetMethod( "op_Implicit", allFlags );


            HurtBox CastTarget( CharacterBody body, Single range )
            {
                var ray = body.inputBank.GetAimRay();
                var myTeam = body.teamComponent.teamIndex;
                var hits = Physics.SphereCastNonAlloc( ray, 0.75f, hitResultsBuffer, range, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal );

                Single worldDist = Single.PositiveInfinity;
                if( hits > 0 && Physics.Raycast( ray, out RaycastHit hit, range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    worldDist = hit.distance + 1f;
                }

                HurtBox result = null;
                Single bestDistance = Single.PositiveInfinity;

                for( Int32 i = 0; i < hits; ++i )
                {
                    var res = hitResultsBuffer[i];

                    if( res.distance > bestDistance ) continue;
                    if( res.distance > worldDist ) continue;
                    var col = res.collider;
                    if( col is null ) continue;
                    var hb = col.GetComponent<HurtBox>();
                    if( hb is null ) continue;
                    var hc = hb.healthComponent;
                    if( hc is null ) continue;
                    if( !FriendlyFireManager.ShouldDirectHitProceed( hc, myTeam ) ) continue;

                    result = hb;
                    bestDistance = res.distance;
                }
                return result;
            }

            var cursor = new ILCursor( il );
            _ = cursor.Emit( OpCodes.Stfld, trackingTarget );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldfld, indicator );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldfld, trackingTarget );
            _ = cursor.Emit( OpCodes.Ldfld, hurtBox_group );
            _ = cursor.Emit( OpCodes.Ldfld, mainHurtBox );
            _ = cursor.Emit<Component>( OpCodes.Callvirt, "get_transform" );
            _ = cursor.Emit( OpCodes.Stfld, indicator_targetTransform );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldfld, trackerUpdateStopwatch );
            _ = cursor.EmitDelegate<Func<Single>>( () => Time.fixedDeltaTime );
            _ = cursor.Emit( OpCodes.Add );
            _ = cursor.Emit( OpCodes.Stfld, trackerUpdateStopwatch );

            ILLabel normalPath = cursor.MarkLabel();
            _ = cursor.Emit( OpCodes.Pop );
            _ = cursor.Emit( OpCodes.Pop );
            ILLabel breakLabel = null;
            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchBltUn( out breakLabel ) );
            cursor.Index = 0;
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldfld, characterBody );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldfld, maxDistance );
            _ = cursor.EmitDelegate<Func<CharacterBody, Single, HurtBox>>( CastTarget );
            _ = cursor.Emit( OpCodes.Dup );
            _ = cursor.EmitDelegate<Func<HurtBox,Boolean>>( (hb) => hb );
            _ = cursor.Emit( OpCodes.Brfalse, normalPath );
            _ = cursor.GotoLabel( normalPath, MoveType.Before );
            _ = cursor.Emit( OpCodes.Br, breakLabel );
        }
    }
}

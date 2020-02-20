using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using RogueWispPlugin.Helpers;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispBurnManager : NetworkBehaviour
        {
            private static Dictionary<UInt32, BurnEffectController.EffectParams> skinBurnParamsCache = new Dictionary<UInt32, BurnEffectController.EffectParams>();

            private Boolean dirty;
            private HashSet<UInt32> activeBurns = new HashSet<UInt32>();
            private HashSet<UInt32> oldBurns = new HashSet<UInt32>();

            private readonly Dictionary<UInt32, BurnTimer> skinTimers = new Dictionary<UInt32, BurnTimer>();
            private readonly Dictionary<UInt32, BurnEffectController.EffectParams> skinParams = new Dictionary<UInt32, BurnEffectController.EffectParams>();
            private readonly Dictionary<UInt32, BurnEffectController> skinEffects = new Dictionary<UInt32, BurnEffectController>();

            private GameObject target;

            private Boolean dead = false;

            public override Boolean OnSerialize( NetworkWriter writer, Boolean initialState )
            {
                if( initialState || this.dirty )
                {
                    writer.Write( this.activeBurns.Count );
                    foreach( var v in this.activeBurns )
                    {
                        writer.Write( v );
                    }
                    this.dirty = false;
                    return true;
                }
                return false;
            }

            public override void OnDeserialize( NetworkReader reader, Boolean initialState )
            {
                var count = reader.ReadInt32();
                var temp = this.oldBurns;
                this.oldBurns = this.activeBurns;
                this.activeBurns = temp;
                this.activeBurns.Clear();
                for( Int32 i = 0; i < count; ++i )
                {
                    var val = reader.ReadUInt32();
                    this.activeBurns.Add( val );
                    if( this.oldBurns.Contains( val ) )
                    {
                        this.oldBurns.Remove( val );
                    } else
                    {
                        this.ActivateBurn( val );
                    }
                }

                foreach( var val in this.oldBurns )
                {
                    this.DeactivateBurn( val );
                }
            }

            private void ActivateBurn( UInt32 skin )
            {
                Main.LogI( skin + " Activate" );
                if( this.skinEffects.ContainsKey( skin ) )
                {
                    return;
                    //this.skinEffects[skin].enabled = true;
                    //Destroy( this.skinEffects[skin] );
                    //this.skinEffects.Remove( skin );
                } else
                {
                    //var burnController = base.gameObject.AddComponent<BurnEffectController>();
                    //burnController.target = this.target;
                    //burnController.effectType = GetSkinParams( skin );
                    //this.skinEffects[skin] = burnController;
                }
                var burnController = base.gameObject.AddComponent<BurnEffectController>();
                burnController.target = this.target;
                burnController.effectType = GetSkinParams( skin );
                this.skinEffects[skin] = burnController;
            }

            private static BurnEffectController.EffectParams GetSkinParams( UInt32 skin )
            {
                if( !skinBurnParamsCache.ContainsKey( skin ) )
                {
                    var tempSkin = WispBitSkin.GetWispSkin( skin );
                    skinBurnParamsCache[skin] = tempSkin.burnParams;
                }

                return skinBurnParamsCache[skin];
            }


            private void DeactivateBurn( UInt32 skin )
            {
                Main.LogI( skin + " Deactivate" );
                if( this.skinEffects.ContainsKey( skin ) )
                {
                    Main.LogI( "Things" );
                    //this.skinEffects[skin].enabled = false;
                    Destroy( this.skinEffects[skin] );
                    this.skinEffects.Remove( skin );
                }
            }

            public void Start()
            {
                ModelLocator ml = this.GetComponent<ModelLocator>();
                if( ml && ml.modelTransform ) this.target = ml.modelTransform.gameObject; else this.dead = true;
            }

            public void FixedUpdate()
            {
                if( this.dead ) return;
                this.UpdateTimers( Time.fixedDeltaTime );
            }

            public void SetSkinDuration( UInt32 skin, Single duration )
            {
                if( this.skinTimers.ContainsKey( skin ) )
                {
                    var timer = this.skinTimers[skin];
                    timer.timer = Mathf.Max( timer.timer, duration );
                } else
                {
                    this.skinTimers[skin] = new BurnTimer( duration );
                    this.activeBurns.Add( skin );
                    this.ActivateBurn( skin );
                }
            }

            private void UpdateTimers( Single delta )
            {
                if( !NetworkServer.active ) return;

                var temp = new Queue<UInt32>();

                foreach( var burnKV in this.skinTimers )
                {
                    if( burnKV.Value.PassTime(delta) )
                    {
                        temp.Enqueue( burnKV.Key );
                        this.dirty = true;
                    }
                }

                while( temp.Count > 0 )
                {
                    var key = temp.Dequeue();

                    this.skinTimers.Remove( key );
                    this.activeBurns.Remove( key );

                    this.DeactivateBurn( key );
                }
            }

            private class BurnTimer
            {
                internal Single timer;
                internal BurnTimer( Single duration )
                {
                    this.timer = duration;
                }

                internal Boolean PassTime( Single delta )
                {
                    this.timer -= delta;
                    return this.timer <= 0f;
                }
            }
        }
    }
}

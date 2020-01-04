using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin.Components
{
    public class WispBurnManager : NetworkBehaviour
    {
        [SyncVar]
        private BurnSkinState state = BurnSkinState.None;
        private BurnSkinState localState = BurnSkinState.None;

        private Dictionary<BurnSkinState,Single> skinTimers = new Dictionary<BurnSkinState, Single>();
        private Dictionary<BurnSkinState,BurnEffectController> skinEffects = new Dictionary<BurnSkinState, BurnEffectController>();

        private GameObject target;

        private Boolean dead = false;

        [Flags]
        public enum BurnSkinState
        {
            None = 0,
            Skin0 = 1,
            Skin1 = 2,
            Skin2 = 4,
            Skin3 = 8,
            Skin4 = 16,
            Skin5 = 32,
            Skin6 = 64,
            Skin7 = 128,
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
            this.UpdateLocalState();
        }

        public void SetSkinDuration( UInt32 skin, Single duration )
        {
            BurnSkinState temp = SkinToState(skin);
            if( this.state.HasFlag( temp ) && duration > this.skinTimers[temp] )
            {
                this.skinTimers[temp] = duration;
            } else
            {
                this.ActivateSkin( temp );
                this.skinTimers[temp] = duration;
            }
        }

        public static BurnSkinState SkinToState( UInt32 skin )
        {
            switch( skin )
            {
                case 0u:
                    return BurnSkinState.Skin0;
                case 1u:
                    return BurnSkinState.Skin1;
                case 2u:
                    return BurnSkinState.Skin2;
                case 3u:
                    return BurnSkinState.Skin3;
                case 4u:
                    return BurnSkinState.Skin4;
                case 5u:
                    return BurnSkinState.Skin5;
                case 6u:
                    return BurnSkinState.Skin6;
                case 7u:
                    return BurnSkinState.Skin7;
                default:
                    return BurnSkinState.None;
            }
        }

        private void UpdateTimers( Single delta )
        {
            if( !NetworkServer.active ) return;
            BurnSkinState temp;
            for( UInt32 i = 0; i < 8; i++ )
            {
                temp = SkinToState( i );
                if( this.state.HasFlag( temp ) )
                {
                    this.skinTimers[temp] -= delta;

                    if( this.skinTimers[temp] <= 0f )
                    {
                        this.skinTimers[temp] = 0f;
                        this.DeactivateSkin( temp );
                    }
                }
            }
        }

        private void UpdateLocalState()
        {
            if( this.state == this.localState ) return;

            BurnSkinState temp;

            for( UInt32 i = 0; i < 8; i++ )
            {
                temp = SkinToState( i );

                if( this.state.HasFlag( temp ) && this.localState.HasFlag( temp ) ) continue;
                if( !this.state.HasFlag( temp ) && !this.localState.HasFlag( temp ) ) continue;

                if( this.state.HasFlag( temp ) )
                {
                    if( !this.skinEffects.ContainsKey( temp ) || this.skinEffects[temp] == null ) this.skinEffects[temp] = this.gameObject.AddComponent<BurnEffectController>();

                    this.skinEffects[temp].effectType = Main.burnOverlayParams[i];
                    this.skinEffects[temp].target = this.target;
                } else
                {
                    if( this.skinEffects.ContainsKey( temp ) && this.skinEffects[temp] ) Destroy( this.skinEffects[temp] );
                    this.skinEffects[temp] = null;
                }

            }

            this.localState = this.state;
        }

        private void ActivateSkin( BurnSkinState state )
        {
            if( this.state.HasFlag( state ) ) return;
            this.state |= state;
        }

        private void DeactivateSkin( BurnSkinState state )
        {
            if( !this.state.HasFlag( state ) ) return;
            this.state &= ~state;
        }
    }
}

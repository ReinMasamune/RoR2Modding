using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Components
{
    public class WispBurnManager : NetworkBehaviour
    {
        [SyncVar]
        BurnSkinState state = BurnSkinState.None;

        BurnSkinState localState = BurnSkinState.None;

        private Dictionary<BurnSkinState,float> skinTimers = new Dictionary<BurnSkinState, float>();
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
            var ml = GetComponent<ModelLocator>();
            if( ml && ml.modelTransform ) target = ml.modelTransform.gameObject; else dead = true; 
        }

        public void FixedUpdate()
        {
            if( dead ) return;
            UpdateTimers( Time.fixedDeltaTime );
            UpdateLocalState();
        }

        public void SetSkinDuration( uint skin, float duration )
        {
            BurnSkinState temp = SkinToState(skin);
            if( state.HasFlag( temp ) && duration > skinTimers[temp] ) skinTimers[temp] = duration;
            else
            {
                ActivateSkin( temp );
                skinTimers[temp] = duration;
            }
        }

        public static BurnSkinState SkinToState( uint skin )
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

        private void UpdateTimers( float delta )
        {
            if( !NetworkServer.active ) return;
            BurnSkinState temp;
            for( uint i = 0; i < 8; i++ )
            {
                temp = SkinToState( i );
                if( state.HasFlag( temp ) )
                {
                    skinTimers[temp] -= delta;

                    if( skinTimers[temp] <= 0f )
                    {
                        skinTimers[temp] = 0f;
                        DeactivateSkin( temp );
                    }
                }
            }
        }

        private void UpdateLocalState()
        {
            if( state == localState ) return;

            BurnSkinState temp;

            for( uint i = 0; i < 8; i++ )
            {
                temp = SkinToState( i );

                if( state.HasFlag( temp ) && localState.HasFlag( temp ) ) continue;
                if( !state.HasFlag( temp ) && !localState.HasFlag( temp ) ) continue;

                if( state.HasFlag( temp ) )
                {
                    if( !skinEffects.ContainsKey(temp) || skinEffects[temp] == null ) skinEffects[temp] = gameObject.AddComponent<BurnEffectController>();

                    skinEffects[temp].effectType = Modules.WispMaterialModule.burnOverlayParams[i];
                    skinEffects[temp].target = target;
                } else
                {
                    if( skinEffects.ContainsKey( temp ) && skinEffects[temp] ) Destroy( skinEffects[temp] );
                    skinEffects[temp] = null;
                }
                
            }

            localState = state;
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

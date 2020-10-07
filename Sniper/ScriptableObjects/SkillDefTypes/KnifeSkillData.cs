namespace Rein.Sniper.SkillDefs
{
    using System;
    using EntityStates;
    using Rein.Sniper.Components;
    using Rein.Sniper.SkillDefTypes.Bases;
    using Rein.Sniper.States.Special;
    using UnityEngine;

    internal class KnifeSkillData : SkillData
    {
        internal static SerializableEntityStateType slashState;
        internal static InterruptPriority interruptPriority;
        internal static String targetMachineName;

        internal override String targetStateMachineName { get; } = targetMachineName;

        internal EntityState InstantiateNextState( KnifeDeployableSync knife )
        {
            var state = EntityState.Instantiate( slashState );
            ( state as KnifePickupSlash ).knifeObject = knife.gameObject;
            return state;
        }

        internal void Initialize( KnifeDeployableSync knifeInstance )
        {
            this.knifeInstance = knifeInstance;
            this.initialized = true;
        }

        internal SkinnedMeshRenderer knifeRenderer { get; set; }    
        internal KnifeDeployableSync knifeInstance { get; private set; }

        private Boolean initialized = false;

        internal override Boolean IsDataValid()
        {
            return !(this.knifeInstance is null);
        }

        internal override Boolean IsDataInitialized()
        {
            return this.initialized;
        }

        internal override void OnInvalidate()
        {
            if( this.knifeRenderer is null )
            {
                return;
            }
            this.knifeRenderer.enabled = true;
        }
    }
}

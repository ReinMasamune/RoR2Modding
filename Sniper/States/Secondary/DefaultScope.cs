using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Enums;
using Sniper.Data;
using UnityEngine.Networking;
using Sniper.States.Bases;

namespace Sniper.States.Secondary
{
    internal class DefaultScope : ScopeBaseState
    {
        const Single maxCharge = 1f;
        const Single chargePerSecond = 0.15f;

        internal override Boolean usesCharge { get; } = true;
        internal override Boolean usesStock { get; } = false;
        internal override Single currentCharge { get => this.charge; }
        internal override UInt32 currentStock { get; } = 0u;
        internal Single charge = 0f;

        // TODO: Implement
        internal override void OnFired()
        {
            this.charge = 0f;
        }
        internal override BulletModifier ReadModifier()
        {
            return default;
        }


        public override void OnEnter()
        {
            base.OnEnter();

            if( NetworkServer.active )
                characterBody.AddBuff( BuffIndex.Slow50 );
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( this.charge < maxCharge )
                this.charge += Time.fixedDeltaTime * chargePerSecond * characterBody.attackSpeed;
            else
            {
                this.charge = maxCharge;
            }
        }

        public override void Update()
        {
            base.Update();
        }


        public override void OnExit()
        {
            base.OnExit();

            if( NetworkServer.active )
                characterBody.RemoveBuff( BuffIndex.Slow50 );
        }
    }
}

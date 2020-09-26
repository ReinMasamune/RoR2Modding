namespace Sniper.States.Bases
{
    using System;
    using System.Diagnostics;

    using EntityStates;

    using Sniper.Data;
    using Sniper.Enums;
    using Sniper.Expansions;
    using Sniper.Modules;
    using Sniper.SkillDefs;

    using UnityEngine;
    using UnityEngine.Networking;

    internal interface ISniperPrimaryDataProvider
    {
        Single baseDuration { get; }
        Single recoilStrength { get; }
        Single damageMultiplier { get; }
        Single forceMultiplier { get; }
        Single upBoostForce { get; }
        String muzzleName { get; }
    }
    internal interface ISnipeState
    {
        Single reloadBoost { get; set; }
        Single chargeBoost { get; set; }
        Single baseDuration { get; }
        Single recoilStrength { get; }
        Single damageMultiplier { get; }
        Single forceMultiplier { get; }
        Single soundFrac { get; }
        String muzzleName { get; }
    }
    internal class SnipeState<TStateData> : SniperSkillBaseState, ISnipeState
        where TStateData : struct, ISniperPrimaryDataProvider
    {
        private static readonly Single _baseDuration;
        private static readonly Single _recoilStrength;
        private static readonly Single _damageMultiplier;
        private static readonly Single _forceMultiplier;
        private static readonly Single _upBoostForce;
        private static readonly String _muzzleName;

        static SnipeState()
        {
            var provider = new TStateData();
            _baseDuration = provider.baseDuration;
            _recoilStrength = provider.recoilStrength;
            _damageMultiplier = provider.damageMultiplier;
            _forceMultiplier = provider.forceMultiplier;
            _upBoostForce = provider.upBoostForce;
            _muzzleName = provider.muzzleName;
        }

        public Single baseDuration => _baseDuration;
        public Single recoilStrength => _recoilStrength;
        public Single damageMultiplier => _damageMultiplier;
        public Single forceMultiplier => _forceMultiplier;
        public Single upBoostForce => _upBoostForce;
        public Single reloadBoost { get; set; }
        public Single chargeBoost { get; set; }
        public Single soundFrac { get; set; }
        public String muzzleName => _muzzleName;


        private IAmmoStateContext context;
        private SniperAmmoSkillDef ammo;
        private Int32 ammoId;

        public override void OnEnter()
        {
            base.OnEnter();
            if(base.isAuthority)
            {
                var data = base.characterBody?.scopeInstanceData;
                var mod = data.shouldModify ? data?.SendFired() ?? default : default;
                this.chargeBoost = mod.damageMultiplier;
                this.soundFrac = mod.charge;
                this.ammo = base.characterBody.ammo;
                this.ammoId = ammo.id;
                this.context = this.ammo.GetContext();
            }
            if(!base.characterMotor.isGrounded)
            {
                this.characterMotor.ApplyForce(Vector3.up * this.upBoostForce);
            }
            this.context.OnEnter(this);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.context.FixedUpdate(this);
        }

        public override void OnExit()
        {
            this.context.OnExit(this);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.soundFrac);
            writer.Write(this.reloadBoost);
            writer.Write(this.chargeBoost);
            writer.Write(this.ammoId);
            this.context.OnSerialize(this, writer);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.soundFrac = reader.ReadSingle();
            this.reloadBoost = reader.ReadSingle();
            this.chargeBoost = reader.ReadSingle();
            this.ammoId = reader.ReadInt32();
            this.ammo = SniperAmmoSkillDef.FromID(this.ammoId);
            this.context = this.ammo.GetContext();
            this.context.OnDeserialize(this, reader);
        }
    }
}

namespace Sniper.SkillDefs
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using EntityStates;

    using JetBrains.Annotations;

    using ReinCore;

    using RoR2;

    using Sniper.Components;
    using Sniper.Enums;
    using Sniper.Expansions;
    using Sniper.Modules;
    using Sniper.SkillDefTypes.Bases;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal delegate ExpandableBulletAttack BulletCreationDelegate(SniperCharacterBody body, ReloadTier reload, Ray aim, String muzzleName);
    internal delegate void ChargeBulletModifierDelegate(ExpandableBulletAttack bullet);
    internal delegate IAmmoStateContext CreateContextDelegate();



    internal interface IAmmoStateContext
    {
        GameObject tracerEffectPrefab { get; }
        void OnEnter<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider;
        void FixedUpdate<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider;
        void OnSerialize<T>(SnipeState<T> state, NetworkWriter writer) where T : struct, ISniperPrimaryDataProvider;
        void OnDeserialize<T>(SnipeState<T> state, NetworkReader reader) where T : struct, ISniperPrimaryDataProvider;
        void OnExit<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider;
    }

    internal class SniperAmmoSkillDef : SniperSkillDef
    {
        private static readonly List<SniperAmmoSkillDef> instances = new();
        internal static SniperAmmoSkillDef FromID(Int32 id) => instances[id];

        internal static SniperAmmoSkillDef Create<TContext>()
            where TContext : IAmmoStateContext, new()
        {
            SniperAmmoSkillDef def = ScriptableObject.CreateInstance<SniperAmmoSkillDef>();

            var tracer = new TContext().tracerEffectPrefab;
            //if(tracer is not null) Log.Message("TracerInit");
            static IAmmoStateContext CreateContext() => new TContext();
            def.createContext = CreateContext;


            def.activationState = SkillsCore.StateType<Idle>();
            def.activationStateMachineName = "";
            def.baseMaxStock = 0;
            def.baseRechargeInterval = 0f;
            def.beginSkillCooldownOnSkillEnd = false;
            def.canceledFromSprinting = false;
            def.fullRestockOnAssign = false;
            def.interruptPriority = InterruptPriority.Any;
            def.isBullets = false;
            def.isCombatSkill = false;
            def.mustKeyPress = false;
            def.noSprint = false;
            def.rechargeStock = 0;
            def.requiredStock = 0;
            def.shootDelay = 0f;
            def.stockToConsume = 0;

            return def;
        }

        protected void Awake()
        {
            this.id = instances.Count;
            instances.Add(this);
        }




        //private BulletCreationDelegate createBullet;
        //private ChargeBulletModifierDelegate chargeModifier;
        private CreateContextDelegate createContext;
        internal SoundModule.FireType fireSoundType;

        internal Int32 id { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IAmmoStateContext GetContext()
        {
            return this.createContext();
        }

        public sealed override void OnFixedUpdate([NotNull] GenericSkill skillSlot)
        {

        }
    }
}

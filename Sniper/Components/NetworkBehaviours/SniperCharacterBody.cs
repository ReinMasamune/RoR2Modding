namespace Rein.Sniper.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Mono.Cecil.Cil;

    using MonoMod.Utils;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Data;
    using Rein.Sniper.Enums;
    using Rein.Sniper.Modules;
    using Rein.Sniper.SkillDefs;
    using Rein.Sniper.UI.Components;


    using UnityEngine;
    using UnityEngine.Events;

    internal class SniperCharacterBody : CharacterBody
    {
        private static readonly GameObject decoyMaster;
        static SniperCharacterBody()
        {
            decoyMaster = DecoyModule.GetDecoyMaster();

            //var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            //var dmd1 = new DynamicMethodDefinition( "base_Start<<<SniperCharacterBody", null, new[] { typeof(CharacterBody) } );
            //var proc1 = dmd1.GetILProcessor();
            //proc1.Emit( OpCodes.Jmp, typeof( CharacterBody ).GetMethod( "Start", flags ) );
            //base_Start = (Action<CharacterBody>)dmd1.Generate().CreateDelegate<Action<CharacterBody>>();
        }
        //private static Action<CharacterBody> base_Start;
        //private static Action<CharacterBody> base_Awake;
        //private static Action<CharacterBody> base_Update;
        internal void CheckIn(SniperReloadableFireSkillDef.SniperPrimaryInstanceData primaryData)
        {
            this.primaryData = primaryData;
        }
        internal SniperReloadableFireSkillDef.SniperPrimaryInstanceData primaryData { get; private set; }



        internal void SendBonusReload(ReloadTier tier)
        {
            this.primaryData?.ForceReload(tier);
        }


        private Coroutine startReloadRoutine;
        internal void StartReload(ReloadParams reloadParams)
        {
            this.curReloadParams = reloadParams;
            this.reloadTimer = 0f;
            this.startReloadRoutine = base.StartCoroutine(this.ReloadStartDelay(this.curReloadParams.reloadDelay / base.attackSpeed));
        }

        internal ReloadTier ReadReload()
        {
            return this.curReloadParams.GetReloadTier(this.reloadTimer);
        }

        private Coroutine stopReloadRoutine;
        internal void StopReload(SkillDefs.SniperReloadableFireSkillDef.SniperPrimaryInstanceData data)
        {
            this.isReloading = false;
            this.stopReloadRoutine = base.StartCoroutine(this.ReloadStopDelay((this.curReloadParams.reloadEndDelay / base.attackSpeed) + this.curReloadParams.GetFlatDelay(this.reloadTimer, base.attackSpeed), data));
        }

        internal void ForceStopReload()
        {
            this.showBar = false;
            this.isReloading = false;
            if(this.stopReloadRoutine != null) this.StopCoroutine(this.stopReloadRoutine);
            if(this.startReloadRoutine != null) this.StopCoroutine(this.startReloadRoutine);
        }

        internal Boolean CanReload()
        {
            return this.isReloading;
        }


        private IEnumerator ReloadStartDelay(Single delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            this.showBar = true;
            this.isReloading = true;
            SoundModule.PlayOpenReload(base.gameObject);
        }
        private IEnumerator ReloadStopDelay(Single delayTime, SkillDefs.SniperReloadableFireSkillDef.SniperPrimaryInstanceData data)
        {
            yield return new WaitForSeconds(delayTime);

            this.showBar = false;
            data.isReloading = false;
        }

        protected new void Start()
        {
            base.Start();
            (this.skillLocator.primary.skillInstanceData as SniperReloadableFireSkillDef.SniperPrimaryInstanceData)?.StartReload(true);

        }

        protected new void Update()
        {
            base.Update();
            this.smoothedAttackSpeed = Mathf.SmoothDamp( this.smoothedAttackSpeed, attackSpeed, ref this.attackSpeedSpeed, 2f);
            if(!Util.HasEffectiveAuthority(base.gameObject)) return;
            if(!this.isReloading) return;
            this.reloadTimer = this.curReloadParams.Update(Time.deltaTime, this.smoothedAttackSpeed, this.reloadTimer);
        }
        private Single smoothedAttackSpeed;
        private Single attackSpeedSpeed;
        private Single attackSpeedSmoother;

        protected new void OnDestroy()
        {
            base.OnDestroy();
            if(!this.reloadUI || this.reloadUI is null) return;
            this.reloadUI.showBar = false;
        }


        private Boolean isReloading = false;
        private Single _reloadTimer = 0.0f;
        private Single reloadTimer
        {
            get => this._reloadTimer;
            set
            {
                this._reloadTimer = value;
                if(this.reloadUI is null) return;
                this.reloadUI.barPosition = value / this.curReloadParams.baseDuration;
            }
        }

        private Boolean _showBar;
        private Boolean showBar
        {
            get => this._showBar;
            set
            {
                if(value == this.showBar) return;
                this._showBar = value;
                if(this.reloadUI is null) return;
                this.reloadUI.showBar = value;
            }
        }

        private Single _barPos;
        private Single barPos
        {
            get => this._barPos;
            set
            {
                this._barPos = value;
                if(this.reloadUI is null) return;
                this.reloadUI.barPosition = value;
            }
        }

        private ReloadParams _curReloadParams;
        private ReloadParams curReloadParams
        {
            get => this._curReloadParams;
            set
            {
                this._curReloadParams = value;
                if(this.reloadUI is null) return;
                this.reloadUI.currentParams = value;
            }
        }

        private ReloadUIController _reloadUI;
        internal ReloadUIController reloadUI
        {
            private get => this._reloadUI;
            set
            {
                this._reloadUI = value;
                value.barPosition = this.barPos;
                value.showBar = this.showBar;
                value.currentParams = this.curReloadParams;
            }
        }

        private SniperCrosshairController _sniperCrosshairController;
        internal SniperCrosshairController sniperCrosshair
        {
            get => this._sniperCrosshairController;
            set
            {
                //Log.WarningT( "Crosshair checked in" );

                this._sniperCrosshairController = value;

                if(value is null) return;

                var primary = base.skillLocator.primary;
                var primaryData = primary.skillInstanceData as SniperReloadableFireSkillDef.SniperPrimaryInstanceData;
                primaryData.crosshair = this.sniperCrosshair;

                var secondary = base.skillLocator.secondary;
                var secondaryData = secondary.skillInstanceData as SniperScopeSkillDef.ScopeInstanceData;
                secondaryData.crosshair = this.sniperCrosshair;

                //Log.WarningT( "Checkin end" );
            }
        }


        internal SniperAmmoSkillDef ammo
        {
            get
            {
                if(this._ammo == null)
                {
                    GenericSkill slot = this.ammoSlot;
                    this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammo;
            }
        }
        private SniperAmmoSkillDef _ammo;

        internal GenericSkill ammoSlot
        {
            get
            {
                if(this._ammoSlot == null)
                {
                    this._ammoSlot = base.skillLocator.GetSkillAtIndex(0);
                    this._ammoSlot.onSkillChanged += (slot) => this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammoSlot;
            }
        }
        private GenericSkill _ammoSlot;


        //internal SniperPassiveSkillDef passive
        //{
        //    get
        //    {
        //        if(this._passive == null)
        //        {
        //            GenericSkill slot = this.passiveSlot;
        //            this._passive = slot.skillDef as SniperPassiveSkillDef;
        //        }
        //        return this._passive;
        //    }
        //}
        //private SniperPassiveSkillDef _passive;

        //internal GenericSkill passiveSlot
        //{
        //    get
        //    {
        //        if(this._passiveSlot == null)
        //        {
        //            this._passiveSlot = base.skillLocator.GetSkillAtIndex(1);
        //            this._passiveSlot.onSkillChanged += (slot) => this._passive = slot.skillDef as SniperPassiveSkillDef;
        //        }
        //        return this._passiveSlot;
        //    }
        //}
        //private GenericSkill _passiveSlot;

        internal SniperScopeSkillDef.ScopeInstanceData scopeInstanceData
        {
            get => base.skillLocator.secondary.skillInstanceData as SniperScopeSkillDef.ScopeInstanceData;
        }

        [field: SerializeField]
        internal Transform scopeAimOriginParent { get; set; }



        internal unsafe void SummonDecoy(Vector3 position, Quaternion rotation)
        {
            ItemIndex* indicies = stackalloc ItemIndex[ItemCatalog.itemCount];

#if ASSERT
            if( !NetworkServer.active )
            {
                Log.ErrorL( "Called on server" );
                return;
            }
#endif

            CharacterMaster summoningMaster = base.master;
            if(summoningMaster == null)
            {
                return;
            }

            CharacterMaster summonedMaster = new MasterSummon
            {
                masterPrefab = decoyMaster,
                position = position,
                rotation = rotation,
                summonerBodyObject = base.gameObject,
                ignoreTeamMemberLimit = true,
                preSpawnSetupCallback = DecoySummonPreSetup
            }.Perform();

            Inventory masterInv = summoningMaster.inventory;
            Inventory decoyInv = summonedMaster.inventory;

            decoyInv.CopyEquipmentFrom(masterInv);
            decoyInv.CopyItemsFrom(masterInv);

            UInt32 counter = 0u;
            foreach(ItemIndex index in decoyInv.itemAcquisitionOrder)
            {
                if(!DecoyModule.whitelist.Contains(index))
                {
                    indicies[counter++] = index;
                }
            }

            for(Int32 i = 0; i < counter; ++i)
            {
                decoyInv.ResetItem(indicies[i]);
            }

            Deployable deployable = summonedMaster.AddComponent<Deployable>();
            deployable.onUndeploy = new UnityEvent();
            deployable.onUndeploy.AddListener(new UnityAction(summonedMaster.TrueKill));
            summoningMaster.AddDeployable(deployable, DecoyModule.deployableSlot);
        }

        private static void DecoySummonPreSetup(CharacterMaster master)
        {
            // I suppose this is not needed? interesting
        }
    }
}

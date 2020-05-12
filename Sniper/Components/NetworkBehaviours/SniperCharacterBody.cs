namespace Sniper.Components
{
    using System;

    using ReinCore;

    using RoR2;

    using Sniper.Modules;
    using Sniper.SkillDefs;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Networking;

    internal class SniperCharacterBody : CharacterBody
    {
        private static readonly GameObject decoyMaster;
        static SniperCharacterBody()
        {
            decoyMaster = DecoyModule.GetDecoyMaster();
        }



        internal SniperAmmoSkillDef ammo
        {
            get
            {
                if( this._ammo == null )
                {
                    GenericSkill slot = this.ammoSlot;
                    this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammo;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private SniperAmmoSkillDef _ammo;
#pragma warning restore IDE1006 // Naming Styles

        private GenericSkill ammoSlot
        {
            get
            {
                if( this._ammoSlot == null )
                {
                    this._ammoSlot = base.skillLocator.GetSkillAtIndex( 0 );
                    this._ammoSlot.onSkillChanged += ( slot ) => this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammoSlot;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private GenericSkill _ammoSlot;
#pragma warning restore IDE1006 // Naming Styles


        internal SniperPassiveSkillDef passive
        {
            get
            {
                if( this._passive == null )
                {
                    GenericSkill slot = this.passiveSlot;
                    this._passive = slot.skillDef as SniperPassiveSkillDef;
                }
                return this._passive;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private SniperPassiveSkillDef _passive;
#pragma warning restore IDE1006 // Naming Styles

        private GenericSkill passiveSlot
        {
            get
            {
                if( this._passiveSlot == null )
                {
                    this._passiveSlot = base.skillLocator.GetSkillAtIndex( 1 );
                    this._passiveSlot.onSkillChanged += ( slot ) => this._passive = slot.skillDef as SniperPassiveSkillDef;
                }
                return this._passiveSlot;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private GenericSkill _passiveSlot;
#pragma warning restore IDE1006 // Naming Styles

        internal SniperScopeSkillDef.ScopeInstanceData scopeInstanceData
        {
            get => base.skillLocator.secondary.skillInstanceData as SniperScopeSkillDef.ScopeInstanceData;
        }

        [field: SerializeField]
        internal Transform scopeAimOriginParent { get; set; }



        internal unsafe void SummonDecoy( Vector3 position, Quaternion rotation )
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
            if( summoningMaster == null )
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

            decoyInv.CopyEquipmentFrom( masterInv );
            decoyInv.CopyItemsFrom( masterInv );

            UInt32 counter = 0u;
            foreach( ItemIndex index in decoyInv.itemAcquisitionOrder )
            {
                if( !DecoyModule.whitelist.Contains( index ) )
                {
                    indicies[counter++] = index;
                }
            }

            for( Int32 i = 0; i < counter; ++i )
            {
                decoyInv.ResetItem( indicies[i] );
            }

            Deployable deployable = summonedMaster.AddComponent<Deployable>();
            deployable.onUndeploy = new UnityEvent();
            deployable.onUndeploy.AddListener( new UnityAction( summonedMaster.TrueKill ) );
            summoningMaster.AddDeployable( deployable, DecoyModule.deployableSlot );
        }

        private static void DecoySummonPreSetup( CharacterMaster master )
        {
            // TODO: Implement, most likely needs to adjust and apply loadout? May need to specifically capture a loadout instance for this.
        }
    }
}

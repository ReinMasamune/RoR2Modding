using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.SkillDefs;
using UnityEngine.Networking;
using Sniper.Modules;
using UnityEngine.Events;

namespace Sniper.Components
{
    internal class SniperCharacterBody : CharacterBody
    {
        private static GameObject decoyMaster;
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
                    var slot = this.ammoSlot;
                    this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammo;
            }
        }
        private SniperAmmoSkillDef _ammo;

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
        private GenericSkill _ammoSlot;


        internal SniperPassiveSkillDef passive
        {
            get
            {
                if( this._passive == null )
                {
                    var slot = this.passiveSlot;
                    this._passive = slot.skillDef as SniperPassiveSkillDef;
                }
                return this._passive;
            }
        }
        private SniperPassiveSkillDef _passive;

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
        private GenericSkill _passiveSlot;

        internal SniperScopeSkillDef.ScopeInstanceData scopeInstanceData
        {
            get => base.skillLocator.secondary.skillInstanceData as SniperScopeSkillDef.ScopeInstanceData;
        }

        [field:SerializeField]
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
            if( summoningMaster == null ) return;

            CharacterMaster summonedMaster = new MasterSummon
            {
                masterPrefab = decoyMaster,
                position = position,
                rotation = rotation,
                summonerBodyObject = base.gameObject,
                ignoreTeamMemberLimit = true,
                preSpawnSetupCallback = DecoySummonPreSetup
            }.Perform();

            var masterInv = summoningMaster.inventory;
            var decoyInv = summonedMaster.inventory;

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

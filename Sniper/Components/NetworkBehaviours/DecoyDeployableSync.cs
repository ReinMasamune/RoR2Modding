namespace Rein.Sniper.Components
{
    using RoR2;
    using Rein.Sniper.SkillDefs;
    using UnityEngine;
    using UnityEngine.Networking;
    internal class DecoyDeployableSync : NetworkBehaviour, IRuntimePrefabComponent
    {
        internal void BodyKilled()
        {
            if( NetworkClient.active )
            {
                if( this.owner?.GetBody()?.skillLocator?.special?.skillInstanceData is DecoySkillDef.ReactivationInstanceData inst )
                {
                    inst.InvalidateReactivation();
                }
            }
        }

        [SerializeField]
        private CharacterBody body;


        private CharacterMaster master;
        private CharacterMaster owner;
        private void Awake()
        {

#if ASSERT
            if( this.body == null )
            {
                Log.ErrorL( "Body was null" );
            }
#endif
        }

        private void Start()
        {
            this.master = this.body.master;
#if ASSERT
            if( this.master == null )
            {
                Log.ErrorL( "Master was null" );
            }
#endif

            this.owner = this.master.minionOwnership?.ownerMaster;
#if ASSERT
            if( this.owner == null )
            {
                Log.ErrorL( "Owner was null" );
            }
#endif
        }

        #region Prefab only
        void IRuntimePrefabComponent.InitializePrefab() => this.body = base.gameObject.GetComponent<CharacterBody>();

        #endregion
    }
}

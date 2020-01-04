namespace AlternativeAcrid
{
    using BepInEx;
    using RoR2;
    using System;
    using UnityEngine;


    [R2API.Utils.R2APISubmoduleDependency(nameof(R2API.EntityAPI), nameof( R2API.EffectAPI ), nameof( R2API.PrefabAPI ), nameof( R2API.SkillAPI ), nameof( R2API.SkinAPI ))]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.AltAcrid", "Rein-AlternativeAcrid", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        private GameObject acridBody;

        private void Awake()
        {
            acridBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CrocoBody" );
            if( acridBody == null ) Debug.Log( "You're Fucked" );
            CreateProjectiles();
            RegisterSkills();
        }
        private void OnEnable()
        {
            AddHooks();
        }
        private void OnDisable()
        {
            RemoveHooks();
        }
        private void Start()
        {
            BuffCatalog.GetBuffDef( BuffIndex.Poisoned ).canStack = true;

            var refState2 = (EntityStates.AimThrowableBase)EntityStates.EntityState.Instantiate( new EntityStates.SerializableEntityStateType( typeof( EntityStates.Toolbot.AimStunDrone ) ) );


            States.Utility.BasePrepJump.arcPrefab = refState2.arcVisualizerPrefab;
            States.Utility.BasePrepJump.endPrefab = refState2.endpointVisualizerPrefab;
            States.Utility.BasePrepJump.endScale = 5f;
            States.Utility.BasePrepJump.maxDistance = 100f;
            States.Utility.BasePrepJump.rayRadius = 0.25f;
            States.Utility.BasePrepJump.useGravity = true;
            States.Utility.BasePrepJump.baseSpeed = 30f;

        }
        private void FixedUpdate()
        {

        }
        private void Update()
        {

        }
        private void LateUpdate()
        {

        }
        private void OnGUI()
        {

        }


    }
}

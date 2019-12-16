namespace AlternateArtificer
{
    using BepInEx;
    using RoR2;
    using System;
    using UnityEngine;

    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.AltArti", "Rein-AlternateArtificer", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        private GameObject artiBody;
        private SkillLocator artiSkillLocator;
        private void Awake()
        {
            artiBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/MageBody" );
            artiSkillLocator = artiBody.GetComponent<SkillLocator>();

            EditModel();
            EditSkills();
            EditComponents();
            EditProjectiles();

            CreateProjectiles();
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

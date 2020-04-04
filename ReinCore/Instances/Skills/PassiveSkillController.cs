using System;
using BepInEx;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace ReinCore
{
    [RequireComponent(typeof(SkillLocator),typeof(CharacterBody))]
    public abstract class PassiveSkillController : MonoBehaviour
    {
        #region Inherit
        


        #endregion
        #region Public
        public GenericSkill passiveGenericSkill { get; private set; }
        public PassiveSkillDef currentPassiveSkill { get; private set; }
        public CharacterBody body { get; private set; }
        public SkillLocator skillLocator { get; private set; }
        


        #endregion
        #region Internal
        private void Awake()
        {

        }

        #endregion
        #region Statics
        private static Accessor<SkillLocator,GenericSkill[]> access_allSkills = new Accessor<SkillLocator,GenericSkill[]>( "allSkills" );

        #endregion
    }
}

//using System;
//using BepInEx;
//using RoR2;
//using RoR2.Skills;
//using UnityEngine;

//namespace ReinCore
//{
//    [RequireComponent(typeof(SkillLocator),typeof(CharacterBody))]
//    public abstract class PassiveSkillController : MonoBehaviour
//    {
//        #region Inherit
        


//        #endregion
//        #region Public
//        public GenericSkill passiveGenericSkill { get; private set; }
//        public PassiveSkillDefBase currentPassiveSkill { get; private set; }
//        public CharacterBody body { get; private set; }
//        public SkillLocator skillLocator { get; private set; }

//        public event PassiveSkillChangedDelegate onPassiveSkillChanged;
//        public delegate void PassiveSkillChangedDelegate( GenericSkill passiveSlot, PassiveSkillDefBase previousPassive, PassiveSkillDefBase newPassive );


//        #endregion
//        #region Internal
//        private void Awake()
//        {
//            this.body = base.GetComponent<CharacterBody>();
//            this.skillLocator = base.GetComponent<SkillLocator>();
//            this.passiveGenericSkill = base.GetComponent<GenericSkill>();

//            this.passiveGenericSkill.onSkillChanged += this.OnSkillChanged;
//        }

//        private void OnSkillChanged( GenericSkill slot )
//        {
//            var prevSkill = this.currentPassiveSkill;
//            var newSkill = slot.skillDef as PassiveSkillDefBase;
//            this.onPassiveSkillChanged?.Invoke( slot, prevSkill, newSkill );
//        }

        

//        #endregion
//        #region Statics
//        private static Accessor<SkillLocator,GenericSkill[]> access_allSkills = new Accessor<SkillLocator,GenericSkill[]>( "allSkills" );

//        #endregion
//    }
//}

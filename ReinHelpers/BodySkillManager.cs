namespace ReinHelpers
{
    using RoR2;
    using RoR2.Skills;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BodySkillManager
    {
        #region Internal vars
        private GameObject body;

        private SkillLocator skillLocator;

        private GenericSkill passive;
        private GenericSkill primary;
        private GenericSkill secondary;
        private GenericSkill utility;
        private GenericSkill special;

        private SkillFamily passiveFamily;
        private SkillFamily primaryFamily;
        private SkillFamily secondaryFamily;
        private SkillFamily utilityFamily;
        private SkillFamily specialFamily;


        private NetworkStateMachine networkStateMachine;
        private SetStateOnHurt setStateOnHurt;
        private Dictionary<String,EntityStateMachine> stateMachines;

        #endregion

        #region Constructor
        public BodySkillManager( GameObject body )
        {
            this.body = body;

            this.skillLocator = this.body.AddOrGetComponent<SkillLocator>();



        }

        #endregion
    }
}

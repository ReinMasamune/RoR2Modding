namespace Sniper.SkillDefs
{
    using System;

    using EntityStates;

    using JetBrains.Annotations;

    using ReinCore;

    using RoR2;

    using Sniper.Expansions;
    using Sniper.SkillDefTypes.Bases;

    using UnityEngine;

    internal class SniperPassiveSkillDef : SniperSkillDef
    {

        internal static SniperPassiveSkillDef Create( Data.BulletModifier modifier, Boolean headshots, Single critMultiplier, Single critPerLevel )
        {
            SniperPassiveSkillDef def = ScriptableObject.CreateInstance<SniperPassiveSkillDef>();

            def.bulletModifier = modifier;
            def.canHeadshot = headshots;
            def.onCritDamageMultiplier = critMultiplier;


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
            def.critPerLevel = critPerLevel;

            return def;
        }


        [SerializeField]
        private Data.BulletModifier bulletModifier;
        [SerializeField]
        private Boolean canHeadshot;
        [SerializeField]
        private Single onCritDamageMultiplier;
        [SerializeField]
        private Single critPerLevel;

        public override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot )
        {
            var res = base.OnAssigned( skillSlot );
            skillSlot.characterBody.levelCrit += this.critPerLevel;
            return res;
        }


        internal void ModifyBullet( ExpandableBulletAttack bulletAttack )
        {
            if( bulletAttack.isCrit )
            {
                bulletAttack.damage *= this.onCritDamageMultiplier;
            }

            bulletAttack.sniper = this.canHeadshot;
            this.bulletModifier.Apply( bulletAttack );
        }

        public sealed override void OnFixedUpdate([NotNull] GenericSkill skillSlot)
        {

        }
    }
}

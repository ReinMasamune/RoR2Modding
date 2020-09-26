#if ANCIENTWISP
using System;

using RoR2;
using RoR2.CharacterAI;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void AW_SetupAI()
        {
            this.Load += this.AW_SetupPrimaryDriver;
            this.Load += this.AW_SetupSecondaryDriver;
            this.Load += this.AW_SetupUtilityDriver;
            //this.Load += this.AW_SetupRetreatDriver;
            //this.Load += this.AW_SetupAdvanceDriver;
            this.Load += this.AW_SetupStrafeDriver;
            this.Load += this.AW_SetupChaseDriver;
        }

        private void AW_SetupAdvanceDriver() => throw new NotImplementedException();
        private void AW_SetupRetreatDriver() => throw new NotImplementedException();

        private void AW_SetupStrafeDriver()
        {
            var strafeDriver = this.AW_master.AddComponent<AISkillDriver>();
            strafeDriver.customName = "Strafe";
            strafeDriver.skillSlot = SkillSlot.None;
            strafeDriver.requireSkillReady = false;
            strafeDriver.requireEquipmentReady = false;
            strafeDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafeDriver.minUserHealthFraction = Single.NegativeInfinity;
            strafeDriver.maxUserHealthFraction = Single.PositiveInfinity;
            strafeDriver.minTargetHealthFraction = Single.NegativeInfinity;
            strafeDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            strafeDriver.minDistance = 0f;
            strafeDriver.maxDistance = 30f;
            strafeDriver.selectionRequiresTargetLoS = false;
            strafeDriver.activationRequiresTargetLoS = false;
            strafeDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafeDriver.moveInputScale = 1f;
            strafeDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            strafeDriver.ignoreNodeGraph = true;
            strafeDriver.driverUpdateTimerOverride = -1f;
            strafeDriver.resetCurrentEnemyOnNextDriverSelection = false;
            strafeDriver.noRepeat = false;
            strafeDriver.shouldSprint = true;
            strafeDriver.shouldFireEquipment = false;
        }

        private void AW_SetupChaseDriver()
        {
            var chaseDriver = this.AW_master.AddComponent<AISkillDriver>();
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.requireEquipmentReady = false;
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.minUserHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxUserHealthFraction = Single.PositiveInfinity;
            chaseDriver.minTargetHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            chaseDriver.minDistance = 30f;
            chaseDriver.maxDistance = Single.PositiveInfinity;
            chaseDriver.selectionRequiresTargetLoS = false;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1f;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.ignoreNodeGraph = false;
            chaseDriver.driverUpdateTimerOverride = -1f;
            chaseDriver.resetCurrentEnemyOnNextDriverSelection = false;
            chaseDriver.noRepeat = false;
            chaseDriver.shouldSprint = true;
            chaseDriver.shouldFireEquipment = false;
        }

        private void AW_SetupUtilityDriver()
        {
            var utilityDriver = this.AW_master.AddComponent<AISkillDriver>();
            utilityDriver.customName = "Utility";
            utilityDriver.skillSlot = SkillSlot.Utility;
            utilityDriver.requiredSkill = null;
            utilityDriver.requireSkillReady = true;
            utilityDriver.requireEquipmentReady = false;
            utilityDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityDriver.minUserHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxUserHealthFraction = WispBossEnrageController.enrageStartHealthFrac;
            utilityDriver.minTargetHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            utilityDriver.minDistance = 0f;
            utilityDriver.maxDistance = 100f;
            utilityDriver.selectionRequiresTargetLoS = false;
            utilityDriver.activationRequiresTargetLoS = false;
            utilityDriver.movementType = AISkillDriver.MovementType.Stop;
            utilityDriver.moveInputScale = 1f;
            utilityDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilityDriver.ignoreNodeGraph = false;
            utilityDriver.driverUpdateTimerOverride = 0.5f;
            utilityDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilityDriver.noRepeat = false;
            utilityDriver.shouldSprint = false;
            utilityDriver.shouldFireEquipment = false;
        }

        private void AW_SetupSecondaryDriver()
        {
            var secondaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            secondaryDriver.customName = "Secondary";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requiredSkill = null;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.requireEquipmentReady = false;
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minDistance = 0f;
            secondaryDriver.maxDistance = 300f;
            secondaryDriver.selectionRequiresTargetLoS = false;
            secondaryDriver.activationRequiresTargetLoS = false;
            secondaryDriver.movementType = AISkillDriver.MovementType.Stop;
            secondaryDriver.moveInputScale = 1f;
            secondaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            secondaryDriver.ignoreNodeGraph = false;
            secondaryDriver.driverUpdateTimerOverride = 0.5f;
            secondaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            secondaryDriver.noRepeat = false;
            secondaryDriver.shouldSprint = false;
            secondaryDriver.shouldFireEquipment = false;
        }

        private void AW_SetupPrimaryDriver()
        {
            var primaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            primaryDriver.customName = "Primary";
            primaryDriver.skillSlot = SkillSlot.Primary;
            primaryDriver.requiredSkill = null;
            primaryDriver.requireSkillReady = true;
            primaryDriver.requireEquipmentReady = false;
            primaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            primaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryDriver.minDistance = 0f;
            primaryDriver.maxDistance = 100f;
            primaryDriver.selectionRequiresTargetLoS = true;
            primaryDriver.activationRequiresTargetLoS = false;
            primaryDriver.movementType = AISkillDriver.MovementType.Stop;
            primaryDriver.moveInputScale = 1f;
            primaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            primaryDriver.ignoreNodeGraph = false;
            primaryDriver.driverUpdateTimerOverride = 0.5f;
            primaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primaryDriver.noRepeat = false;
            primaryDriver.shouldSprint = false;
            primaryDriver.shouldFireEquipment = false;
        }
    }
}
#endif

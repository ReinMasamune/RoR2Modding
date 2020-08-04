namespace ReinGeneralFixes
{
    using RoR2;
    using RoR2.Skills;

    using UnityEngine;

    internal partial class Main
    {
        partial void BalanceCommandoCDs() => this.FirstFrame += this.EditCommandoSecondaryCooldown;

        private void EditCommandoSecondaryCooldown()
        {
            SkillFamily family = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<SkillLocator>().secondary.skillFamily;
            family.variants[0].skillDef.baseRechargeInterval = 2f;

        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/

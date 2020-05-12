namespace Sniper.States.Special
{
    using Sniper.SkillDefs;
    using Sniper.States.Bases;

    internal class KnifeActivation : ActivationBaseState<KnifeSkillData>
    {
        // TODO: Implement Knife Activation
        internal override KnifeSkillData CreateSkillData()
        {
            base.data = new KnifeSkillData();
            return base.data;
        }
    }
}

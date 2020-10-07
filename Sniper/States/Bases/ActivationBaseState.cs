namespace Rein.Sniper.States.Bases
{
    using Rein.Sniper.SkillDefTypes.Bases;

    internal abstract class ActivationBaseState<TSkillData> : SniperSkillBaseState where TSkillData : SkillData, new()
    {
        internal TSkillData data;
        internal abstract TSkillData CreateSkillData();
    }
}

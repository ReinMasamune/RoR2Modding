namespace Sniper.States.Bases
{
    using Sniper.SkillDefTypes.Bases;

    // CLEANUP: Switch this to an interface.
    internal abstract class ReactivationBaseState<TSkillData> : SniperSkillBaseState where TSkillData : SkillData, new()
    {
        internal TSkillData skillData { get; set; }
    }
}

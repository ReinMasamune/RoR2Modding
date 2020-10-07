namespace Rein.Sniper.States.Bases
{
    using Rein.Sniper.SkillDefTypes.Bases;

    // CLEANUP: Switch this to an interface.
    internal abstract class ReactivationBaseState<TSkillData> : SniperSkillBaseState where TSkillData : SkillData, new()
    {
        internal TSkillData skillData { get; set; }
    }
}

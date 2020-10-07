namespace Rein.Sniper.States.Bases
{
    using EntityStates;

    using Rein.Sniper.Components;

    internal abstract class SniperSkillBaseState : BaseSkillState
    {
        protected new SniperCharacterBody characterBody
        {
            get => base.characterBody as SniperCharacterBody;
        }
    }
}

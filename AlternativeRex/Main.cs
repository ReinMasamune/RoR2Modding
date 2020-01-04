namespace AlternativeRex
{
    using BepInEx;
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.AltRex", "Rein-AlternativeRex", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        private void OnDisable()
        {

        }

        private void OnEnable()
        {
            On.EntityStates.Treebot.Weapon.FirePlantSonicBoom.OnEnter += this.FirePlantSonicBoom_OnEnter;
        }

        private void FirePlantSonicBoom_OnEnter( On.EntityStates.Treebot.Weapon.FirePlantSonicBoom.orig_OnEnter orig, EntityStates.Treebot.Weapon.FirePlantSonicBoom self )
        {
            var otherSelf = (EntityStates.Treebot.Weapon.FireSonicBoom)self;
            otherSelf.airKnockbackDistance = 0f;
            otherSelf.groundKnockbackDistance = 0f;
            otherSelf.liftVelocity = 0f;
            otherSelf.idealDistanceToPlaceTargets = otherSelf.maxDistance * 0.5f;

            orig( self );
        }
    }
}

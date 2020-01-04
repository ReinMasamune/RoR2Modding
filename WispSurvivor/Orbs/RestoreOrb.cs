using RoR2;

namespace RogueWispPlugin.Orbs
{
    public class RestoreOrb : RoR2.Orbs.Orb
    {
        public System.UInt32 skin = 0;

        public System.UInt32 stacks = 0;
        public System.Single length = 0f;

        public System.Boolean crit = false;
        public System.Boolean hasTarget = false;

        public override void Begin()
        {
            this.duration = this.distanceToTarget / 75f;
            EffectData effect = new EffectData
            {
                origin = origin,
                genericFloat = duration,
                genericUInt = stacks,
                scale = 1f,
                genericBool = true
            };

            effect.SetHurtBoxReference( this.target );

            EffectManager.SpawnEffect( Main.utilityLeech[this.skin], effect, true );
        }

        public override void OnArrival()
        {
            if( !this.target ) return;
            HealthComponent hc = this.target.healthComponent;
            if( !hc ) return;
            CharacterBody body = this.target.healthComponent.body;
            if( !body ) return;

            BuffIndex b = BuffCatalog.FindBuffIndex("WispFlameChargeBuff");

            for( System.Int32 i = 0; i < this.stacks; i++ )
            {
                body.AddTimedBuff( b, this.length );
            }
        }


    }
}

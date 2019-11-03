using RoR2;

namespace WispSurvivor.Orbs
{
    public class RestoreOrb : RoR2.Orbs.Orb
    {
        public uint skin = 0;

        public uint stacks = 0;

        public bool crit = false;
        public bool hasTarget = false;

        public override void Begin()
        {
            duration = distanceToTarget / 50f;
            EffectData effect = new EffectData
            {
                origin = origin,
                genericFloat = duration,
                genericUInt = stacks
            };

            effect.SetHurtBoxReference(target);

            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.utilityLeech[skin] , effect , true);
        }

        public override void OnArrival()
        {
            if (!target) return;
            HealthComponent hc = target.healthComponent;
            if (!hc) return;
            CharacterBody body = target.healthComponent.body;
            if (!body) return;

            BuffIndex b = BuffCatalog.FindBuffIndex("WispFlameChargeBuff");
            for( int i = 0; i < stacks; i++ )
            {
                body.AddTimedBuff(b, 5f);
            }
        }


    }
}

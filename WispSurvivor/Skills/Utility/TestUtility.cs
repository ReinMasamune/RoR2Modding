
/*
namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class TestUtility : BaseState
        {
            public static System.Double chargeUsed = 10.0;

            public static System.Single baseDuration = 0.2f;
            public static System.Single maxRange = 50f;
            public static System.Single returnIdlePercent = 0.5f;
            public static System.Single chargeScaler = 1f;

            public static System.Single radius = 3f;
            public static System.Single baseTime = 5f;
            public static System.Single targetResetFreq = 0.5f;
            public static System.Single damageMult = 0.25f;
            public static System.Single baseDebuffTime = 1.5f;
            public static System.Single dotTimeMult = 1f;
            public static System.Single tickFreq = 1f;
            public static System.Single minTime = 1f;
            public static System.Single spreadLossFrac = 1.1f;

            private System.Double charge;

            private System.Single duration;

            private System.Boolean hasFired = false;
            private System.Boolean scanned = false;
            private System.Boolean synced = false;
            private System.Boolean useTarget = false;

            private System.UInt32 skin = 0;

            private Vector3 targetVec;
            private Vector3 normal;

            private ChildLocator childLoc;
            private Animator anim;
            private HurtBox target;
            private BullseyeSearch search = new BullseyeSearch();
            private WispPassiveController passive;

            public override void OnEnter()
            {
                base.OnEnter();

                this.passive = this.gameObject.GetComponent<WispPassiveController>();

                Transform modelTrans = base.GetModelTransform();
                //Sound

                this.duration = baseDuration;

                if( this.characterBody )
                {
                    this.characterBody.SetAimTimer( this.duration + 1f );
                }

                this.skin = this.characterBody.skinIndex;

                //charge = passive.UseCharge(chargeUsed, );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                //FireOrb();
                if( this.fixedAge > this.duration && this.isAuthority )
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            public override void OnExit() => base.OnExit();

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;

            public override void OnSerialize( NetworkWriter writer )
            {
                if( this.isAuthority )
                {
                    writer.Write( HurtBoxReference.FromHurtBox( this.target ) );
                    writer.Write( this.targetVec );
                    writer.Write( this.skin );
                }
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                if( !this.isAuthority )
                {
                    this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
                    this.targetVec = reader.ReadVector3();
                    this.skin = reader.ReadUInt32();
                    this.synced = true;
                }
            }

            private void GetTarget()
            {
                this.useTarget = false;
                Ray r = this.GetAimRay();

                RaycastHit rh;

                if( Physics.SphereCast( r, 0.25f, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.targetVec = rh.point;
                    this.normal = rh.normal;

                    Collider col = rh.collider;
                    if( col )
                    {
                        HurtBox hb = col.GetComponent<HurtBox>();
                        if( hb )
                        {
                            TeamIndex team = TeamComponent.GetObjectTeam(hb.healthComponent.gameObject);
                            if( team != TeamComponent.GetObjectTeam( this.gameObject ) )
                            {
                                this.target = hb;
                                this.useTarget = true;
                            }
                        }
                    }
                } else
                {
                    this.targetVec = r.GetPoint( maxRange );
                    this.normal = Vector3.up;
                }
            }

            
            private void FireOrb()
            {
                if (!NetworkServer.active) return;

                GetTarget();

                float time = baseTime + (chargeScaler * (float)(charge - 100) / 100);
                float debuffTime = baseDebuffTime + (chargeScaler * (float)(charge - 100) / 100);

                Orbs.BlazeOrb orb = new Orbs.BlazeOrb();

                orb.attacker = gameObject;
                orb.bRadius = radius;
                orb.bTargetResetFreq = targetResetFreq;
                orb.crit = RollCrit();
                orb.iDamageMult = damageMult;
                orb.iDebuffTime = debuffTime;
                orb.iDotMult = dotTimeMult;
                orb.iTickFreq = tickFreq;
                orb.minTime = minTime;
                orb.normal = normal;
                orb.origin = targetVec;
                orb.skin = skin;
                orb.spreadTimeDiv = spreadLossFrac;
                orb.team = TeamComponent.GetObjectTeam(gameObject);
                orb.time = time;

                OrbManager.instance.AddOrb(orb);
            }
            
        }
    }

}
*/
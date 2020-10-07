namespace Rein.Sniper.States.Special
{
    using System;
    using Object = System.Object;
    using Int32 = System.Int32;
    using Single = System.Single;
    using Rein.Sniper.Modules;
    using Rein.Sniper.SkillDefs;
    using Rein.Sniper.States.Bases;
    using UnityEngine;
    using RoR2;
    using RoR2.Projectile;
    using EntityStates;

    internal class KnifeActivation : ActivationBaseState<KnifeSkillData>
    {
        private const Single parentAtFrac = 0.39f;
        private const Single throwAtFrac = 0.58f;
        private const Single baseDuration = 0.35f;
        private const Single damageMultiplier = 1f;

        private static readonly GameObject projectilePrefab = ProjectileModule.GetKnifeProjectile();


        private Single duration;
        private Single parentTime;
        private Single throwTime;
        private Boolean isParented = false;
        private Boolean hasThrown = false;

        private Transform knifeBone;
        private Transform handBone;
        private Transform knifeDefault;
        private SkinnedMeshRenderer knifeRenderer;


        internal override KnifeSkillData CreateSkillData()
        {
            base.data = new KnifeSkillData();
            return base.data;
        }


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / base.attackSpeedStat;
            this.throwTime = this.duration * throwAtFrac;
            base.PlayAnimation( "Gesture, Additive", "ThrowKnife", "rateThrowKnife", this.duration );


            this.handBone = base.FindModelChild( "ThrowAnimationParent" );
            this.knifeDefault = base.FindModelChild( "ThrowKnifeDefaultPosition" );
            this.knifeBone = base.FindModelChild( "ThrowKnifeBone" );
            this.knifeRenderer = base.GetModelTransform().Find( "ThrowKnife" ).GetComponent<SkinnedMeshRenderer>();

            if( base.data != null )
            {
                base.data.knifeRenderer = this.knifeRenderer;
            }
            // FUTURE: Knife throw sound
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( !this.hasThrown )
            {
                if( base.fixedAge >= this.throwTime )
                {
                    this.Throw();
                }
            }

            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }

        }

        public override void Update()
        {
            base.Update();

            if( !this.isParented && base.age >= this.parentTime )
            {
                this.knifeBone.SetParent( this.handBone, false );
                this.isParented = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Throw()
        {
            this.knifeRenderer.enabled = false;
            this.knifeBone.SetParent( this.knifeDefault, false );

            if( base.isAuthority )
            {
                var aim = base.GetAimRay();

                var info = new FireProjectileInfo
                {
                    crit = base.RollCrit(),
                    damage = base.damageStat * damageMultiplier,
                    damageColorIndex = DamageColorIndex.Default,
                    force = 100f,
                    owner = base.gameObject,
                    position = aim.origin,
                    procChainMask = default,
                    projectilePrefab = projectilePrefab,
                    rotation = Util.QuaternionSafeLookRotation( aim.direction ),
                    target = null,
                    useFuseOverride = false,
                    useSpeedOverride = false,
                };

                ProjectileManager.instance.FireProjectile( info );
            }
            this.hasThrown = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}

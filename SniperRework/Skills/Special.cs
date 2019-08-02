using RoR2;
using UnityEngine;
using ReinSniperRework;
using RoR2.Projectile;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperSpecial : BaseState
    {
        ReinDataLibrary data;
        private float duration;

        private FireProjectileInfo projInfo;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            Util.PlaySound(data.r_fireSound, base.gameObject);
            duration = data.r_baseDuration / this.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            if (base.isAuthority)
            {
                //Projectile stuff
                projInfo.crit = false;
                projInfo.damage = 0f;
                //projInfo.damageColorIndex
                projInfo.force = 0f;
                projInfo.owner = base.gameObject;
                projInfo.position = aimRay.origin;
                //projInfo.procChainMask
                projInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                //projInfo.target
                //projInfo.useFuseOverride
                //projInfo.useSpeedOverride
                //projInfo.fuseOverride
                //projInfo.speedOverride
                projInfo.projectilePrefab = data.r_mineProj;

                ProjectileManager.instance.FireProjectile(projInfo);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}

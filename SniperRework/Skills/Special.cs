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
                HookMineInvader inv = data.r_mineProj.GetComponent<HookMineInvader>();
                inv.owner = base.gameObject;

                inv.enabled = true;

                //Projectile stuff
                projInfo.crit = false;
                projInfo.damage = 0f;
                projInfo.force = 0f;
                projInfo.owner = base.gameObject;
                projInfo.position = aimRay.origin;
                projInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                projInfo.projectilePrefab = data.r_mineProj;

                ProjectileManager.instance.FireProjectile(projInfo);

                inv.enabled = false;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}

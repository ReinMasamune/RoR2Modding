namespace Rein.Sniper.Ammo
{
    using System;

    using RoR2;

    using Rein.Sniper.Components;
    using Rein.Sniper.Expansions;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;
    using Rein.Sniper.Modules;

    internal class FMJContext : OnHitContextBase<RicochetData>
        {
            private static readonly GameObject _tracer;
            static FMJContext()
            {
                _tracer = VFXModule.GetStandardAmmoTracer();
                RicochetController.ricochetEffectPrefab = VFXModule.GetRicochetEffectPrefab();
            }
            public override GameObject tracerEffectPrefab => _tracer;
            protected override LayerMask stopperMask => base.stopperMask & ~LayerIndex.entityPrecise.mask;
            protected override OnBulletDelegate<RicochetData> onStop => (bullet, hit) =>
            {
                if(hit.collider)
                {
                    Vector3 v1 = hit.direction;
                    Vector3 v2 = hit.surfaceNormal;
                    Single dot = (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
                    Single chance = Mathf.Clamp( Mathf.Acos( dot ) / 0.02f / Mathf.PI, 0f, 100f);
                    if(Util.CheckRoll(100f - chance, bullet.attackerBody.master))
                    {
                        Vector3 newDir = (-2f * dot * v2) + v1;
                        var newBul = bullet.Clone();// as ExpandableBulletAttack<RicochetData>;
                        newBul.origin = hit.point;
                        newBul.aimVector = newDir;
                        var wepObj = newBul.weapon = new GameObject("temp", typeof(NetworkIdentity));
                        
                        if(newBul.data.counter++ == 0) newBul.radius *= 10f;

                        if(hit.damageModifier == HurtBox.DamageModifier.SniperTarget)
                        {
                            newBul.damage *= 1.5f;
                        }

                        RicochetController.QueueRicochet(newBul, (UInt32)(hit.distance / 6f) + 1u);
                    }
                }
            };

            public override void InitBullet<T>(ExpandableBulletAttack<RicochetData> bullet, SnipeState<T> state)
            {
                base.InitBullet(bullet, state);
                bullet.damage *= state.reloadBoost;
                bullet.damage *= (1f + state.chargeBoost);
                if(bullet.isCrit)
                {
                    bullet.damage *= 1f + (state.chargeBoost / 20f);
                }
            }

        }

}

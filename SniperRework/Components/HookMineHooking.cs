using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using RoR2.Orbs;
using RoR2.Projectile;

namespace ReinSniperRework
{
    [RequireComponent(typeof(ProjectileStickOnImpact))]
    public class HookMineHooking : MonoBehaviour
    {
        public float hookInterval;
        public float hookDuration;
        public float hookRadius;
        public int hooksPerTick;
        public int hookTicks;
        public GameObject owner;
        public TeamIndex teamFriendly;
        public TeamIndex teamHostile;

        private float timer = 0f;
        private float hookTimer = 0f;

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            hookTimer += Time.fixedDeltaTime;
            if( hookTimer >= hookInterval )
            {
                hookTimer -= hookInterval;
                Hook();
                hookTicks--;
                if (hookTicks < 0)
                {
                    Destroy(gameObject);
                }
            }


        }

        private void Hook()
        {
            SphereSearch search = new SphereSearch();
            search.origin = gameObject.transform.position;
            search.radius = hookRadius;
            search.mask = LayerIndex.entityPrecise.mask;
            HurtBox[] hurtB = search.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.AllExcept(teamFriendly)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes();
            List<CharacterBody> targets = new List<CharacterBody>();

            foreach (HurtBox HB in hurtB)
            {
                if (HB)
                {
                    HealthComponent HC = HB.healthComponent;
                    if (HC)
                   {
                        CharacterBody target = HB.healthComponent.body;
                        if (target)
                        {
                            if (!(targets.Contains(target)))
                            {
                                targets.Add(target);
                            }
                        }
                    }
                }
            }
            targets.Reverse();

            if( targets.Count > 0 )
            {
                for (int i = 0; i < Math.Min(hooksPerTick,targets.Count); i++)
                {
                    CharacterBody target = targets[i];
                    if (target)
                    {
                        HurtBox HB = target.mainHurtBox;
                        if( HB )
                        {
                            BounceOrb yonk = new BounceOrb();
                            yonk.origin = gameObject.transform.position;
                            yonk.damageValue = 0f;
                            yonk.isCrit = false;
                            yonk.teamIndex = teamFriendly;
                            yonk.attacker = owner;
                            yonk.procCoefficient = 0f;
                            yonk.target = HB;
                            OrbManager.instance.AddOrb(yonk);
                        }
                    }
                }
            }
        }
    }
}
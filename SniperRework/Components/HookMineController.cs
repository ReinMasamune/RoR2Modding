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
    public class HookMineController : MonoBehaviour
    {
        public float primingDelay;
        public float activationDelay;
        public float hookInterval;
        public float hookDuration;
        public float hookRadius;
        public int hooksPerTick;
        public string primeSound;
        public string activationSound;
        public TeamIndex teamFriendly;
        public TeamIndex teamHostile;
        public GameObject wardPrefab;

        private float timer = 0f;
        private float hookTimer = 0f;
        private ProjectileStickOnImpact stick;
        private ProjectileController control;
        private HookMineController.MineState mineState = HookMineController.MineState.Flying;
        private bool tripped = false;

        public enum MineState
        {
            Flying,
            Priming,
            Sticking,
            Activating,
            Hooking,
            Dying
        }

        public void Start()
        {
            stick = gameObject.GetComponent<ProjectileStickOnImpact>();
            control = gameObject.GetComponent<ProjectileController>();
        }

        public void FixedUpdate()
        {
            if( !control.owner )
            {
                mineState = MineState.Dying;
            }
            switch( mineState )
            {
                case HookMineController.MineState.Flying:
                    if( stick.stuck )
                    {
                        mineState = HookMineController.MineState.Priming;
                        timer = 0f;
                    }
                    break;

                case HookMineController.MineState.Priming:
                    if( timer > primingDelay )
                    {
                        mineState = HookMineController.MineState.Sticking;
                        timer = 0f;
                    }
                    else
                    {
                        timer += Time.fixedDeltaTime;
                    }

                    break;

                case HookMineController.MineState.Sticking:
                    if( tripped )
                    {
                        mineState = HookMineController.MineState.Activating;
                        timer = 0f;
                    }
                    break;

                case HookMineController.MineState.Activating:
                    if (timer < activationDelay)
                    {
                        timer += Time.fixedDeltaTime;
                    }
                    else
                    {
                        OnActivation();
                        mineState = HookMineController.MineState.Hooking;
                        timer = 0f;
                    }
                    break;

                case HookMineController.MineState.Hooking:
                    hookTimer += Time.fixedDeltaTime;
                    timer += Time.fixedDeltaTime;
                    if( hookTimer > hookInterval )
                    {
                        hookTimer -= hookInterval;
                        Hook();
                    }
                    if( timer > hookDuration )
                    {
                        mineState = HookMineController.MineState.Dying;
                        timer = 0f;
                    }
                    break;

                case HookMineController.MineState.Dying:
                    Destroy(gameObject);
                    break;

                default:
                    Debug.Log("Yep, you broke the minestate.");
                    break;
            }
        }

        public void OnTriggerStay( Collider col )
        {
            if( NetworkServer.active )
            {
                if (mineState == HookMineController.MineState.Sticking)
                {
                    if (col)
                    {
                        HurtBox HB = col.GetComponent<HurtBox>();
                        if (HB)
                        {
                            HealthComponent HC = HB.healthComponent;
                            if (HC)
                            {
                                TeamComponent TC = HC.GetComponent<TeamComponent>();
                                if (TC)
                                {
                                    if ( TC.teamIndex == teamHostile )
                                    {
                                        tripped = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void OnActivation()
        {
            if( NetworkServer.active)
            {
                if( wardPrefab )
                {
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(wardPrefab, base.transform.position, Quaternion.identity);
                    BuffWard ward = obj.GetComponent<BuffWard>();
                    TeamFilter filter = obj.GetComponent<TeamFilter>();
                    filter.teamIndex = (filter.teamIndex == TeamIndex.Player) ? TeamIndex.Monster : TeamIndex.Player;
                    NetworkServer.Spawn(obj);
                }
                this.stick.enabled = false;
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
                            yonk.attacker = control.owner;
                            yonk.procCoefficient = 0f;
                            yonk.target = HB;
                            OrbManager.instance.AddOrb(yonk);
                        }
                    }
                }
            }
            
        }

        public struct CheckTarget : Unity.Jobs.IJob
        {


            public void Execute()
            {

            }
        }
    }
}
using BepInEx;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;
using System.Collections.Generic;
using RoR2.Orbs;
using RoR2.Projectile;

namespace ReinSniperRework
{
    [RequireComponent(typeof(ProjectileStickOnImpact))]
    public class HookMineController : MonoBehaviour
    {

        private GameObject activationEffectPrefab;
        public GameObject wardPrefab;
        private GameObject prepForActivationChildEffect;

        public float primingDelay = 0.3f;
        public float prepForActivationDuration = 1f;
        public string primingSoundString = "";
        public string activationSoundString = "";

        public float maxActiveTime = 5f;

        private float fixedAge;


        private ProjectileStickOnImpact stick;
        private ProjectileController control;
        private MineProximityDetonator proxDet;

        private HookMineController.MineState mineState;

        public enum MineState
        {
            Flying,
            Priming,
            Sticking,
            PrepForActivation,
            Active,
            Dead
        }

        private void Start()
        {
            stick = base.GetComponent<ProjectileStickOnImpact>();
            control = base.GetComponent<ProjectileController>();
            //Force to spin it, later

            if (NetworkServer.active && this.control.owner)
            {
                //DeployableSlot stuff, needed?
            }
        }

        public void Activate()
        {
            if (NetworkServer.active && this.mineState != HookMineController.MineState.Active)
            {
                this.mineState = HookMineController.MineState.Active;

                //Explosion would be here

                if (this.activationSoundString != "")
                {
                    Util.PlaySound(this.activationSoundString, base.gameObject);
                }

                if (this.activationEffectPrefab)
                {
                    //effect stuff later
                }

                if (this.wardPrefab)
                {
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.wardPrefab, base.transform.position, Quaternion.identity);
                    BuffWard ward = obj.GetComponent<BuffWard>();
                    //Radius adjustment?
                    TeamFilter team = obj.GetComponent<TeamFilter>();
                    team.teamIndex = ((team.teamIndex == TeamIndex.Player) ? TeamIndex.Monster : TeamIndex.Player);
                    NetworkServer.Spawn(obj);
                }
                //Destroy was called here, doing it somewhere else
            }
        }

        private void FixedUpdate()
        {
            switch (this.mineState)
            {
                case HookMineController.MineState.Flying:
                    if (NetworkServer.active)
                    {
                        this.proxDet.gameObject.SetActive(false);
                    }

                    if (this.stick.stuck)
                    {
                        this.mineState = HookMineController.MineState.Priming;
                        return;
                    }

                    break;
                case HookMineController.MineState.Priming:
                    if (NetworkServer.active)
                    {
                        this.proxDet.gameObject.SetActive(false);
                    }

                    this.fixedAge += Time.fixedDeltaTime;

                    if (!this.stick.stuck)
                    {
                        this.mineState = HookMineController.MineState.Flying;
                        this.fixedAge = 0f;
                    }

                    if (this.fixedAge >= this.primingDelay)
                    {
                        this.mineState = HookMineController.MineState.Sticking;
                        this.fixedAge = 0f;

                        if (this.primingSoundString != "")
                        {
                            Util.PlaySound(this.primingSoundString, base.gameObject);
                            return;
                        }
                    }

                    break;
                case HookMineController.MineState.Sticking:
                    if (NetworkServer.active)
                    {
                        this.proxDet.gameObject.SetActive(true);
                    }

                    this.fixedAge += Time.fixedDeltaTime;

                    if (!this.stick.stuck)
                    {
                        this.mineState = HookMineController.MineState.Flying;
                        this.fixedAge = 0f;
                        return;
                    }

                    break;
                case HookMineController.MineState.PrepForActivation:
                    if (NetworkServer.active)
                    {
                        this.proxDet.gameObject.SetActive(false);
                        this.fixedAge += Time.fixedDeltaTime;

                        if (this.fixedAge >= this.prepForActivationDuration)
                        {
                            this.Activate();
                            this.mineState = HookMineController.MineState.Active;
                            this.fixedAge = 0f;
                        }
                    }

                    break;

                case HookMineController.MineState.Active:
                    if (NetworkServer.active)
                    {
                        this.fixedAge += Time.fixedDeltaTime;
                        if (this.fixedAge >= this.maxActiveTime)
                        {
                            //GetRidOfMe
                        }
                    }

                    break;

                default:
                    return;
            }
        }

        public void PrepareForActivation()
        {
            if (this.mineState < HookMineController.MineState.PrepForActivation)
            {
                this.mineState = HookMineController.MineState.PrepForActivation;
                this.fixedAge = 0f;
                this.prepForActivationChildEffect.SetActive(true);
                this.stick.enabled = false;
                Rigidbody rb = base.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //Detatch force would be here
            }
        }
    }
}
using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class FireHeatwave : BaseState
    {
        public float fireDuration;
        public float fireDelay;
        public float damageValue;
        public float explosionRadius;
        public float maxRange;
        public float maxAngle;

        public bool crit;

        public uint skin;

        private bool fired = false;

        private Vector3 targetVec;
        private HurtBox target;
        private BullseyeSearch search = new BullseyeSearch();
        private ChildLocator childLoc;

        public override void OnEnter()
        {
            base.OnEnter();
            if( isAuthority )
            {
                skin = characterBody.skinIndex;
                GetTarget();
            }
            childLoc = GetModelTransform().GetComponent<ChildLocator>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( !fired )
            {
                characterBody.isSprinting = false;
            }
            if( !fired && fixedAge >= fireDelay )
            {
                FireOrb();
            }
            if( fixedAge >= fireDuration )
            {
                if( isAuthority )
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(HurtBoxReference.FromHurtBox(target));
            writer.Write(targetVec);
            writer.Write(skin);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
            targetVec = reader.ReadVector3();
            skin = reader.ReadUInt32();
        }

        private void GetTarget()
        {
            Ray r = GetAimRay();

            search.teamMaskFilter = TeamMask.all;
            search.teamMaskFilter.RemoveTeam(TeamComponent.GetObjectTeam(gameObject));
            search.filterByLoS = true;
            search.searchOrigin = r.origin;
            search.searchDirection = r.direction;
            search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            search.maxDistanceFilter = maxRange;
            search.maxAngleFilter = maxAngle;
            search.RefreshCandidates();
            target = search.GetResults().FirstOrDefault<HurtBox>();

            RaycastHit rh;
            
            if( Physics.Raycast( r , out rh , maxRange , LayerIndex.world.mask | LayerIndex.entityPrecise.mask , QueryTriggerInteraction.UseGlobal ) )
            {
                targetVec = rh.point;
            }
            else
            {
                targetVec = r.GetPoint(maxRange);
            }
        }

        private void FireOrb()
        {
            if (fired) return;
            fired = true;
            if (!NetworkServer.active) return;

            Orbs.SnapOrb snap = new Orbs.SnapOrb();
            snap.damage = damageValue;
            snap.crit = RollCrit();
            snap.team = TeamComponent.GetObjectTeam(gameObject);
            snap.attacker = gameObject;
            snap.procCoef = 1.0f;
            snap.radius = explosionRadius;
            snap.skin = skin;
            Transform trans = childLoc.FindChild("MuzzleRight");
            snap.origin = trans.position;
            snap.speed = 150f;
            if( target )
            {
                snap.target = target;
                snap.useTarget = true;
                targetVec = target.transform.position;
            }
            else
            {
                snap.useTarget = false;
            }
            snap.targetPos = targetVec;
            OrbManager.instance.AddOrb(snap);
        }
    }
}

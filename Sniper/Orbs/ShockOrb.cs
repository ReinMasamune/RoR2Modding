namespace Rein.Sniper.Orbs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;
    using UnityEngine.Networking;

    internal sealed class ShockOrb : Orb, ISerializableObject
    {
        const Single baseMaxRange = 20f;

        internal static EffectIndex orbEffect = EffectIndex.Invalid;

        internal static BullseyeSearch searchPool
        {
            get => Pool<BullseyeSearch, InitShockBullseye, CleanShockBullseye>.item;
            set => Pool<BullseyeSearch, InitShockBullseye, CleanShockBullseye>.item = value;
        }


        internal Single speed;
        internal Single damage;
        internal Int32 threshold;
        internal Int32 stackCount;
        internal Boolean crit;
        internal Single procCoef;
        internal Vector3 backupTargetPos;
        internal TeamIndex attackerTeam => this.attacker.teamComponent.teamIndex;
        internal CharacterBody attacker;

        internal HashSet<HealthComponent> hitEnemies;



        private Vector3 targetPos
        {
            get
            {
                var target = this.target;
                if(!this.target) return this.backupTargetPos;
                var tran = target.transform;
                if(!tran) return this.backupTargetPos;
                return this.backupTargetPos = tran.position;
            }
        }
        

        public void Deserialize(NetworkReader reader)
        {
            this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
            this.origin = reader.ReadVector3();
            this.speed = reader.ReadSingle();
            this.damage = reader.ReadSingle();
            this.threshold = reader.ReadInt32();
            this.stackCount = reader.ReadInt32();
            this.crit = reader.ReadBoolean();
            this.procCoef = reader.ReadSingle();
            this.backupTargetPos = reader.ReadVector3();
            var netId = reader.ReadNetworkIdentity();
            if(netId)
            {
                this.attacker = netId.GetComponent<CharacterBody>();
            }
            this.hitEnemies = new();
            var i = reader.ReadInt32();
            while(i-->0)
            {
                var nid = reader.ReadNetworkIdentity();
                if(nid && nid.GetComponent<HealthComponent>() is HealthComponent hc && hc)
                {
                    this.hitEnemies.Add(hc);
                }
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.target));
            writer.Write(this.origin);
            writer.Write(this.speed);
            writer.Write(this.damage);
            writer.Write(this.threshold);
            writer.Write(this.stackCount);
            writer.Write(this.crit);
            writer.Write(this.procCoef);
            writer.Write(this.backupTargetPos);
            writer.Write(this.attacker.networkIdentity);
            writer.Write(this.hitEnemies.Count);
            foreach(var v in this.hitEnemies)
            {
                writer.Write(v.body);
            }
        }

        public override void Begin()
        {
            base.duration = 0.1f + (base.origin - this.targetPos).magnitude / this.speed;

            if(orbEffect == EffectIndex.Invalid)
            {
                orbEffect = EffectCatalog.FindEffectIndexFromPrefab(VFXModule.GetShockOrbPrefab());
            }

            if(this.target)
            {
                EffectData data = new()
                {
                    origin = this.origin,
                    genericFloat = base.duration,
                };
                if(this.target) data.SetHurtBoxReference(this.target);
                EffectManager.SpawnEffect(orbEffect, data, true);
            }

            


            base.Begin();

            this.hitEnemies.Add(this.target.healthComponent);
        }

        public override void OnArrival()
        {
            base.OnArrival();

            var h = this.target;
            if(!h) h = null;
            var body = h?.healthComponent?.body;
            if(!body) body = null;

            var damage = this.damage;

            if(body)
            {
                this.stackCount = body.GetBuffCount(CatalogModule.shockAmmoDebuff);
            }
            if(this.stackCount > 0)
            {
                var threshold = this.stackCount;
                var newCount = this.stackCount;
                if(this.stackCount >= this.threshold)
                {
                    newCount -= this.threshold;
                    threshold = this.threshold;
                } else
                {
                    newCount = this.threshold - this.stackCount;
                }

                body?.ApplyBuff(CatalogModule.shockAmmoDebuff, newCount - this.stackCount);

                damage *= 6f;

                int hitCounter = this.threshold;
                foreach(var hb in this.FindValidTargets())
                {
                    if(hitCounter --> 0)
                    {
                        SpawnNextOrb(hb);
                    } else break;
                }
            } else
            {
                body?.ApplyBuff(CatalogModule.shockAmmoDebuff, this.threshold);
            }

            DamageInfo dInfo = new()
            {
                attacker = this.attacker.gameObject,
                crit = this.crit,
                damage = damage,
                damageColorIndex = DamageColorIndex.Default, //Add custom color for fun
                damageType = DamageType.Generic,
                dotIndex = DotController.DotIndex.None,
                force = Vector3.zero,
                inflictor = null,
                position = this.targetPos,
                procChainMask = default,
                procCoefficient = this.procCoef,
                rejected = false,
            };
            NetworkingHelpers.DealDamage(dInfo, this.target, true, true, true);
        }

        internal void SpawnNextOrb(HurtBox target) => new ShockOrb()
        {
            speed = this.speed,
            attacker = this.attacker,
            crit = this.crit,
            damage = this.damage,
            hitEnemies = this.hitEnemies,
            origin = this.targetPos,
            procCoef = this.procCoef,
            threshold = this.threshold,
            target = target,
            stackCount = target?.healthComponent?.body?.GetBuffCount(CatalogModule.shockAmmoDebuff) ?? 0,
            backupTargetPos = target.transform.position,
        }.Create();


        internal IEnumerable<HurtBox> FindValidTargets()
        {
            static (HurtBox hb, Int32 stackCount) GetStacks(HurtBox hb) => (hb, hb.healthComponent.body.GetBuffCount(CatalogModule.shockAmmoDebuff));
            static Boolean HasStacks((HurtBox hb, Int32 stackCount) input) => input.stackCount > 0;
            static Int32 GetStackCount((HurtBox hb, Int32 stackCount) input) => input.stackCount;
            static Boolean NoStacks((HurtBox hb, Int32 stackCount) input) => input.stackCount <= 0;
            static HurtBox GetHurtBox((HurtBox hb, Int32 stackCount) input) => input.hb;

            var search = searchPool;
            search.maxDistanceFilter *= (1f + Mathf.Sqrt(this.threshold));
            var pos = search.searchOrigin = this.targetPos;

            var sqdist = 0.75f * search.maxDistanceFilter * search.maxDistanceFilter;
            search.RefreshCandidates();

            var process = search.GetResults()
                .Where(h => FriendlyFireManager.ShouldSeekingProceed(h.healthComponent, this.attackerTeam))
                .Where(h => !this.hitEnemies.Contains(h.healthComponent))
                .Select(GetStacks);

            var without = process
                .Where(NoStacks)
                .Where(x => (x.hb.transform.position - pos).sqrMagnitude < sqdist);

            var with = process
                .Where(HasStacks)
                .OrderByDescending(GetStackCount);

            return with.Concat(without).Select(GetHurtBox).ToArray();
        }


        private struct InitShockBullseye : IInitItem<BullseyeSearch>
        {
            public BullseyeSearch InitItem() => new BullseyeSearch()
            {
                searchDirection = Vector3.zero,
                teamMaskFilter = TeamMask.all, //Use custom masking here
                filterByLoS = false,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = baseMaxRange,
                filterByDistinctEntity = true,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                minDistanceFilter = 0f,
            };
        }

        private struct CleanShockBullseye : ICleanItem<BullseyeSearch>
        {
            public void CleanItem(BullseyeSearch item)
            {
                item.maxDistanceFilter = baseMaxRange;
                item.searchOrigin = Vector3.zero;
                item.viewer = null;
            }
        }
    }
    /*Behaviour:
     * First bounce is always the enemy shot, and that enemy will always have stacks
     * When bouncing to an enemy with stacks, play trigger effect and fire more orbs
     * Prioritize bouncing to enemies with stacks.
     * Never able to trigger on the same enemy more than once
     * Threshold at each point determines how many arcs to fire on trigger
     * Threshold also increases range
     */
}

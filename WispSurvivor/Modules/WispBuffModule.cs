using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WispSurvivor.Modules
{
    public static class WispBuffModule
    {
        public static int debuffIndex;
        public static int buffIndex;

        public static void DoModule(GameObject body, Dictionary<Type, Component> dic)
        {
            SetupDeathHook(body, dic);
        }

        public static void RegisterBuffs()
        {
            BuffDef wispFireDebuff = new BuffDef
            {
                buffColor = new Color(0.5f, 0.1f, 0.7f, 1f),
                buffIndex = BuffIndex.Count,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffOnFireIcon",
                isDebuff = true,
                name = "WispCurseBurn"
            };
            BuffDef wispRestoreBuff = new BuffDef
            {
                buffColor = new Color(0.5f, 0.1f, 0.7f, 1f),
                buffIndex = BuffIndex.Count,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffTeslaIcon",
                isDebuff = false,
                name = "WispFlameChargeBuff"
            };

            //AddNewBuff
            AddNewBuff(wispFireDebuff);
            AddNewBuff(wispRestoreBuff);
        }

        private static void SetupDeathHook(GameObject body, Dictionary<Type, Component> dic)
        {
            GlobalEventManager.onCharacterDeathGlobal += report =>
            {
                if( report.victimBody && report.attackerBody )
                {
                    BuffIndex b = BuffCatalog.FindBuffIndex("WispCurseBurn");
                    uint stax = (uint)report.victimBody.GetBuffCount(b);
                    if( stax > 0 )
                    {
                        if( report.attackerBody.subtitleNameToken == "ReinThings.WispSurvivor" )
                        {
                            Orbs.RestoreOrb orb = new Orbs.RestoreOrb();
                            orb.skin = report.attackerBody.skinIndex;
                            orb.origin = report.victimBody.coreTransform.position;
                            orb.target = report.attackerBody.mainHurtBox;
                            orb.stacks = stax;

                            RoR2.Orbs.OrbManager.instance.AddOrb(orb);

                            //report.attackerBody.GetComponent<Components.WispPassiveController>().AddCharge(stax);
                        }
                    }
                }
            };
        }

        private static void ExFunction(GameObject body, Dictionary<Type, Component> dic)
        {

        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }

        private static void AddNewBuff(BuffDef b)
        {
            BuffDef[] buffs = typeof(BuffCatalog).GetFieldValue<BuffDef[]>("buffDefs");
            Dictionary<string, BuffIndex> name2Buff = typeof(BuffCatalog).GetFieldValue<Dictionary<string, BuffIndex>>("nameToBuffIndex");

            int ogNum = BuffCatalog.buffCount;

            typeof(BuffCatalog).SetPropertyValue<int>("buffCount",ogNum + 1);
            Array.Resize<BuffDef>(ref buffs, ogNum + 1);

            buffs[ogNum] = b;
            name2Buff[b.name] = (BuffIndex)ogNum;

            typeof(BuffCatalog).SetFieldValue<BuffDef[]>("buffDefs", buffs);
            typeof(BuffCatalog).SetFieldValue<Dictionary<string, BuffIndex>>("nameToBuffIndex", name2Buff);
        }
    }
}

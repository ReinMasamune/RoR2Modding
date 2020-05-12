#if ROGUEWISP
using ReinCore;

using RoR2;

using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal static BuffIndex RW_curseBurn;
        internal static BuffIndex RW_flameChargeBuff;
        internal static BuffIndex RW_armorBuff;

        partial void RW_Buff()
        {
            this.Load += this.RW_RegisterBuffs;
            this.FirstFrame += this.RW_EditEnrageBuff;
            this.FirstFrame += this.GetBuffInds;
        }

        private void GetBuffInds()
        {
            RW_curseBurn = BuffCatalog.FindBuffIndex( "WispCurseBurn" );
            RW_flameChargeBuff = BuffCatalog.FindBuffIndex( "WispFlameChargeBuff" );
            RW_armorBuff = BuffCatalog.FindBuffIndex( "WispArmorBuff" );
        }

        private void RW_EditEnrageBuff()
        {
            BuffDef enrage = BuffCatalog.GetBuffDef(BuffIndex.EnrageAncientWisp);
            enrage.buffColor = new Color( 0.5f, 0.1f, 0.7f, 1f );
            enrage.iconPath = "Textures/BuffIcons/texMovespeedBuffIcon";
        }

        private void RW_RegisterBuffs()
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
                iconPath = "Textures/BuffIcons/texBuffEngiShieldIcon",
                isDebuff = false,
                name = "WispFlameChargeBuff"
            };
            BuffDef wispArmorBuff = new BuffDef
            {
                buffColor = new Color(0.5f, 0.1f, 0.7f, 1f),
                buffIndex = BuffIndex.Count,
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                isDebuff = false,
                name = "WispArmorBuff"
            };

            BuffsCore.getAdditionalEntries += ( list ) =>
            {
                list.Add( wispFireDebuff );
                list.Add( wispRestoreBuff );
                list.Add( wispArmorBuff );
            };

            //AddNewBuff

            //this.RW_curseBurn = (BuffIndex)R2API.ItemAPI.AddCustomBuff( new R2API.CustomBuff( wispFireDebuff.name, wispFireDebuff ) );
            //this.RW_flameChargeBuff = (BuffIndex)R2API.ItemAPI.AddCustomBuff( new R2API.CustomBuff( wispRestoreBuff.name, wispRestoreBuff ) );
            //this.RW_armorBuff = (BuffIndex)R2API.ItemAPI.AddCustomBuff( new R2API.CustomBuff( wispArmorBuff.name, wispArmorBuff ) );
        }
        // TODO: Buff


        //private static BuffIndex AddNewBuff( BuffDef b )
        //{
        // 
        //    BuffDef[] buffs = typeof(BuffCatalog).GetFieldValue<BuffDef[]>("buffDefs");
        //  
        //    Dictionary<String, BuffIndex> name2Buff = typeof(BuffCatalog).GetFieldValue<Dictionary<String, BuffIndex>>("nameToBuffIndex");

        //    Int32 ogNum = BuffCatalog.buffCount;

        // 
        //    typeof( BuffCatalog ).SetPropertyValue<Int32>( "buffCount", ogNum + 1 );
        //    Array.Resize<BuffDef>( ref buffs, ogNum + 1 );

        //    buffs[ogNum] = b;
        //    name2Buff[b.name] = (BuffIndex)ogNum;

        //  
        //    typeof( BuffCatalog ).SetFieldValue<BuffDef[]>( "buffDefs", buffs );

        //  
        //    typeof( BuffCatalog ).SetFieldValue<Dictionary<String, BuffIndex>>( "nameToBuffIndex", name2Buff );
        //    return name2Buff[b.name];
        //}
    }

}
#endif
/*
namespace RogueWispPlugin.Modules
{
    public static class WispBuffModule
    {
        public static Int32 debuffIndex;
        public static Int32 buffIndex;

        public static Single utilityMSBoost = 0.25f;
        public static Single specialArmorBoost = 150f;

        public static void DoModule( GameObject body, Dictionary<Type, Component> dic )
        {


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

            //AddNewBuff
            AddNewBuff( wispFireDebuff );
            AddNewBuff( wispRestoreBuff );
            AddNewBuff( wispArmorBuff );

            BuffDef enrage = BuffCatalog.GetBuffDef(BuffIndex.EnrageAncientWisp);
            enrage.buffColor = new Color( 0.5f, 0.1f, 0.7f, 1f );
            enrage.iconPath = "Textures/BuffIcons/texMovespeedBuffIcon";

            BuffIndex armorBuff = BuffCatalog.FindBuffIndex("WispArmorBuff");


        }


        private static void ExFunction( GameObject body, Dictionary<Type, Component> dic )
        {

        }

        private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;

        private static void AddNewBuff( BuffDef b )
        {
            BuffDef[] buffs = typeof(BuffCatalog).GetFieldValue<BuffDef[]>("buffDefs");
            Dictionary<String, BuffIndex> name2Buff = typeof(BuffCatalog).GetFieldValue<Dictionary<String, BuffIndex>>("nameToBuffIndex");

            Int32 ogNum = BuffCatalog.buffCount;

            typeof( BuffCatalog ).SetPropertyValue<Int32>( "buffCount", ogNum + 1 );
            Array.Resize<BuffDef>( ref buffs, ogNum + 1 );

            buffs[ogNum] = b;
            name2Buff[b.name] = (BuffIndex)ogNum;

            typeof( BuffCatalog ).SetFieldValue<BuffDef[]>( "buffDefs", buffs );
            typeof( BuffCatalog ).SetFieldValue<Dictionary<String, BuffIndex>>( "nameToBuffIndex", name2Buff );
        }
    }
}
*/
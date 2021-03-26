//namespace ReinCore
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;

//    using MonoMod.Cil;

//    using RoR2;

//    using UnityEngine;

//    public static class EquipmentCore
//    {
//        public static Boolean loaded { get; internal set; } = false;

//        public static event EquipAddDelegate getAdditionalEntries;
//        public delegate void EquipAddDelegate( List<EquipmentDef> equipList );

//        static EquipmentCore()
//        {
//            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
//            loaded = true;
//        }

//        private static readonly MethodInfo collectAndRegister = typeof(CatalogModHelper<EquipmentDef>).GetMethod("CollectAndRegisterAdditionalEntries", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance) ?? throw new MissingMethodException("Couldn't find method CollectAndRegisterAdditionalEntries for EquipmentCatalog");
//        private static readonly FieldInfo equipmentDefs = typeof(EquipmentCatalog).GetField(nameof(EquipmentCatalog.equipmentDefs), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new MissingFieldException("No equipmentdefs in equipmentcatalog");

//        private delegate void RefArrayDel<T>(ref T[] array);

//        private static void EmittedDelegate1(ref EquipmentDef[] array)
//        {
//            var list = new List<EquipmentDef>();
//            getAdditionalEntries?.Invoke(list);

//            if(list.Count > 0)
//            {
//                var start = array.Length;
//                Array.Resize<EquipmentDef>(ref array, start + list.Count);
//                list.CopyTo(array, start);
//            }
//        }
//        private static void Init_Il(ILContext il) => new ILCursor(il)
//            .GotoNext(MoveType.After, x => x.MatchCallOrCallvirt(collectAndRegister))
//            .LdSFldA_(equipmentDefs)
//            .CallDel_<RefArrayDel<EquipmentDef>>(EmittedDelegate1);
//    }

//    public abstract class CustomEquipmentDef : EquipmentDef
//    {
//        //PickupModelPath
//        //PickupIconPath

//        public abstract new String name { get; }
//        public abstract new Single cooldown { get; }
//        public abstract new String nameToken { get; }
//        public abstract new String descriptionToken { get; }
//        public abstract new String loreToken { get; }
//        public abstract new String addressToken { get; }
//        public abstract new String unlockableName { get; }
//        public abstract new ColorCatalog.ColorIndex colorIndex { get; }
//        public abstract new Boolean canDrop { get; }
//        public abstract new Boolean enigmaCompatible { get; }
//        public abstract new Boolean isLunar { get; }
//        public abstract new Boolean isBoss { get; }
//        public abstract new BuffIndex passiveBuff { get; }
//        public abstract new Boolean appearsInSinglePlayer { get; }
//        public abstract new Boolean appearsInMultiPlayer { get; }


//        private readonly GameObject _pickupModel;
//        private readonly Sprite _pickupIcon;

//        public abstract new Sprite pickupIconSprite { get; }
//        public abstract new GameObject pickupModelPrefab { get; }

//        public CustomEquipmentDef() : base()
//        {
//            base.name = this.name;
//            base.cooldown = this.cooldown;
//            base.nameToken = this.nameToken;
//            base.descriptionToken = this.descriptionToken;
//            base.loreToken = this.loreToken;
//            base.addressToken = this.addressToken;
//            base.unlockableName = this.unlockableName;
//            base.colorIndex = this.colorIndex;
//            base.canDrop = this.canDrop;
//            base.enigmaCompatible = this.enigmaCompatible;
//            base.isLunar = this.isLunar;
//            base.isBoss = this.isBoss;
//            base.passiveBuff = this.passiveBuff;
//            base.appearsInMultiPlayer = this.appearsInMultiPlayer;
//            base.appearsInSinglePlayer = this.appearsInSinglePlayer;

//            this._pickupModel = this.pickupModelPrefab;
//            this._pickupIcon = this.pickupIconSprite;
//        }
//    }
//}


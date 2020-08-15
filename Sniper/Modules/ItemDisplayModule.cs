namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;

    using RoR2;
    using IDRS = RoR2.ItemDisplayRuleSet;
    using NRG = RoR2.ItemDisplayRuleSet.NamedRuleGroup;
    using DRG = RoR2.DisplayRuleGroup;
    using ItemRule = RoR2.ItemDisplayRule;

    using UnityEngine;
    using UnityObject = UnityEngine.Object;
    using MonoMod.Cil;
    using RoR2.Networking;

    internal static class ItemDisplayModule
    {
        internal static IDRS GetSniperItemDisplay(ChildLocator childLoc)
        {
            if(_sniperItemDisplay == null)
            {
                _sniperItemDisplay = CreateSniperItemDisplay(childLoc);
            }

            return _sniperItemDisplay;
        }
        private static IDRS _sniperItemDisplay;


        private static IDRS CreateSniperItemDisplay(ChildLocator childLoc)
        {
            var idrs = ScriptableObject.CreateInstance<IDRS>();
            AddEquips();
            AddItems();




            idrs.namedEquipmentRuleGroups = equipList.ToArray();
            idrs.namedItemRuleGroups = itemList.ToArray();

            //VerifyIdrs(idrs, childLoc);
            return idrs;
        }

        private static void VerifyIdrs(IDRS idrs, ChildLocator childLoc)
        {
            var tempItems = new HashSet<String>(itemNames);
            var tempEquip = new HashSet<String>(equipNames);
            

            foreach(var eq in idrs.namedEquipmentRuleGroups)
            {
                _ = tempEquip.Remove(eq.name);
                foreach(var n in eq.displayRuleGroup.rules)
                {
                    var str = n.childName;
                    if(str == default) continue;
                    if(childLoc.FindChild(str) is null)
                    {
                        Log.Warning($"No child {str}");
                    }
                }
            }
            foreach(var it in idrs.namedItemRuleGroups)
            {
                _ = tempItems.Remove(it.name);
                foreach(var n in it.displayRuleGroup.rules)
                {
                    var str = n.childName;
                    if(str == default) continue;
                    if(childLoc.FindChild(str) is null)
                    {
                        Log.Warning($"No child {str}");
                    }
                }
            }

            foreach(var eq in tempEquip)
            {
#if LOGIDRS
                Log.Warning($"Missing equip display: {eq}");
#warning LOGIDRS is on
#endif
            }
            foreach(var it in tempItems)
            {
#if LOGIDRS
                Log.Warning($"Missing item display: {it}");
#endif
            }

            //foreach(var eq)
        }


        static ItemDisplayModule()
        {
            var pathsToGrabFrom = new[]
            {
                "Prefabs/CharacterBodies/CommandoBody",
                "Prefabs/CharacterBodies/ToolbotBody",
                "Prefabs/CharacterBodies/HuntressBody",
                //"Prefabs/CharacterBodies/EngiBody",
                //"Prefabs/CharacterBodies/MageBody",
                //"Prefabs/CharacterBodies/MercBody",
                //"Prefabs/CharacterBodies/TreebotBody",
                //"Prefabs/CharacterBodies/LoaderBody",
                //"Prefabs/CharacterBodies/CrocoBody",
            };


            foreach(var path in pathsToGrabFrom)
            {
                if(Resources.Load<GameObject>(path)?.GetComponent<ModelLocator>()?.modelTransform?.GetComponent<CharacterModel>()?.itemDisplayRuleSet is IDRS rules)
                {
                    foreach(var eq in rules.namedEquipmentRuleGroups)
                    {
                        _ = equipNames.Add(eq.name);
                        foreach(var v in eq.displayRuleGroup.rules)
                        {
                            GameObject prefab = v.followerPrefab;
                            if(prefab is null) continue;
                            if(item.TryGetValue(prefab.name, out var cur))
                            {
                                if(cur == prefab) continue;

                                Log.Error($"Non identical duplicate found for equip name: {prefab.name}");
                            }
                            item[prefab.name] = prefab;
                        }
                    }
                    foreach(var it in rules.namedItemRuleGroups)
                    {
                        _ = itemNames.Add(it.name);
                        foreach(var v in it.displayRuleGroup.rules)
                        {
                            GameObject prefab = v.followerPrefab;
                            if(prefab is null) continue;
                            if(item.TryGetValue(prefab.name, out var cur))
                            {
                                if(cur == prefab) continue;

                                Log.Error($"Non identical duplicate found for item name: {prefab.name}");
                            }
                            item[prefab.name] = prefab;
                        }
                    }
                } else
                {
                    Log.Error($"Invalid path: {path}");
                }
            }
        }
        private static readonly HashSet<String> itemNames = new HashSet<String>();
        private static readonly HashSet<String> equipNames = new HashSet<String>();
        private static readonly Dictionary<String,GameObject> item = new Dictionary<String, GameObject>();
        private static readonly Dictionary<String,GameObject> equip = new Dictionary<String, GameObject>();

        private static readonly List<NRG> equipList = new List<NRG>();
        private static readonly List<NRG> itemList = new List<NRG>();

        private static void AddItem(this List<NRG> list, String name, params ItemRule[] rules)
        {
            list.Add(new NRG
            {
                name = name,
                displayRuleGroup = new DRG
                {
                    rules = rules
                }
            });
        }
        private static ItemRule Rule(String parent, String prefab, Vector3 localPos = default, Vector3 localAngles = default, Vector3 localScale = default )
        {
            return new ItemRule
            {
                childName = parent,
                followerPrefab = item[prefab],
                localPos = localPos,
                localScale = localScale,
                localAngles = localAngles,
                limbMask = LimbFlags.None,
                ruleType = ItemDisplayRuleType.ParentedPrefab,
            };
        }
        private static ItemRule Rule(LimbFlags limb)
        {
            return new ItemRule
            {
                childName = null,
                followerPrefab = null,
                localPos = default,
                localScale = default,
                localAngles = default,
                limbMask = limb,
                ruleType = ItemDisplayRuleType.LimbMask,
            };
        }
        private static Vector3 V() => new Vector3(0f, 0f, 0f);
        private static Vector3 V(Single all) => new Vector3(all, all, all);
        private static Vector3 V(Single x = 0f, Single y = 0f, Single z = 0f) => new Vector3(x, y, z);
        private static Vector4 V(Single x = 0f, Single y = 0f, Single z = 0f, Single w = 0f) => new Vector4(x, y, z, w);

        private static void AddEquips()
        {
            AddEliteEquips();
            AddFollowerEquips();
            AddGunEquips();
            AddBackEquips();
            AddChestEquips();
        }

        private static void AddEliteEquips()
        {
            equipList.AddItem("AffixRed",
                Rule("Head", "DisplayEliteHorn", V(-0.085f, -0.074f, -0.064f), V(65.392f, 68.02f, -114.032f), V(0.1f)),
                Rule("Head", "DisplayEliteHorn", V(0.085f, -0.074f, -0.064f), V(113.848f, 112.995f, -65.202f), V(0.1f)));
            equipList.AddItem("AffixBlue",
                Rule("Head", "DisplayEliteRhinoHorn", V(0f, -0.0676f, -0.0226f), V(420.349f, -180f, 0f), V(0.3f)),
                Rule("Head", "DisplayEliteRhinoHorn", V(0f, -0.081f, -0.101f), V(400.317f, -180f, 0f), V(0.2f)));
            equipList.AddItem("AffixWhite",
                Rule("Head", "DisplayEliteIceCrown", V(0f, -0.011f, -0.203f), V(9.33f, -180f, 0f), V(0.03f)));
            equipList.AddItem("AffixPoison",
                Rule("Head", "DisplayEliteUrchinCrown", V(0f, -0.0037f, -0.225f), V(170f, 0f, -90f), V(0.06f)));
            equipList.AddItem("AffixHaunted",
                Rule("Head", "DisplayEliteStealthCrown", V(0f, 0f, -0.174f), V(10f, -180f, 0f), V(0.06f)));
        }
        private static void AddFollowerEquips()
        {
            equipList.AddItem("Meteor",
                Rule("Base", "DisplayMeteor", V(-0.554f, 0.121f, -1.01f), V(-90f, 0f, 0f), V(1f)));
            equipList.AddItem("BlackHole",
                Rule("Base", "DisplayGravCube", V(-0.554f, 0.121f, -1.01f), V(-90f, 0f, 0f), V(1f)));
            equipList.AddItem("Saw",
                Rule("Base", "DisplaySawmerang", V(-0.63f, 0.91f, -1.01f), V(-90f, 0f, 0f), V(0.15f)));
        }
        private static void AddGunEquips()
        {
            equipList.AddItem("BFG",
                Rule("MuzzleRailgun", "DisplayBFG", V(0f, -0.069f, 0.385f), V(0f, 0f, 180f), V(0.25f, 0.25f, 0.35f)));
            equipList.AddItem("GoldGat",
                Rule("MuzzleRailgun", "DisplayGoldGat", V(-0.021f, -0.252f, 0.427f), V(0f, -90f, 180f), V(0.1f)));
        }
        private static void AddBackEquips()
        {
            equipList.AddItem("Jetpack",
                Rule("ChestBack", "DisplayBugWings", V(0f, 0.087f, -0.208f), V(-16f, 0f, 0f), V(0.15f)));
        }
        private static void AddChestEquips()
        {
            equipList.AddItem("TeamWarCry",
                Rule("Chest", "DisplayTeamWarCry", V(-0.101f, -0.101f, 0.131f), V(7.67f, -23.9f, -3.48f), V(0.03f)));
        }
        
























        private static void AddItems()
        {
            AddFollowerItems();
            AddFingerItems();
        }

        private static void AddFollowerItems()
        {
            itemList.AddItem("Icicle",
                Rule("Base", "DisplayFrostRelic", V(-0.659f, 0.394f, -0.787f), V(-90f, 0f, 0f), V(1f)));
            itemList.AddItem("Talisman",
                Rule("Base", "DisplayTalisman", V(0.658f, 0.873f, -1.025f), V(-90f, 0f, 0f), V(1f)));
            itemList.AddItem("FocusConvergence",
                Rule("Base", "DisplayFocusedConvergence", V(0.443f, -0.026f, -1.309f), V(-90f, 0f, 0f), V(0.075f)));
        }
        private static void AddFingerItems()
        {
            itemList.AddItem("IceRing",
                Rule("Finger42R", "DisplayIceRing", V(), V(90f, 0f, 0f), V(0.2f)));
            itemList.AddItem("FireRing",
                Rule("Finger22R", "DisplayFireRing", V(), V(90f, 0f, 0f), V(0.2f)));
        }
    }
}

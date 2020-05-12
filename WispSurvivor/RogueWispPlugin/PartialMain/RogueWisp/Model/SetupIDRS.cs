#if ROGUEWISP
using System;
using System.Collections.Generic;
using System.Reflection;

using RoR2;

using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_SetupIDRS()
        {
            this.Load += this.RW_DoIDRSSetup;
        }

        private void PopulateDisplayDict()
        {
            var refRuleset = Resources.Load<GameObject>("Prefabs/CharacterBodies/MageBody")
                .GetComponent<ModelLocator>()
                    .modelTransform
                        .GetComponent<CharacterModel>()
                            .itemDisplayRuleSet;

            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var array1 = typeof(ItemDisplayRuleSet).GetField("namedItemRuleGroups", allFlags ).GetValue( refRuleset ) as ItemDisplayRuleSet.NamedRuleGroup[];
            var array2 = typeof(ItemDisplayRuleSet).GetField("namedEquipmentRuleGroups", allFlags ).GetValue( refRuleset ) as ItemDisplayRuleSet.NamedRuleGroup[];
            foreach( var val in array1 )
            {
                foreach( var val2 in val.displayRuleGroup.rules )
                {
                    var prefab = val2.followerPrefab;
                    if( prefab == null ) continue;
                    var key = prefab.name?.ToLower();
                    if( itemDisplayPrefabs.ContainsKey( key ) ) continue;

                    itemDisplayPrefabs[key] = prefab;
                }
            }

            foreach( var val in array2 )
            {
                foreach( var val2 in val.displayRuleGroup.rules )
                {
                    var prefab = val2.followerPrefab;
                    if( prefab == null ) continue;
                    var key = prefab.name?.ToLower();
                    if( itemDisplayPrefabs.ContainsKey( key ) ) continue;

                    itemDisplayPrefabs[key] = prefab;
                }
            }

            //foreach( var kv in itemDisplayPrefabs )
            //{
            //    Main.LogW( kv.Key );
            //}

        }

        private void RW_DoIDRSSetup()
        {
            this.PopulateDisplayDict();

            var idrs = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            var itemDisplays = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            var equipmentDisplays = new List<ItemDisplayRuleSet.NamedRuleGroup>();

            #region Equipment
            //Wings
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Jetpack",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayBugWings"),
                            childName = "ChestCannon2",
                            localPos = new Vector3( -0.177f, 0.126f, 0f ),
                            localAngles = new Vector3( -180f, -90f, 0f ),
                            localScale = new Vector3( 0.15f, 0.15f, 0.15f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );

            //Crowdfunder
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldGat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayGoldGat"),
                            childName = "shoulder.r",
                            localPos = new Vector3( -0.452f, 0.146f, 0f ),
                            localAngles = new Vector3( -110f, 90f, 0f ),
                            localScale = new Vector3( 0.2f, 0.2f, 0.2f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );
            #endregion
            #region Affixes
            //AffixRed
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixRed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3( 0.111f, 0.404f, 0.045f ),
                            localAngles = new Vector3( 30f, 0f, -30f ),
                            localScale = new Vector3( 0.1f, 0.1f, 0.1f ),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3( -0.111f, 0.404f, 0.045f ),
                            localAngles = new Vector3( 30f, 0f, 30f ),
                            localScale = new Vector3( -0.1f, 0.1f, 0.1f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );

            //AffixBlue
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixBlue",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.355f, 0.096f ),
                            localAngles = new Vector3( -30f, 0f, 0f ),
                            localScale = new Vector3( 0.3f, 0.3f, 0.3f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixWhite
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixWhite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.529f, 0f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.03f, 0.03f, 0.03f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixPoison
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixPoison",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.541f, 0.046f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.05f, 0.05f, 0.05f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixHaunted
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixHaunted",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.552f, 0.045f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.06f, 0.06f, 0.06f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );
            #endregion
            #region Items
            //IncreaseHealing
            itemDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IncreaseHealing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3( 0.26f, 0.4f, 0.36f ),
                            localAngles = new Vector3( 0f, -90f, 0f ),
                            localScale = new Vector3( 0.6f, 0.6f, -0.6f ),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3( -0.26f, 0.4f, 0.36f ),
                            localAngles = new Vector3( 0f, -90f, 0f ),
                            localScale = new Vector3( 0.6f, 0.6f, 0.6f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );


            //NovaOnHeal
            itemDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NovaOnHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3( 0.305f, 0.074f, 0.42f ),
                            localAngles = new Vector3( 0f, -90f, 0f ),
                            localScale = new Vector3( 1f, 1f, -1f ),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3( -0.305f, 0.074f, 0.42f ),
                            localAngles = new Vector3( 0f, -90f, 0f ),
                            localScale = new Vector3( 1f, 1f, 1f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );


            //KillEliteFrenzy
            itemDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "KillEliteFrenzy",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayBrainstalk"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.3881f, 0.0257f ),
                            localAngles = new Vector3( 20f, 0f, 0f ),
                            localScale = new Vector3( 0.2f, 0.2f, 0.2f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );



            //Clover
            itemDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Clover",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = this.LoadDisplay("DisplayClover"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.413f, -0.038f ),
                            localAngles = new Vector3( 180f, 90f, 0f ),
                            localScale = new Vector3( 0.5f, 0.5f, 0.5f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );
            #endregion


            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            var itemArray = itemDisplays.ToArray();
            var equipArray = equipmentDisplays.ToArray();

            typeof( ItemDisplayRuleSet ).GetField( "namedItemRuleGroups", allFlags ).SetValue( idrs, itemArray );
            typeof( ItemDisplayRuleSet ).GetField( "namedEquipmentRuleGroups", allFlags ).SetValue( idrs, equipArray );


            this.RW_charModel.itemDisplayRuleSet = idrs;
        }


        private static Dictionary<String,GameObject> itemDisplayPrefabs = new Dictionary<String,GameObject>();
        private GameObject LoadDisplay( String name )
        {
            if( itemDisplayPrefabs.ContainsKey( name.ToLower() ) )
            {
                return itemDisplayPrefabs[name.ToLower()];
            } else
            {
                Main.LogE( name.ToLower() + " was not found." );
                return null;
            }
        }
    }
}
#endif
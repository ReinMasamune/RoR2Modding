using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Utils;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RoR2.Projectile;

namespace WispSurvivor.Util
{
    public static class PrefabUtilities
    {
        #region General
        /// <summary>
        /// Returns true if the GameObject has a component of type T
        /// Retuns false if the GameObject is null, or does not have a component of type T
        /// </summary>
        /// <typeparam Type to check="T"></typeparam>
        /// <param Object to check="g"></param>
        /// <returns></returns>
        public static bool HasComponent<T>( this GameObject g )
        {
            return g && g.GetComponent<T>() != null;
        }

        /// <summary>
        /// Gets and returns component of type T from GameObject
        /// If there is no component of type T, adds one and returns it
        /// </summary>
        /// <typeparam Type to get="T"></typeparam>
        /// <param Object to check="g"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this GameObject g ) where T : Component
        {
            var v = g.GetComponent<T>();
            if( v == null )
            {
                v = g.AddComponent<T>();
            }
            return v;
        }

        public static T AddOrGetComponent<T>( this Transform g ) where T : Component
        {
            return AddOrGetComponent<T>(g.gameObject);
        }

        public static T AddOrGetComponent<T>( this Component g ) where T : Component
        {
            return AddOrGetComponent<T>(g.gameObject);
        }


        /// <summary>
        /// Creates and returns a copy of a GameObject. 
        /// This copy is disabled immediately on creation and will not call Awake() on its components
        /// </summary>
        /// <param Object to clone="g"></param>
        /// <returns></returns>
        public static GameObject InstantiateClone( this GameObject g, string nameToSet,  bool registerNetwork = true, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0 )
        {
            GameObject prefab = MonoBehaviour.Instantiate<GameObject>(g, GetParent().transform);
            prefab.name = nameToSet;
            if( registerNetwork )
            {
                RegisterPrefabInternal(prefab, file, member, line);
            }
            return prefab;
        }

        public static void RegisterNetworkPrefab( this GameObject g, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            RegisterPrefabInternal(g, file, member, line);
        }

        #endregion

        #region Catalogs
        /// <summary>
        /// Adds a GameObject to the BodyCatalog and returns true
        /// Returns false if the GameObject is null
        /// Will not work after BodyCatalog is initalized.
        /// </summary>
        /// <param Body Prefab="g"></param>
        /// <returns></returns>
        public static bool RegisterNewBody( GameObject g )
        {
            if( g != null )
            {
                RoR2.BodyCatalog.getAdditionalEntries += list =>
                {
                    list.Add(g);
                };

                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a GameObject to the projectile catalog and returns true
        /// GameObject must be non-null and have a ProjectileController component
        /// returns false if GameObject is null or is missing the component
        /// </summary>
        /// <param Projectile Prefab="g"></param>
        /// <returns></returns>
        public static bool RegisterNewProjectile( GameObject g )
        {
            if( g.HasComponent<ProjectileController>() )
            {
                RoR2.ProjectileCatalog.getAdditionalEntries += list =>
                {
                    list.Add(g);
                };

                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a GameObject to the effects catalog and returns true
        /// Returns false if GameObject is null
        /// </summary>
        /// <param Effect Prefab="prefab"></param>
        /// <returns></returns>
        public static bool RegisterNewEffect(GameObject prefab)
        {
            List<GameObject> effects = EffectManager.instance.GetFieldValue<List<GameObject>>("effectPrefabsList");
            Dictionary<GameObject, uint> effectLookup = EffectManager.instance.GetFieldValue<Dictionary<GameObject, uint>>("effectPrefabToIndexMap");

            if (!prefab)
            {
                return false;
            }

            int index = effects.Count;

            effects.Add(prefab);
            effectLookup.Add(prefab, (uint)index);
            return true;
        }
        #endregion

        #region SkinDefs
        /// <summary>
        /// Creates and returns a new SkinDef
        /// Note that selecting modded skins will break a save file if the mod is uninstalled while it is selected
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static SkinDef CreateNewSkinDef( SkinDefInfo skin )
        {
            On.RoR2.SkinDef.Awake += SkinDef_Awake;

            SkinDef newSkin = ScriptableObject.CreateInstance<SkinDef>();

            newSkin.baseSkins = skin.baseSkins;
            newSkin.icon = skin.icon;
            newSkin.unlockableName = skin.unlockableName;
            newSkin.rootObject = skin.rootObject;
            newSkin.rendererInfos = skin.rendererInfos;
            newSkin.nameToken = skin.nameToken;
            newSkin.name = skin.name;
            


            On.RoR2.SkinDef.Awake -= SkinDef_Awake;
            return newSkin;
        }

        /// <summary>
        /// Creates and returns a RendererInfo with the specifified settings
        /// </summary>
        /// <param Renderer="r"></param>
        /// <param Default Material="m"></param>
        /// <returns></returns>
        public static CharacterModel.RendererInfo CreateRendererInfo( Renderer r , Material m , bool ignoreOverlays , UnityEngine.Rendering.ShadowCastingMode shadow )
        {
            CharacterModel.RendererInfo ren = new CharacterModel.RendererInfo();
            ren.renderer = r;
            ren.defaultMaterial = m;
            ren.ignoreOverlays = ignoreOverlays;
            ren.defaultShadowCastingMode = shadow;
            return ren;
        }

        /// <summary>
        /// Struct used to define the fields for a SkinDef before actually creating it
        /// </summary>
        public struct SkinDefInfo
        {
            public SkinDef[] baseSkins;
            public Sprite icon;
            public string nameToken;
            public string unlockableName;
            public GameObject rootObject;
            public CharacterModel.RendererInfo[] rendererInfos;
            public string name;
        }
        #endregion

        #region Internal
        private static void SkinDef_Awake(On.RoR2.SkinDef.orig_Awake orig, SkinDef self)
        {
            //Intentionally do nothing
        }

        private static GameObject parent;
        private static GameObject GetParent()
        {
            if( !parent )
            {
                parent = new GameObject("ModsAreHere");
                MonoBehaviour.DontDestroyOnLoad(parent);
                parent.SetActive(false);

                On.RoR2.Util.IsPrefab += (orig, obj) =>
                {
                    if (obj.transform.parent && obj.transform.parent.gameObject.name == "ModsAreHere") return true;
                    return orig(obj);
                };
            }

            return parent;
        }

        private struct HashStruct
        {
            public GameObject prefab;
            public string goName;
            public string callPath;
            public string callMember;
            public int callLine;
        }

        private static List<HashStruct> thingsToHash = new List<HashStruct>();

        private static bool needToRegister = false;

        private static void RegisterPrefabInternal( GameObject prefab , string callPath, string callMember, int callLine )
        {
            HashStruct h = new HashStruct
            {
                prefab = prefab,
                goName = prefab.name,
                callPath = callPath,
                callMember = callMember,
                callLine = callLine
            };
            thingsToHash.Add(h);
            SetupRegistrationEvent();
        }

        private static void SetupRegistrationEvent()
        {
            if( !needToRegister )
            {
                needToRegister = true;
                On.RoR2.Networking.GameNetworkManager.OnStartClient += RegisterClientPrefabsNStuff;
            }
        }

        private static void RegisterClientPrefabsNStuff(On.RoR2.Networking.GameNetworkManager.orig_OnStartClient orig, RoR2.Networking.GameNetworkManager self, UnityEngine.Networking.NetworkClient newClient)
        {
            orig(self, newClient);
            foreach (HashStruct h in thingsToHash)
            {
                ClientScene.RegisterPrefab(h.prefab, NetworkHash128.Parse(h.goName + h.callPath + h.callMember + h.callLine.ToString()));
            }
        }

        #endregion
    }
}

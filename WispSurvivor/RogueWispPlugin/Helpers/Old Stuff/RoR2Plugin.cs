using BepInEx;

namespace Rein.RoR2Plugin
{
    public abstract partial class RoR2Plugin : BaseUnityPlugin
    {
        public abstract void CreateHooks();

        public abstract void RemoveHooks();

        /*
        public abstract void PluginActivate();

        public abstract void PluginDeactivate();

        public abstract void CatalogEdits();


        protected virtual void Awake()
        {
            CatalogEdits();
        }

        protected virtual void Start()
        {

        }

        */

        protected virtual void OnEnable() => this.CreateHooks();//PluginActivate();//PerformCatalogEdits();

        protected virtual void OnDisable() => this.RemoveHooks();//PluginDeactivate();//UndoCatalogEdits();

        /*
        private void PerformCatalogEdits()
        {

        }

        private void UndoCatalogEdits()
        {

        }
        */
    }
}

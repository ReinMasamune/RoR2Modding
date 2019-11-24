using System;
using BepInEx;
using UnityEngine;

namespace RoR2Plugin
{
    public abstract partial class RoR2Plugin : BaseUnityPlugin
    {
        public abstract void CreateHooks();

        public abstract void RemoveHooks();

        public abstract void OnLoad();

        public abstract void OnUnload();

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

        protected virtual void OnEnable()
        {
            CreateHooks();
            OnLoad();
            //PluginActivate();
            //PerformCatalogEdits();
        }

        protected virtual void OnDisable()
        {
            RemoveHooks();
            OnUnload();
            //PluginDeactivate();
            //UndoCatalogEdits();
        }

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

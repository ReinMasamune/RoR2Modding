using System;
using BepInEx;
using UnityEngine;

namespace RoR2Plugin
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

        protected virtual void OnEnable()
        {
            CreateHooks();
            //PluginActivate();
            //PerformCatalogEdits();
        }

        protected virtual void OnDisable()
        {
            RemoveHooks();
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

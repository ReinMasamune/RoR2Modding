using BepInEx;
using System;
using RoR2;
using UnityEngine;

namespace NoTrafficLawsInSpace
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.1")]

    public class ReinSurvivorMod : BaseUnityPlugin
    {
        public void Awake()
        {
            R2API.SurvivorAPI.SurvivorCatalogReady += delegate (object s, EventArgs e)
            {
                GameObject body = BodyCatalog.FindBodyPrefab("ToolbotBody");
                body.AddComponent<SpeedyCart>();

                //var survivor = new SurvivorDef
                //{
                //    bodyPrefab = body,
                //    descriptionToken = "",
                //    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                //    primaryColor = new Color(0.25f, 0.25f, 0.25f),
                //    unlockableName = "",
                //    survivorIndex = SurvivorIndex.Count
                //};
                //R2API.SurvivorAPI.AddSurvivorOnReady(survivor);
            };
        }
    }
}
#if STAGE
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        internal static SceneDef wispSceneDef;
        internal static List<GameObject> instantiateOnSceneLoad;

        partial void CreateSceneDef();
        partial void CreateSceneObjects();

        partial void CreateStage()
        {
            this.CreateSceneDef();
            this.CreateSceneObjects();
        }
    }

}
#endif
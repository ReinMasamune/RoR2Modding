
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreatePrefabAccessors();
        partial void CreateMeshAccessors();
        partial void CreateMaterialAccessors();
        partial void CreateShaderAccessors();
        partial void CreateTextureAccessors();


        partial void CreateAccessors()
        {
            this.CreatePrefabAccessors();
            this.CreateMeshAccessors();
            this.CreateMaterialAccessors();
            this.CreateShaderAccessors();
            this.CreateTextureAccessors();
        }
    }
}


namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreateMaterialAccessors();
        partial void CreateShaderAccessors();
        partial void CreateTextureAccessors();
        partial void CreateMeshAccessors();
        partial void CreateAccessors()
        {
            this.CreateMaterialAccessors();
            this.CreateShaderAccessors();
            this.CreateTextureAccessors();
            this.CreateMeshAccessors();
        }
    }
}

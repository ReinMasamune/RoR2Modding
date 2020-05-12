namespace Sniper.ScriptableObjects.Custom
{
    using System;

    using UnityEngine;

    [CreateAssetMenu( fileName = "TextureSet", menuName = "Rein/Sniper/SniperTexture", order = 0 )]
    public class SniperTexture : ScriptableObject
    {
        public String textureName;
        public Texture2D texture;
    }
}

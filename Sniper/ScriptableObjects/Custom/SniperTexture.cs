using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sniper.ScriptableObjects.Custom
{
    [CreateAssetMenu( fileName = "TextureSet", menuName = "Rein/Sniper/SniperTexture", order = 0 )]
    public class SniperTexture : ScriptableObject
    {
        public String textureName;
        public Texture2D texture;
    }
}

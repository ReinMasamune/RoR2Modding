namespace MaterialTesting
{
    using System;

    using UnityEngine;
    [CreateAssetMenu(fileName = "MaterialInfo")]
    public class MaterialInfo : ScriptableObject
    {
        public Material material;
        public Mesh target;

        public Single size;
        public Single distance;
    }
}

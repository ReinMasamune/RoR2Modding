#if MATEDITOR
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MaterialEditor : MonoBehaviour
    {
        private Material mat;

        private void Awake()
        {
            this.mat = base.gameObject.GetComponent<MeshRenderer>().material;
        }

        private void OnGUI()
        {

        }
    }

}
#endif
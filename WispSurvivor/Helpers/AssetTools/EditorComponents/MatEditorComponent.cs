#if MATEDITOR
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MaterialEditor : MonoBehaviour
    {
        private Material material;
        private StandardMaterial mat;
        private void Awake()
        {
            this.material = base.gameObject.GetComponent<MeshRenderer>().material;
            if( this.material )
            {
                this.mat = new StandardMaterial( this.material );
            }
        }

        private void OnGUI()
        {

        }
    }

}
#endif
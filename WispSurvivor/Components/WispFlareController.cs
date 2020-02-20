using RogueWispPlugin.Helpers;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispFlareController : MonoBehaviour
        {
            public SpriteRenderer flare1;
            public SpriteRenderer flare2;

            public System.Single intensity = 0f;

            private CharacterBody body;

            private EyeFlare eye1;
            private EyeFlare eye2;
            private Color color;

            public void Start()
            {
                this.body = this.GetComponent<CharacterBody>();
                this.eye1 = this.flare1.GetComponent<EyeFlare>();
                this.eye2 = this.flare2.GetComponent<EyeFlare>();

                //var skin = body.modelLocator.modelTransform.GetComponent<WispModelBitSkinController>();

                var skin = WispBitSkin.GetWispSkin( this.body.skinIndex );
                this.flare1.color = skin.mainColor;
                this.flare2.color = skin.mainColor;
                this.flare1.material = skin.tracerMaterial;
                this.flare2.material = skin.tracerMaterial;
            }

            public void Update()
            {
                if( this.intensity > 0f )
                {
                    this.flare1.gameObject.SetActive( true );
                    this.flare2.gameObject.SetActive( true );
                    this.eye1.localScale = this.eye2.localScale = this.intensity * 4f;
                } else
                {
                    this.flare1.gameObject.SetActive( false );
                    this.flare2.gameObject.SetActive( false );
                }
            }
        }
    }
}

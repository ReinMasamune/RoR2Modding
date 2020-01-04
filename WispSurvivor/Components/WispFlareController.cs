using RoR2;
using UnityEngine;

namespace RogueWispPlugin.Components
{
    public class WispFlareController : MonoBehaviour
    {
        public SpriteRenderer flare1;
        public SpriteRenderer flare2;

        public System.Single intensity = 0f;

        private CharacterBody body;

        private EyeFlare eye1;
        private EyeFlare eye2;

        public void Start()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.eye1 = this.flare1.GetComponent<EyeFlare>();
            this.eye2 = this.flare2.GetComponent<EyeFlare>();
        }

        public void Update()
        {
            if( this.intensity > 0f )
            {
                this.flare1.gameObject.SetActive( true );
                this.flare2.gameObject.SetActive( true );
                this.eye1.localScale = this.eye2.localScale = this.intensity * 4f;
                this.flare1.color = this.flare2.color = Main.fireColors[this.body.skinIndex];
            } else
            {
                this.flare1.gameObject.SetActive( false );
                this.flare2.gameObject.SetActive( false );
            }
        }
    }
}

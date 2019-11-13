using UnityEngine;
using System.Collections.Generic;
using RoR2;

namespace WispSurvivor.Components
{
    public class WispFlareController : MonoBehaviour
    {
        public SpriteRenderer flare1;
        public SpriteRenderer flare2;

        public float intensity = 0f;

        private CharacterBody body;

        private EyeFlare eye1;
        private EyeFlare eye2;

        public void Start()
        {
            body = GetComponent<CharacterBody>();
            eye1 = flare1.GetComponent<EyeFlare>();
            eye2 = flare2.GetComponent<EyeFlare>();
        }

        public void Update()
        {
            if( intensity > 0f )
            {
                flare1.gameObject.SetActive(true);
                flare2.gameObject.SetActive(true);
                eye1.localScale = eye2.localScale = intensity * 4f;
                flare1.color = flare2.color = Modules.WispMaterialModule.fireColors[body.skinIndex];
            } else
            {
                flare1.gameObject.SetActive(false);
                flare2.gameObject.SetActive(false);
            }
        }
    }
}

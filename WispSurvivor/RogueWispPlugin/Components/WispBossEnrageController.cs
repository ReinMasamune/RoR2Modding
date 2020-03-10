using System;
using System.Collections.Generic;
using RogueWispPlugin.Helpers;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispBossEnrageController : MonoBehaviour
        {
            public const Single enrageStartHealthFrac = 0.5f;

            public ParticleSystem bodyParticles;
            public SpriteRenderer flare1;
            public WispModelBitSkinController bodySkin;
            public CharacterBody body;
            public BuffIndex enrageBuffIndex;
            public Single baseFlareIntensity;
            public Single enrageFlareIntensity;
            public Single baseEmissionRate;
            public Single enrageEmissionRate;
            public UInt32 baseSkinIndex;
            public UInt32 enrageSkinIndex;


            private EyeFlare flareController;
            private Boolean enraged = false;
            private Boolean dead = false;
            private WispBitSkin enrageSkin;
            private HealthComponent health;

            private void DoSound()
            {
                for( Int32 i = 0; i < 2; ++i )
                {
                    Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 1.3f );
                    Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 1.6f );
                    Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.45f );
                    Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.65f );
                }
            }

            private void Awake()
            {
                this.flareController = this.flare1.GetComponent<EyeFlare>();
            }

            private void Start()
            {
                //this.flareController = this.flare1.GetComponent<EyeFlare>();
                this.health = this.body.healthComponent;

                this.body.skinIndex = this.baseSkinIndex;

                var skin = WispBitSkin.GetWispSkin( this.baseSkinIndex );
                this.enrageSkin = WispBitSkin.GetWispSkin( this.enrageSkinIndex );

                this.bodySkin.Apply( WispBitSkin.GetWispSkin( this.baseSkinIndex ) );

                this.flare1.color = skin.mainColor;
                this.flare1.material = skin.tracerMaterial;
                this.flareController.localScale = this.baseFlareIntensity;

                var psEmis = this.bodyParticles.emission;
                psEmis.rateOverTime = this.baseEmissionRate;
            }



            private void FixedUpdate()
            {
                if( !this.enraged && this.health.combinedHealthFraction <= enrageStartHealthFrac )
                {
                    this.enraged = true;
                    this.body.skinIndex = this.enrageSkinIndex;
                    this.bodySkin.Apply( this.enrageSkin );
                    this.flare1.color = this.enrageSkin.mainColor;
                    this.flare1.material = this.enrageSkin.tracerMaterial;
                    this.flareController.localScale = this.enrageFlareIntensity;
                    var psEmis = this.bodyParticles.emission;
                    psEmis.rateOverTime = this.enrageEmissionRate;



                    if( NetworkServer.active )
                    {
                        this.body.AddBuff( this.enrageBuffIndex );
                    }
                }

                if( !this.dead && this.health.alive == false )
                {
                    this.dead = true;
                    this.flare1.enabled = false;
                    this.flareController.enabled = false;
                }
            }
        }
    }
}

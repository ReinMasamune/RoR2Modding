using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal abstract class EffectElement : MonoBehaviour
    {
        internal new String name
        {
            get
            {
                return this.obj.name;
            }
            set
            {
                this.obj.name = value;
            }
        }
        internal String path
        {
            get
            {
                return this.parent.path + "/" + this.name;
            }
        }
        internal GameObject obj { get; private set; }
        internal EffectCategory parent { get; private set; }


        internal abstract Vector3 scale { get; set; }
        internal abstract Vector3 position { get; set; }
        internal abstract Quaternion rotation { get; set; }


        internal abstract EffectElement Duplicate( GameObject newObj, EffectCategory newParent );


        internal EffectElement( String name, EffectCategory parent )
        {
            this.obj = new GameObject();
            this.name = name;
            this.parent = parent;
            parent.AddChild( this );

            this.obj.transform.parent = parent.obj.transform.parent;
            this.scale = Vector3.one;
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
        }

        internal EffectElement( EffectElement orig, GameObject newObj, EffectCategory newParent )
        {
            this.obj = newObj;
            this.name = orig.name;
            this.parent = newParent;
        }
    }
}

/*
Element types:

(ActivateGoldBeacon)
Coins
LongLifeNoiseTrails, Bright
Flash, White
Flash, Red
Point light
Dash, Bright
Sphere

(ActivateRadarTower)
ExpandingSphere
MassSparks
Point Light
PP

(Affix White Delay Effect)
Chunks
Nova Sphere
Point light
Flash, White
CoreSphere
Spikes
LightShafts

(AmmoPackPickupEffect)
Ring
Shells

(AncientWispEnrage)
Crosses
SwingTrail

(ArchWispDeath)
Ring
Chunks
Mask
Chunks, Sharp
Flames
Flash
Distortion
Debris
Point light

(ArchWispPreDeath)
LightShafts
Chunks
Flames
Goo
Point light
Flames
Flames, Radial
Flash

(AssassinDaggerTrail)
SwingTrail

(BeamSphereExplosion)
Ring
Chunks, Sharp
Flames
Flash
Point light
Lightning

(BearProc)
Fluff

(BeetleGuardDefenseUp)
Pulse
Shields

(BeetleQueenBurrow)
Debris, 3D
Debris
Dust,3D
Dust,Billboarded
Debris, 3D
Debris
Dust

(BeetleQueenScream)
Distortion
Spit
Debris

(BeetleQueenScreamDeath)
Acid
Distortion
Spit

(BeetleWardDeathEffect)
Bugs
Point Light
Blood
Blooc, Opague

(BellDeath)
BurstLight
Beams

(BellPreDeath)
LightShafts
Flames

(BirdSharkDeathEffect)
SmallFeather
Blood

(BisonChargeStep)
Debris
Dust

(BoostJumpEffect)
Feather, Radial
Feather, Directional

(BootCharging)
Squares
Point light

(BootIsReady)
Squares
Distortion
BlueRing
InnerEnergy
Point light

(BootTrigger)
Squares
Distortion
BlueRing
InnerEnergy
Point light

(BrittleDeath)
BurstLight
Beams

(BubbleShieldEndEffect)
ScaledHitsparks
UnscaledHitsparks
ScaledSmoke, Billboard
ScaledSmokeRing, Mesh
Unscaled Smoke, Billboard
AreaIndicatorRing, Billboard
AreaIndicatorRing, Random Billboard
Physics Sparks
Flash, Soft Glow
Unscaled Flames
Dash, Bright
Point Light
Chunks, Solid
Chunks, Billboards

(ChargeMadeFireBomb)
Base
OrbCore
Point light

(ChargeMageIceBomb)
Point light
Beams
Base
OrbCore

(ChargeMageLightningBomb)
Base
OrbCore
Point light
Sparks, Trail

(ClayBossDeath)




























































































*/
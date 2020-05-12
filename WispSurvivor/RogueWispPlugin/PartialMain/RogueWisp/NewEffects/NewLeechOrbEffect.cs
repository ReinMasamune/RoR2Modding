#if ROGUEWISP
using Rein.RogueWispPlugin.Helpers;

using ReinCore;

using RoR2;

using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_NewLeechOrbEffect()
        {
            this.Load += this.RW_CreateNewLeechOrb;
        }

        private void RW_CreateNewLeechOrb()
        {
            var obj = new GameObject().ClonePrefab("LeechOrb", false);

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = true;
            effComp.soundName = "";

            var skin = obj.AddComponent<WispSkinnedEffect>();

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var eventFuncs = obj.AddComponent<EventFunctions>();

            var orb = obj.AddComponent<WispOrbEffect>();
            orb.startVelocity1 = new Vector3( -10f, 10f, -10f );
            orb.startVelocity2 = new Vector3( 10f, 0f, 10f );
            orb.endVelocity1 = new Vector3( -4f, 0f, -4f );
            orb.endVelocity2 = new Vector3( 4f, 0f, 10f );
            orb.movementCurve = AnimationCurve.EaseInOut( 0f, 0f, 1f, 1f );
            orb.faceMovement = true;
            orb.callArrivalIfTargetIsGone = false;
            orb.soundString = "Play_treeBot_m1_hit_heal";


            var fireParticles = EffectHelper.AddFire( obj, skin, MaterialType.Flames, 1f, 0.3f, 20f, 10f, 0f, true );



            utilityLeech = obj;
            RegisterEffect( utilityLeech );
        }
        /*
partial void RW_LeechOrbEffects() => this.Load += this.RW_CreateLeechOrbEffects;

private void RW_CreateLeechOrbEffects()
{
GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/HauntOrbEffect");

for( Int32 i = 0; i < 8; i++ )
{
utilityLeeches[i] = CreateLeechOrb( baseFX, i );
}
}

private static GameObject CreateLeechOrb( GameObject baseFX, Int32 skinIndex )
{
GameObject obj = baseFX.InstantiateClone("LeechEffect"+skinIndex.ToString(), false);

obj.GetComponent<EffectComponent>().applyScale = true;

MonoBehaviour.Destroy( obj.GetComponent<AkEvent>() );
MonoBehaviour.Destroy( obj.GetComponent<AkGameObj>() );

OrbHelper.ConvertOrbSettings( obj );

obj.GetComponent<WispOrbEffect>().soundString = "Play_treeBot_m1_hit_heal";

Transform vfx = obj.transform.Find("VFX");
Transform core = vfx.Find("Core");

ParticleSystemRenderer corePSR = core.GetComponent<ParticleSystemRenderer>();
ParticleSystem corePS = core.GetComponent<ParticleSystem>();

corePSR.material = fireMaterials[skinIndex][1];

ParticleSystem.MainModule coreMain = corePS.main;
coreMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

return obj;
}
*/
    }

}
#endif
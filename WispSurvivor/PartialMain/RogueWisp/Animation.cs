using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_Animation()
        {
            this.Load += this.RW_SetAnimationController;
            this.Load += this.RW_SetupAimAnimator;
        }

        private void RW_SetupAimAnimator()
        {
            AimAnimator aa = this.RW_body.GetComponent<ModelLocator>().modelTransform.GetComponent<AimAnimator>();
            aa.directionComponent = this.RW_body.GetComponent<CharacterDirection>();
            aa.pitchRangeMin = -45f;
            aa.pitchRangeMax = 45f;
            aa.yawRangeMin = -45f;
            aa.yawRangeMax = 45f;
            aa.pitchGiveupRange = 20f;
            aa.yawGiveupRange = 20f;
            aa.giveupDuration = 4f;
            aa.raisedApproachSpeed = 1000f;
            aa.loweredApproachSpeed = 200f;
            aa.smoothTime = 0.1f;
            aa.fullYaw = false;
            aa.aimType = AimAnimator.AimType.Direct;
            aa.enableAimWeight = false;

            this.RW_body.AddComponent<WispAimAnimationController>();
        }

        private void RW_SetAnimationController()
        {
            ModelLocator ml = this.RW_body.GetComponent<ModelLocator>();
            Animator anim = ml.modelTransform.GetComponent<Animator>();

            RuntimeAnimatorController oac = anim.runtimeAnimatorController;
            RuntimeAnimatorController rac = this.RW_assetBundle.LoadAsset<RuntimeAnimatorController>("Assets/__Export/animAncientWisp.controller");

            Dictionary<String, AnimationClip> origAnimationMap = new Dictionary<String, AnimationClip>();
            Dictionary<String, AnimationClip> newAnimationMap = new Dictionary<String, AnimationClip>();
            Dictionary<String, String> translation = new Dictionary<String, String>();
            Dictionary<AnimationClip, AnimationClip> finalMap = new Dictionary<AnimationClip, AnimationClip>();

            translation.Add( "WispSurvivorSwipe1", "AncientWispArmature|Throw1" );
            translation.Add( "WispSurvivorSwipe2", "AncientWispArmature|Throw2" );

            foreach( AnimationClip ac in oac.animationClips )
            {
                origAnimationMap.Add( ac.name, ac );
            }
            foreach( AnimationClip ac in rac.animationClips )
            {
                newAnimationMap.Add( ac.name, ac );
            }
            foreach( String s in translation.Keys )
            {
                finalMap.Add( newAnimationMap[s], origAnimationMap[translation[s]] );
            }

            anim.runtimeAnimatorController = rac;
            AnimatorOverrideController ovac = new AnimatorOverrideController(rac);
            List<KeyValuePair<AnimationClip, AnimationClip>> replacedAnimations = new List<KeyValuePair<AnimationClip, AnimationClip>>(ovac.overridesCount);
            ovac.GetOverrides( replacedAnimations );
            anim.runtimeAnimatorController = ovac;

            for( Int32 i = 0; i < ovac.overridesCount; i++ )
            {
                KeyValuePair<AnimationClip, AnimationClip> kv = replacedAnimations[i];
                if( finalMap.ContainsKey( kv.Key ) )
                {
                    replacedAnimations[i] = new KeyValuePair<AnimationClip, AnimationClip>( kv.Key, finalMap[kv.Key] );
                }
            }

            ovac.ApplyOverrides( replacedAnimations );
        }
    }
#endif
}

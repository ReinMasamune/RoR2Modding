//namespace Rein.RogueWispPlugin.Modules
//{
/*
public static class WispAnimationModule
{
    public static void DoModule( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
    {
        SetAnimationController( body, dic, bundle );
        SetAimAnimator( body, dic );
    }

    private static void SetAnimationController( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
    {
        ModelLocator ml = dic.C<ModelLocator>();
        Animator anim = ml.modelTransform.GetComponent<Animator>();

        RuntimeAnimatorController oac = anim.runtimeAnimatorController;
        RuntimeAnimatorController rac = bundle.LoadAsset<RuntimeAnimatorController>("Assets/__Export/animAncientWisp.controller");

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

    private static void SetAimAnimator( GameObject body, Dictionary<Type, Component> dic )
    {
        AimAnimator aa = dic.C<ModelLocator>().modelTransform.GetComponent<AimAnimator>();
        aa.directionComponent = dic.C<CharacterDirection>();
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

        body.AddComponent<Components.WispAimAnimationController>();
    }

    private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;
}
*/
//}

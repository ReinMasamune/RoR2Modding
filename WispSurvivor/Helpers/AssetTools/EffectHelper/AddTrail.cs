using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject,UInt32> trailCounter = new Dictionary<GameObject, UInt32>();
        internal static TrailRenderer AddTrail( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, Single width, Single startWidth, Single endWidth, Single time, Boolean applyColor = true )
        {
            if( !trailCounter.ContainsKey( mainObj ) ) trailCounter[mainObj] = 0u;
            var obj = new GameObject( "Trail" + trailCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var trail = obj.AddComponent<TrailRenderer>();
            if( matType != MaterialType.Constant )
            {
                skin.AddRenderer( trail, matType );
            }
            if( applyColor ) skin.AddTrail( trail );

            trail.time = time;
            trail.startWidth = startWidth;
            trail.endWidth = endWidth;
            trail.widthMultiplier = width;

            trail.textureMode = LineTextureMode.Stretch;
            trail.alignment = LineAlignment.View;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            trail.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            trail.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            trail.sortingLayerName = "Default";
            trail.lightProbeProxyVolumeOverride = null;
            trail.probeAnchor = null;
            trail.lightmapScaleOffset = new Vector4( 1f, 1f, 0f, 0f );
            trail.realtimeLightmapScaleOffset = new Vector4( 1f, 1f, 0f, 0f );
            trail.minVertexDistance = 0.025f;
            trail.shadowBias = 0f;
            trail.autodestruct = false;
            trail.emitting = true;
            trail.generateLightingData = false;
            trail.receiveShadows = true;
            trail.allowOcclusionWhenDynamic = true;
            trail.numCornerVertices = 64;
            trail.numCapVertices = 64;
            trail.renderingLayerMask = 1;
            trail.rendererPriority = 0;
            trail.sortingLayerID = 0;
            trail.sortingOrder = 0;
            trail.lightmapIndex = -1;
            trail.realtimeLightmapIndex = -1;

            return trail;
        }
    }
}

namespace Rein.Sniper.Effects
{
    using System;

    using JetBrains.Annotations;

    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateShockOrbEffect()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect").ClonePrefab("ShockOrbEffect", false);
            foreach(var r in obj.GetComponentsInChildren<Renderer>())
            {
                var cm = new CloudMaterial(r.material.Instantiate());
                cm.remapTexture.texture = TextureModule.GetShockAmmoRamp();
                r.material = cm.material;
            }
            obj.GetComponent<OrbEffect>().endEffect = VFXModule.GetShockImpactPrefab();

            return obj;
        }

        internal static GameObject CreateShockImpactPrefab()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniImpactVFXLightning").ClonePrefab("ShockImpactEffect", false);
            foreach(Transform t in obj.transform)
            {
                var ps = t.GetComponent<ParticleSystem>();
                if(ps is not null)
                {
                    
                    static Color ModifyColor(Color input)
                    {
                        Color.RGBToHSV(input, out var h, out var s, out var v);
                        var dif = Math.Abs(0.6666667f - h);
                        if(dif > 0.1f) return input;
                        var oh = h;
                        var os = s;
                        var ov = v;
                        h += 0.6666667f * 2f;
                        h /= 3f;
                        if(dif < 0.02f)
                        {
                            s *= 1.05f;
                            v *= 1.05f;
                        } else if(dif > 0.06f)
                        {
                            if(s > 0.07f)
                            {
                                s /= 1.2f;
                            } else
                            {
                                s *= 1.05f;
                                v *= 1.05f;
                            }
                            // s /= 1.2f;
                        } else
                        {
                            s /= 1.2f;
                        }

                        var c = Color.HSVToRGB(h, s, v);
                        //Log.MessageT($"diff: {dif}\noldColor: {input}\nnewColor: {c}\noldhsv {oh}, {os}, {ov}\nnewhsv {h}, {s}, {v}");
                        return c;
                    }
                    static void ModifyGradient(Gradient input)
                    {
                        for(Int32 i = 0; i < input.colorKeys.Length; ++i)
                        {
                            ref var c = ref input.colorKeys[i];
                            c.color = ModifyColor(c.color);
                        }
                    }

                    var mainM = ps.main;

                    var startColor = mainM.startColor;
                    switch(startColor.mode)
                    {
                        case ParticleSystemGradientMode.Color:
                            startColor.color = ModifyColor(startColor.color);
                            break;

                        case ParticleSystemGradientMode.TwoColors:
                        case ParticleSystemGradientMode.RandomColor:
                            startColor.colorMin = ModifyColor(startColor.colorMin);
                            startColor.colorMax = ModifyColor(startColor.colorMax);
                            break;

                        case ParticleSystemGradientMode.Gradient:
                            ModifyGradient(startColor.gradient);
                            break;

                        case ParticleSystemGradientMode.TwoGradients:
                            ModifyGradient(startColor.gradientMin);
                            ModifyGradient(startColor.gradientMax);
                            break;
                    }
                    mainM.startColor = startColor;
                }
            }
            return obj;
        }
    }
}

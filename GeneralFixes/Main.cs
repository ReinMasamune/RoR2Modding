using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

namespace ReinGeneralFixes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinGeneralBugfixes", "ReinGeneralBugfixes", "1.0.0")]

    public class ReinArchWispDemo : BaseUnityPlugin
    {
        public void Awake()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += (il) =>
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext(MoveType.Before,
                    x => x.MatchLdfld<DamageInfo>("damageType"),
                    x => x.MatchLdcI4(0x80),
                    x => x.MatchAnd(),
                    x => x.MatchLdcI4(0)
                );
                Debug.Log(c);
                c.RemoveRange(33);
                Debug.Log(c);
                //c.Emit(OpCodes.Ldarg_1);
                c.Emit(OpCodes.Ldarg_2);
                c.EmitDelegate<Action<DamageInfo,GameObject>>( (di,vic) =>
                {
                    
                    if( (di.damageType & DamageType.IgniteOnHit ) > DamageType.Generic )
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.Burn, 4f * di.procCoefficient, 1f);
                    }

                    if ((di.damageType & DamageType.PercentIgniteOnHit) > DamageType.Generic)
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.PercentBurn, 4f * di.procCoefficient, 1f);
                    }

                    if (di.attacker.GetComponent<CharacterBody>().HasBuff(BuffIndex.AffixRed) )
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.PercentBurn, 4f * di.procCoefficient, 1f);
                    }
                    
                });
                Debug.Log(il);
            };
        }
    }
}
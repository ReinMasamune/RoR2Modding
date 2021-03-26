namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Utils;

    using RoR2;

    using PropertyAttributes = Mono.Cecil.PropertyAttributes;
    using MethodAttributes = Mono.Cecil.MethodAttributes;
    using TypeAttributes = Mono.Cecil.TypeAttributes;
    using ParameterAttributes = Mono.Cecil.ParameterAttributes;
    using RoR2.UI;
    using MonoMod.Cil;
    using BepInEx.Configuration;


    using Bf = System.Reflection.BindingFlags;
    using System.Xml;
    using UnityEngine;


    using UnityObject = UnityEngine.Object;
    using Object = System.Object;



    public static class BodiesCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void Add(GameObject def)
        {
            addedBodies.Add(def);
        }



        static BodiesCore()
        {
            //Log.Warning("SurvivorsCore loaded");
            //HooksCore.RoR2.SurvivorCatalog.Init.On += Init_On;
            HooksCore.RoR2.BodyCatalog.Init.Il += Init_Il;
            HooksCore.RoR2.BodyCatalog.SetBodyPrefabs.Il += SetBodyPrefabs_Il;

            loaded = true;
        }
        private static readonly List<GameObject> addedBodies = new();
        private static void SetBodyPrefabs_Il(ILContext il)
        {
            var c = new ILCursor(il);
            if(c.TryGotoNext(MoveType.After, x => x.MatchCallOrCallvirt<Array>("Sort")))
            {
                c
                    .Mark(out var lab)
                    .Move(-11)
                    .MoveAfterLabels()
                    .Br_(lab);

            } else
            {
                Log.Warning("No sort call in BodyCatalog.SetBodyPrefabs");
            }
        }



        private static void Init_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(BodyCatalog), nameof(BodyCatalog.SetBodyPrefabs)))
            .CallDel_(ArrayHelper.AppendDel(addedBodies));
    }
}

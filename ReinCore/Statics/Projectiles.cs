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



    public static class ProjectilesCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void Add(GameObject def)
        {
            added.Add(def);
        }



        static ProjectilesCore()
        {
            //Log.Warning("SurvivorsCore loaded");
            //HooksCore.RoR2.SurvivorCatalog.Init.On += Init_On;
            HooksCore.RoR2.ProjectileCatalog.Init.Il += Init_Il;
            HooksCore.RoR2.ProjectileCatalog.SetProjectilePrefabs.Il += SetProjectilePrefabs_Il;

            loaded = true;
        }
        private static readonly List<GameObject> added = new();
        private static void SetProjectilePrefabs_Il(ILContext il)
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
                Log.Warning("No sort call in ProjectileCatalog.SetProjectilePrefabs");
            }

            //c.LogFull();
        }



        private static void Init_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(ProjectileCatalog), nameof(ProjectileCatalog.SetProjectilePrefabs)))
            .CallDel_(ArrayHelper.AppendDel(added));
    }
}

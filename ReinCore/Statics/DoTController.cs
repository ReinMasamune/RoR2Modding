namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq.Expressions;
    using System.Reflection;


    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using Rewired.Utils.Classes.Data;

    using RoR2;


    //public static class DotExtensions
    //{
    //    public static void Inflict<TDot>(this CharacterBody body, TDot dot, CharacterBody attacker)
    //        where TDot : struct, IDotType => DotController<TDot>.Inflict(body, dot, attacker);
    //}

    internal static class DotController<TDot>
        where TDot : struct, IDotType
    {
        //internal static void Inflict(CharacterBody body, TDot dot, CharacterBody attacker)
        //{
        //    if(!active) Activate();

        //    var id = body.GetInstanceID();
        //    if(id == 0)
        //    {
        //        Log.Error("Unable to apply dot to body with instance id 0");
        //        return;
        //    }
        //    if(characterLookup.TryGetValue(id, out var kv))
        //    {
        //        kv.dots.Add(new())
        //    } 
        //}





        //private static readonly Dictionary<Int32, (CharacterBody body, RentList<DotContext> dots)> characterLookup = new();

        private static Boolean active = false;

        //private static void Activate() => RoR2Application.onFixedUpdate += FixedUpdate;
        //private static void Deactivate() => RoR2Application.onFixedUpdate -= FixedUpdate;

        //private static void FixedUpdate()
        //{
        //    if(characterLookup.Count == 0)
        //    {
        //        characterLookup.Clear();
        //        Deactivate();
        //        return;
        //    }
        //}



        private struct DotContext
        {
            internal DotContext(TDot dot, CharacterBody attacker)
            {
                this.dot = dot;
                this.removeMe = false;
                this.attacker = attacker;
            }

            internal Boolean removeMe;
            internal readonly TDot dot;
            internal CharacterBody attacker;
        }
    }

    public interface IDotType : ISerializableObject
    { 
    }
}
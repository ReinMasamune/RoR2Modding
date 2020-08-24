namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using EntityStates.Missions.Goldshores;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using RoR2;


    public static class DotExtensions
    {
        public static void Inflict<TDot>(this TDot dot)
            where TDot : struct, IDotType => DotController<TDot>.Inflict(dot);
    }

    internal static class DotController<TDot>
        where TDot : struct, IDotType
    {
        internal static void Inflict(TDot dot)
        {
            if(!active) Activate();

            if(characterStarts.TryGetValue(dot.target, out var start))
            {
                _ = activeDots.AddAfter(start, new DotContext(dot));
            } else
            {
                characterStarts[dot.target] = activeDots.AddLast(new DotContext(dot, true));
            }
        }





        private static readonly LinkedList<DotContext> activeDots = new LinkedList<DotContext>();
        private static readonly Dictionary<CharacterBody, LinkedListNode<DotContext>> characterStarts = new Dictionary<CharacterBody, LinkedListNode<DotContext>>();

        private static Boolean active = false;

        private static void Activate() => RoR2Application.onFixedUpdate += FixedUpdate;
        private static void Deactivate() => RoR2Application.onFixedUpdate -= FixedUpdate;

        private static void FixedUpdate()
        {
            
        }




        private struct DotContext
        {
            internal DotContext(TDot dot, Boolean isCharacterHead = false)
            {
                this.dot = dot;
                this.isCharacterHead = isCharacterHead;
            }

            internal readonly TDot dot;
            internal readonly Boolean isCharacterHead;
        }
    }

    public interface IDotType : ISerializableObject
    {
        Boolean shouldRemove { get; }
        CharacterBody target { get; }
    }
}
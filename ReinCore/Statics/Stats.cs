namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Mono.Security.X509.Extensions;

    using MonoMod.Cil;

    using RoR2;

    


    public static class StatsCore
    {
        public static class ItemValues
        {

        }
        public static class BuffValues
        {

        }

        


        

        
    }

    public readonly ref struct ItemCounts
    {
        internal static Int32 GetRequiredBytes()
        {
            return ItemCatalog.itemCount * 4;
        }

        private readonly unsafe UInt32* counts;
        private readonly UInt32 maxValue;

        internal unsafe ItemCounts(void* ptr, CharacterBody body)
        {
            this.maxValue = (UInt32)(ItemCatalog.itemCount - 1);
            this.counts = (UInt32*)ptr;

            var inv = body.inventory;

            if(inv is null)
            {
                for(UInt32 i = 0; i < ItemCatalog.itemCount; ++i)
                {
                    this[i] = 0u;
                }
            } else
            {
                for(UInt32 i = 0; i < ItemCatalog.itemCount; ++i)
                {
                    this[i] = (UInt32)inv.GetItemCount((ItemIndex)i);
                }
            }
        }

        public unsafe UInt32 this[ItemIndex item]
        {
            get
            {
                var ind = (UInt32)item;
                if(ind > this.maxValue) throw new IndexOutOfRangeException();
                return this.counts[ind];
            }
        }

        private unsafe UInt32 this[UInt32 index]
        {
            set
            {
                if(index > this.maxValue) throw new IndexOutOfRangeException();
                this.counts[index] = value;
            }
        }
    }
    public readonly ref struct BuffCounts
    {
        internal static Int32 GetRequiredBytes()
        {
            return BuffCatalog.buffCount * 4;
        }

        private readonly unsafe UInt32* counts;
        private readonly UInt32 maxValue;

        internal unsafe BuffCounts(void* ptr, CharacterBody body)
        {
            this.maxValue = (UInt32)(BuffCatalog.buffCount - 1);
            this.counts = (UInt32*)ptr;

            for(UInt32 i = 0; i < BuffCatalog.buffCount; ++i)
            {
                this.counts[i] = (UInt32)body.GetBuffCount((BuffIndex)i);
            }
        }

        public unsafe UInt32 this[BuffIndex item]
        {
            get
            {
                var ind = (UInt32)item;
                if(ind > this.maxValue) throw new IndexOutOfRangeException();
                return this.counts[ind];
            }
        }

        private unsafe UInt32 this[UInt32 index]
        {
            set
            {
                if(index > this.maxValue) throw new IndexOutOfRangeException();
                this.counts[index] = value;
            }
        }
    }



    public delegate void Emitter(ILCursor cursor, Int32 statLoc, Int32 itemsLoc, Int32 buffsLoc);
    public interface IStatModifier<TStat>
    {
        Emitter Emit { get; }
    }


    public interface IConditionEmitter
    {
        ILCursor Emit(ILCursor cursor, Int32 itemsLoc, Int32 buffsLoc, ILLabel skip);
    }
    public interface IOperationEmitter
    {
        ILCursor Emit(ILCursor cursor, Int32 itemsLoc, Int32 buffsLoc);
    }
    public class ConditionalOperation<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit { get => (cursor, statLoc, itemsLoc, buffsLoc) =>
        {
            this.OperationEmitter.Emit(this.ConditionEmitter.Emit(cursor.MoveAfterLabels().DefLabel(out var label), itemsLoc, buffsLoc, label).LdLoc_(statLoc), itemsLoc, buffsLoc)
            .StLoc_(statLoc)
            .MarkLabel(label);
        };}

        private readonly IConditionEmitter ConditionEmitter;
        private readonly IOperationEmitter OperationEmitter;

        ConditionalOperation(IConditionEmitter condition, IOperationEmitter operation)
        {
            this.ConditionEmitter = condition;
            this.OperationEmitter = operation;
        }
    }

    public static class Condition
    {
        public static IConditionEmitter HasItem(ItemIndex item) => new HasItemConst(item);
        private struct HasItemConst : IConditionEmitter
        {
            internal HasItemConst(ItemIndex item) => this.item = item;
            private readonly ItemIndex item;

            public ILCursor Emit(ILCursor cursor, Int32 itemsLoc, Int32 buffsLoc, ILLabel skip) => throw new NotImplementedException();
        }
        public static IConditionEmitter HasItem(Func<ItemIndex> itemIndexGetter) => new HasItemDynamic(itemIndexGetter);
        private class HasItemDynamic : IConditionEmitter
        {
            internal HasItemDynamic(Func<ItemIndex> getter) => this.getter = getter;
            private readonly Func<ItemIndex> getter;

            public ILCursor Emit(ILCursor cursor, Int32 itemsLoc, Int32 buffsLoc, ILLabel skip) => throw new NotImplementedException();
        }
        
        //public static IConditionEmitter HasBuff(BuffIndex buff)
        //{

        //}
        //public static IConditionEmitter HasBuff(Func<BuffIndex> buffIndexGetter)
        //{

        //}

    }

    public static class Operation
    {

    }



    public class ItemDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdLocA_(itemsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(in ItemCounts items, ref TStat stat);
        public readonly Modifier modifier;

        public ItemDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class BuffDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdLocA_(buffsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(in BuffCounts buffs, ref TStat stat);
        public readonly Modifier modifier;

        public BuffDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class BodyDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdArg_(0)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(CharacterBody body, ref TStat stat);
        public readonly Modifier modifier;

        public BodyDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class ItemBuffDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdLocA_(itemsLoc)
                .LdLocA_(buffsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(in ItemCounts items, in BuffCounts buffs, ref TStat stat);
        public readonly Modifier modifier;

        public ItemBuffDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class ItemBodyDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdArg_(0)
                .LdLocA_(itemsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(CharacterBody body, in ItemCounts items, ref TStat stat);
        public readonly Modifier modifier;

        public ItemBodyDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class BuffBodyDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdArg_(0)
                .LdLocA_(buffsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(CharacterBody body, in BuffCounts buffs, ref TStat stat);
        public readonly Modifier modifier;

        public BuffBodyDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class ItemBuffBodyDelegate<TStat> : IStatModifier<TStat>
    {
        public Emitter Emit
        {
            get => (cursor, statLoc, itemsLoc, buffsLoc) => _ = cursor
                .LdArg_(0)
                .LdLocA_(itemsLoc)
                .LdLocA_(buffsLoc)
                .LdLocA_(statLoc)
                .CallDel_<Modifier>(this.modifier);
        }

        public delegate void Modifier(CharacterBody body, in ItemCounts items, in BuffCounts buffs, ref TStat stat);
        public readonly Modifier modifier;

        public ItemBuffBodyDelegate(Modifier modifier) => this.modifier = modifier;
    }

    public class Stat<TStat>
            where TStat : struct
    {
        private LinkedList<IStatModifier<TStat>> modifiers = new LinkedList<IStatModifier<TStat>>();

        private struct LayerMarker : IStatModifier<TStat>
        {
            public Emitter Emit => (_, _, _, _) => { };
        }

        public enum Layer
        {

        }

        private readonly Dictionary<Layer,LinkedListNode<IStatModifier<TStat>>> layerMarkers;
    }
}


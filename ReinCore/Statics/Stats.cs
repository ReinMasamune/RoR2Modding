namespace ReinCore.RecalcStats
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    using JetBrains.Annotations;

    using Mono.Security.X509.Extensions;

    using MonoMod.Cil;

    using RoR2;
    
    public static class Tests
    {
        public static Context Test1(Context context) => context
            .Var(out Local<int> someNumber);
        public static Context Test2(Context context) => context
            .Var(out Local<int> someNumber, 6);
        public static Context Test3(Context context) => context
            .Var(out Local<int> someNumber, () => 6);
    }

    public static class Stats
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void Action();
        public delegate void Action<TLoc>(Local<TLoc> local);
        public delegate void Action<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2);

        public delegate void StatsAction(CharacterStats stats);
        public delegate void StatsAction<TLoc>(Local<TLoc> local, CharacterStats stats);
        public delegate void StatsAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats);

        public delegate void ItemAction(ItemCounts items);
        public delegate void ItemAction<TLoc>(Local<TLoc> local, ItemCounts items);
        public delegate void ItemAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items);

        public delegate void BuffAction(BuffCounts buffs);
        public delegate void BuffAction<TLoc>(Local<TLoc> local, BuffCounts buffs);
        public delegate void BuffAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, BuffCounts buffs);

        public delegate void StatsItemAction(CharacterStats stats, ItemCounts items);
        public delegate void StatsItemAction<TLoc>(Local<TLoc> local, CharacterStats stats, ItemCounts items);
        public delegate void StatsItemAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items);

        public delegate void StatsBuffAction(CharacterStats stats, BuffCounts buffs);
        public delegate void StatsBuffAction<TLoc>(CharacterStats stats, BuffCounts buffs);
        public delegate void StatsBuffAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, BuffCounts buffs);

        public delegate void ItemBuffAction(ItemCounts items, BuffCounts buffs);
        public delegate void ItemBuffAction<TLoc>(Local<TLoc> local, ItemCounts items, BuffCounts buffs);
        public delegate void ItemBuffAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items, BuffCounts buffs);

        public delegate void StatsItemBuffAction(CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate void StatsItemBuffAction<TLoc>(Local<TLoc> local, CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate void StatsItemBuffAction<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items, BuffCounts buffs);


        public delegate Boolean Condition();
        public delegate Boolean Condition<TLoc>(Local<TLoc> local);
        public delegate Boolean Condition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2);

        public delegate Boolean StatsCondition(CharacterStats stats);
        public delegate Boolean StatsCondition<TLoc>(Local<TLoc> local, CharacterStats stats);
        public delegate Boolean StatsCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats);


        public delegate Boolean ItemCondition(ItemCounts items);
        public delegate Boolean ItemCondition<TLoc>(Local<TLoc> local, ItemCounts items);
        public delegate Boolean ItemCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items);


        public delegate Boolean BuffCondition(BuffCounts buffs);
        public delegate Boolean BuffCondition<TLoc>(Local<TLoc> local, BuffCounts buffs);
        public delegate Boolean BuffCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, BuffCounts buffs);


        public delegate Boolean StatsItemCondition(CharacterStats stats, ItemCounts items);
        public delegate Boolean StatsItemCondition<TLoc>(Local<TLoc> local, CharacterStats stats, ItemCounts items);
        public delegate Boolean StatsItemCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items);


        public delegate Boolean StatsBuffCondition(CharacterStats stats, BuffCounts buffs);
        public delegate Boolean StatsBuffCondition<TLoc>(CharacterStats stats, BuffCounts buffs);
        public delegate Boolean StatsBuffCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, BuffCounts buffs);


        public delegate Boolean ItemBuffCondition(ItemCounts items, BuffCounts buffs);
        public delegate Boolean ItemBuffCondition<TLoc>(Local<TLoc> local, ItemCounts items, BuffCounts buffs);
        public delegate Boolean ItemBuffCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items, BuffCounts buffs);


        public delegate Boolean StatsItemBuffCondition(CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate Boolean StatsItemBuffCondition<TLoc>(Local<TLoc> local, CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate Boolean StatsItemBuffCondition<TLoc1, TLoc2>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items, BuffCounts buffs);


        public delegate TReturn Func<TReturn>();
        public delegate TReturn Func<TLoc, TReturn>(Local<TLoc> local);
        public delegate TReturn Func<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2);

        public delegate TReturn StatsFunc<TReturn>(CharacterStats stats);
        public delegate TReturn StatsFunc<TLoc, TReturn>(Local<TLoc> local, CharacterStats stats);
        public delegate TReturn StatsFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats);


        public delegate TReturn ItemFunc<TReturn>(ItemCounts items);
        public delegate TReturn ItemFunc<TLoc, TReturn>(Local<TLoc> local, ItemCounts items);
        public delegate TReturn ItemFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items);


        public delegate TReturn BuffFunc<TReturn>(BuffCounts buffs);
        public delegate TReturn BuffFunc<TLoc, TReturn>(Local<TLoc> local, BuffCounts buffs);
        public delegate TReturn BuffFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, BuffCounts buffs);


        public delegate TReturn StatsItemFunc<TReturn>(CharacterStats stats, ItemCounts items);
        public delegate TReturn StatsItemFunc<TLoc, TReturn>(Local<TLoc> local, CharacterStats stats, ItemCounts items);
        public delegate TReturn StatsItemFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items);


        public delegate TReturn StatsBuffFunc<TReturn>(CharacterStats stats, BuffCounts buffs);
        public delegate TReturn StatsBuffFunc<TLoc, TReturn>(CharacterStats stats, BuffCounts buffs);
        public delegate TReturn StatsBuffFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, BuffCounts buffs);


        public delegate TReturn ItemBuffFunc<TReturn>(ItemCounts items, BuffCounts buffs);
        public delegate TReturn ItemBuffFunc<TLoc, TReturn>(Local<TLoc> local, ItemCounts items, BuffCounts buffs);
        public delegate TReturn ItemBuffFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, ItemCounts items, BuffCounts buffs);


        public delegate TReturn StatsItemBuffFunc<TReturn>(CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate TReturn StatsItemBuffFunc<TLoc, TReturn>(Local<TLoc> local, CharacterStats stats, ItemCounts items, BuffCounts buffs);
        public delegate TReturn StatsItemBuffFunc<TLoc1, TLoc2, TReturn>(Local<TLoc1> local1, Local<TLoc2> local2, CharacterStats stats, ItemCounts items, BuffCounts buffs);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    public interface IContext
    {
        Context context { get; }
    }

    public sealed class Context : IContext
    {
        public Context context => this;

        internal Local<T> DefineLocal<T>()
        {
            throw new NotImplementedException();
        }
    }
    public sealed class IfContext<T> : IContext
        where T : IContext
    {
        internal IfContext(T under) => this.under = under;

        internal T under;

        public Context context => this.under.context;
    }
    public sealed class ElseContext<T> : IContext
        where T : IContext
    {
        internal ElseContext(T under) => this.under = under;

        internal T under;

        public Context context => this.under.context;
    }



    public class CharacterStats
    {
        public ReadonlyStat<Int32> level;
    }

    public class Stat<T>
    {

    }

    public class ReadonlyStat<T>
    {

    }

    public class Local<T>
    {

    }



    public readonly struct ItemCounts
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
    public readonly struct BuffCounts
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


    public static class DoXtn
    {
        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.Action> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.Action<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.Action<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.StatsAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.StatsAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.StatsAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.ItemAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.ItemAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.ItemAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.BuffAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.BuffAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.BuffAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.StatsItemAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.StatsItemAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.StatsItemAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.StatsBuffAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.StatsBuffAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.StatsBuffAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.ItemBuffAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.ItemBuffAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.ItemBuffAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }


        public static TCtx Do<TCtx>(this TCtx ctx, Expression<Stats.StatsItemBuffAction> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc>(this TCtx ctx, Local<TLoc> local, Expression<Stats.StatsItemBuffAction<TLoc>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
        public static TCtx Do<TCtx, TLoc1, TLoc2>(this TCtx ctx, Local<TLoc1> local1, Local<TLoc2> local2, Expression<Stats.StatsItemBuffAction<TLoc1, TLoc2>> expr)
            where TCtx : IContext
        {
            return ctx;
        }
    }

    public static class VarXtn
    {
        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, TLoc value = default)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.Func<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.Func<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.ItemFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.ItemFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.BuffFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.BuffFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.StatsFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.StatsFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.StatsItemFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.StatsItemFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.StatsBuffFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.StatsBuffFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.ItemBuffFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.ItemBuffFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }


        public static TCtx Var<TCtx, TLoc>(this TCtx ctx, out Local<TLoc> local, Expression<Stats.StatsItemBuffFunc<TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
        public static TCtx Var<TCtx, TLoc, TInLoc>(this TCtx ctx, out Local<TLoc> local, Local<TInLoc> inLocal, Expression<Stats.StatsItemBuffFunc<TInLoc, TLoc>> expr)
            where TCtx : IContext
        {
            local = ctx.context.DefineLocal<TLoc>();
            return ctx;
        }
    }

    public static class IfXtn
    {
        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.Condition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.Condition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.Condition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.ItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.ItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.ItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.BuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.BuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.BuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.StatsCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.StatsCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.StatsCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.StatsItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.StatsItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.StatsItemCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.StatsBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.StatsBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.StatsBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.ItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.ItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.ItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }



        public static IfContext<TCtx> If<TCtx>(this TCtx ctx, Expression<Stats.StatsItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc>(this TCtx ctx, Expression<Stats.StatsItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
        public static IfContext<TCtx> If<TCtx, TLoc1, TLoc2>(this TCtx ctx, Expression<Stats.StatsItemBuffCondition> expr)
            where TCtx : IContext
        {
            return default;
        }
    }

    public static class EndXtn
    {
        public static TCtx End<TCtx>(this IfContext<TCtx> ctx)
            where TCtx : IContext
        {
            return default;
        }

        public static TCtx End<TCtx>(this ElseContext<TCtx> ctx)
            where TCtx : IContext
        {
            return default;
        }
    }

    public static class ElseXtn
    {
        public static ElseContext<TCtx> Else<TCtx>(this IfContext<TCtx> ctx)
            where TCtx : IContext
        {
            return default;
        }
    }
}


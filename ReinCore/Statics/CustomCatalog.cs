namespace ex
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using RoR2;



    public abstract class CustomCatalog<TSelf, TDef, TIndex>
        where TSelf : CustomCatalog<TSelf, TDef, TIndex>, new()
        where TDef : Def<TIndex>
        where TIndex : unmanaged, Enum, IComparable<UInt64>, IEquatable<UInt64>, IComparable, IFormattable, IConvertible
    {
        public static ICatalogHandle handle { get; } = new CatalogHandle<TSelf, TDef, TIndex>();
        //Various functions for looking up by index and name
        public static TDef? GetDef(TIndex index)
        {
            EnsureInitialized();
            index.ToValue(out UInt64 ind);
            return definitions![ind];
        }
        public static TDef GetDef(String guid)
        {
            EnsureInitialized();
            return guidToDef[guid];
        }

        public static Boolean TryGetDef(TIndex index, out TDef? def)
        {
            def = null;
            if(!initialized) return false;
            index.ToValue(out UInt64 ind);
            if(ind >= count) return false;
            def = definitions![ind];
            return true;
        }

        public static Boolean TryGetDef(String guid, out TDef? def)
        {
            def = null;
            if(!initialized) return false;
            return guidToDef.TryGetValue(guid, out def);
        }


        public sealed class Token
        {
            public TDef def { get; }
            public Boolean registered { get; private set; }
            public Boolean added => this.index is not null;
            public TIndex? index => this.def.index;
            public void Register()
            {
                if(this.registered) return;
                moddedEntries += this.RegistrationFunc;
                this.registered = true;
            }
            public void Unregister()
            {
                if(!this.registered) return;
                moddedEntries -= this.RegistrationFunc;
                this.registered = false;
            }
            internal Token(TDef def) => this.def = def;
            private TDef RegistrationFunc() => this.def;
        }

        public static UInt64 count => (UInt64)(definitions?.LongLength ?? 0L);

        public static Token Add(TDef item)
        {
            var tok = new Token(item);
            tok.Register();
            outstandingTokens.Add(tok);
            return tok;
        }

        private static readonly List<Token> outstandingTokens = new();


        public static event Action onPreInit;
        public static event Action onPostInit;
        public static event Action onCatalogReset;

        private static event Func<TDef> moddedEntries;

        private static TDef[]? definitions;
        private static readonly Dictionary<String, TDef> guidToDef = new();

        private static Boolean initialized = false;


        protected static readonly TSelf instance = new();

        private static UInt64 _curIndex = 0ul;
        private static Boolean hooksApplied = false;
        private static TIndex currentIndex
        {
            get
            {
                _curIndex.ToEnum(out TIndex index);
                return index;
            }
            set => _curIndex = value.ToUInt64(null);
        }
        protected CustomCatalog()
        {
            if(instance is not null) throw new InvalidOperationException("Instance already created");
        }

        public static void OnDepReset()
        {
            if(initialized)
            {
                initialized = false;
                foreach(var def in definitions!)
                {
                    def.index = null;
                    def.active = false;
                }
                definitions = null;
                guidToDef.Clear();
            }
        }

        internal static void EnsureInitialized()
        {
            if(!initialized) throw new InvalidOperationException("Catalog is not initialized");
        }

        internal static void InitializeIfNeeded()
        {
            if(!initialized) InitCatalog();
        }

        private static void InitCatalog()
        {
            var deps = instance.dependencies;
            foreach(var dep in deps)
            {
                dep.InitializeIfNeeded();
                if(!hooksApplied)
                {
                    dep.onCatalogReset += OnDepReset;
                    dep.onPostInit += InitializeIfNeeded;
                }
            }
            hooksApplied = true;


            var preSubbed = onPreInit?.Subscribed();
            if(preSubbed is not null)
            {
                foreach(var item in preSubbed)
                {
                    try
                    {
                        item.Invoke();
                    } catch(Exception e)
                    {
                        //Log error
                    }
                }
            }


            foreach(var def in definitions)
            {
                def.active = false;
                def.index = null;
            }
            definitions = null; // Return to pool instead;
            guidToDef.Clear();

            InitEntries();
            instance.ProcessAllDefinitions(definitions!);
            initialized = true;

            var postSubbed = onPostInit?.Subscribed();
            if(postSubbed is not null)
            {
                foreach(var item in postSubbed)
                {
                    try
                    {
                        item.Invoke();
                    } catch(Exception e)
                    {
                        //Log error
                    }
                }
            } 
        }

        private static void InitEntries()
        {
            var baseEntries = instance.GetBaseEntries();

            foreach(var entry in baseEntries)
            {
                Register(entry);
            }

            var moddedEntries = GetModdedEntries().OrderBy(def => def.guid);

            foreach(var entry in moddedEntries)
            {
                Register(entry);
            }

            definitions = new TDef[_curIndex];
            foreach(var def in guidToDef.Values)
            {
                if(def.index is not TIndex index)
                {
                    // Log potentially fatal error, this code should be entirely unreachable.
                    continue;
                }
                definitions[index.ToUInt64(null)] = def;
            }
        }

        private static IEnumerable<TDef> GetModdedEntries()
        {
            var result = Enumerable.Empty<TDef>();
            var subs = moddedEntries?.Subscribed();
            if(subs is null) return result;

            foreach(var v in subs)
            {
                var item =
                result = result.Append(v());
            }
            return result;
        }

        private static void Register(TDef def)
        {
            if(guidToDef.TryGetValue(def.guid, out var existing) && existing.index is TIndex index)
            {
                existing.active = false;
                existing.index = null;
                def.index = index;
                //Log existing overwritten info
            } else
            {
                def.index = currentIndex;
                _curIndex++;
            }

            def.active = true;
            guidToDef[def.guid] = def;

            instance.RegisterDef(def);
        }

        protected abstract IEnumerable<TDef> GetBaseEntries();
        protected abstract IEnumerable<ICatalogHandle> dependencies { get; }
        protected abstract void RegisterDef(TDef def);
        protected abstract void ProcessAllDefinitions(TDef[] definitions);
    }

    public abstract class Def<TIndex>
        where TIndex : unmanaged, Enum, IComparable<UInt64>, IEquatable<UInt64>, IComparable, IFormattable, IConvertible
    {
        public TIndex? index { get; internal set; }
        public String guid { get; }
        public Boolean active { get; internal set; }
    }

    internal static class MiscXtn
    {
        public static IEnumerable<TDelegate> Subscribed<TDelegate>(this TDelegate self)
            where TDelegate : MulticastDelegate
            => self.GetInvocationList().Cast<TDelegate>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ToValue<TFrom, TTo>(this TFrom from, out TTo to)
            where TFrom : unmanaged, Enum, IComparable<TTo>, IEquatable<TTo>, IComparable, IFormattable, IConvertible
            where TTo : unmanaged, IComparable<TTo>, IEquatable<TTo>, IComparable, IFormattable, IConvertible
            => to = *(TTo*)&from;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ToEnum<TFrom, TTo>(this TFrom from, out TTo to)
            where TFrom : unmanaged, IComparable<TFrom>, IEquatable<TFrom>, IComparable, IFormattable, IConvertible
            where TTo : unmanaged, Enum, IComparable<TFrom>, IEquatable<TFrom>, IComparable, IFormattable, IConvertible
            => to = *(TTo*)&from;
    }

    public interface ICatalogHandle
    {
        event Action onCatalogReset;
        event Action onPreInit;
        event Action onPostInit;

        void InitializeIfNeeded();
        void EnsureInitialized();
    }
    internal struct CatalogHandle<TCatalog, TDef, TIndex> : ICatalogHandle
        where TCatalog : CustomCatalog<TCatalog, TDef, TIndex>, new()
        where TDef : Def<TIndex>
        where TIndex : unmanaged, Enum, IComparable<UInt64>, IEquatable<UInt64>, IComparable, IFormattable, IConvertible
    {
        public void InitializeIfNeeded() => CustomCatalog<TCatalog, TDef, TIndex>.InitializeIfNeeded();
        public void EnsureInitialized() => CustomCatalog<TCatalog, TDef, TIndex>.EnsureInitialized();

        public event Action onCatalogReset
        {
            add => CustomCatalog<TCatalog, TDef, TIndex>.onCatalogReset += value;
            remove => CustomCatalog<TCatalog, TDef, TIndex>.onCatalogReset -= value;
        }
        public event Action onPreInit
        {
            add => CustomCatalog<TCatalog, TDef, TIndex>.onPreInit += value;
            remove => CustomCatalog<TCatalog, TDef, TIndex>.onPreInit -= value;
        }
        public event Action onPostInit
        {
            add => CustomCatalog<TCatalog, TDef, TIndex>.onPostInit += value;
            remove => CustomCatalog<TCatalog, TDef, TIndex>.onPostInit -= value;
        }
    }
}
namespace ex
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using ReinCore;

    using RoR2;

    public abstract class CustomCatalog<TSelf, TDef>
        where TSelf : CustomCatalog<TSelf, TDef>, new()
        where TDef : CustomCatalog<TSelf, TDef>.ICatalogDef
        //where TDef : class, CustomCatalog<TSelf, TDef>.IDef
    {
        #region SubTypes
        public interface ICatalogDef
        {
            String guid { get; }
            Entry? entry { get; set; }
        }
        public class Entry
        {
            public Index? index { get; internal set; }
            public TDef def { get; }
            public Boolean active => this.index is not null;

            internal Entry(TDef def)
            {
                this.def = def;
            }
        }

        private struct Handle : ICatalogHandle
        {
            public void InitializeIfNeeded() => CustomCatalog<TSelf, TDef>.InitializeIfNeeded();
            public void EnsureInitialized() => CustomCatalog<TSelf, TDef>.EnsureInitialized();

            public event Action onCatalogReset
            {
                add => CustomCatalog<TSelf, TDef>.onCatalogReset += value;
                remove => CustomCatalog<TSelf, TDef>.onCatalogReset -= value;
            }
            public event Action onPreInit
            {
                add => CustomCatalog<TSelf, TDef>.onPreInit += value;
                remove => CustomCatalog<TSelf, TDef>.onPreInit -= value;
            }
            public event Action onPostInit
            {
                add => CustomCatalog<TSelf, TDef>.onPostInit += value;
                remove => CustomCatalog<TSelf, TDef>.onPostInit -= value;
            }
        }
        public sealed class RegistrationToken
        {
            public TDef def => this.entry.def;
            public Entry entry { get; }
            public Boolean registered { get; private set; }
            public Boolean added => this.index is not null;
            public Index? index => this.entry.index;
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
            internal RegistrationToken(TDef def) => this.entry = def.entry;
            private TDef RegistrationFunc() => this.def;
        }

        public enum Index : UInt64
        {
            Invalid = 0ul,
        }
        #endregion

        #region Customizable functionality
        protected virtual IEnumerable<ICatalogHandle> dependencies { get => Enumerable.Empty<ICatalogHandle>(); }
        protected virtual void OnDefRegistered(TDef def) { }
        protected virtual void ProcessAllDefinitions(TDef[] definitions) { }
        protected virtual IEnumerable<TDef> GetBaseEntries() => Enumerable.Empty<TDef>();
        protected virtual void FirstInitSetup() { }
        #endregion


        #region Inherited static interface
        public static ICatalogHandle handle { get; } = new Handle();
        public static TDef? GetDef(Index index)
        {
            EnsureInitialized();
            return definitions![(UInt64)index];
        }
        public static TDef? GetDef(String guid)
        {
            EnsureInitialized();
            return guidToDef[guid];
        }
        public static Boolean TryGetDef(Index index, out TDef? def)
        {
            def = default;
            if(!initialized) return false;
            var ind = (UInt64)index;
            if(ind >= count) return false;
            def = definitions![ind - 1];
            return true;
        }
        public static Boolean TryGetDef(String guid, out TDef? def)
        {
            def = default;
            if(!initialized) return false;
            return guidToDef.TryGetValue(guid, out def);
        }

        public static UInt64 count => (UInt64)(definitions?.LongLength ?? 0L);

        public static RegistrationToken Add(TDef item)
        {
            var tok = new RegistrationToken(item);
            tok.Register();
            outstandingTokens.Add(tok);
            return tok;
        }
        public static event Action onPreInit;
        public static event Action onPostInit;
        public static event Action onCatalogReset;
        #endregion

        #region Protected inherited static interface
        protected static readonly TSelf instance = new();
        #endregion


        #region Internal static interface
        private static readonly List<RegistrationToken> outstandingTokens = new();
        //Convert this to hashset? delegate does not really make sense anymore with this setup as it is abstracted behind Add and defs should not be recreated on reset.
        private static event Func<TDef> moddedEntries;
        private static TDef[]? definitions;
        private static readonly Dictionary<String, TDef> guidToDef = new();
        private static Boolean initialized = false;
        private static UInt64 _curIndex = 0ul;
        private static Boolean firstInitComplete = false;
        private static Index currentIndex
        {
            get
            {
                return (Index)_curIndex;
            }
            set => _curIndex = (UInt64)value;
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
                if(!firstInitComplete)
                {
                    dep.onCatalogReset += OnDepReset;
                    dep.onPostInit += InitializeIfNeeded;
                }
            }
            if(!firstInitComplete)
            {
                instance.FirstInitSetup();
                firstInitComplete = true;
            }
            


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

            if(definitions is not null)
            {
                foreach(var def in definitions)
                {
                    def.entry.index = null;
                }
            }
            definitions = null;
            guidToDef.Clear();

            InitEntries();
            instance.ProcessAllDefinitions(definitions);
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
                if(def?.entry?.index is not Index index)
                {
                    // Log potentially fatal error, this code should be entirely unreachable.
                    continue;
                }
                definitions[(UInt64)index] = def;
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
            var entry = def.entry ??= new(def);
            if(guidToDef.TryGetValue(def.guid, out var existing) && existing?.entry?.index is Index index)
            {
                existing.entry.index = null;
                entry.index = index;
                //Log existing overwritten info
            } else
            {
                entry.index = currentIndex;
                _curIndex++;
            }

            guidToDef[def.guid] = def;

            instance.OnDefRegistered(def);
        }
        #endregion

        #region Constructors
        protected CustomCatalog()
        {
            if(instance is not null) throw new InvalidOperationException("Instance already created");
        }


        #endregion


        public static void OnDepReset()
        {
            if(initialized)
            {
                initialized = false;
                foreach(var def in definitions!)
                {
                    def.entry.index = null;
                }
                definitions = null;
                guidToDef.Clear();
            }
        }

        



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

}
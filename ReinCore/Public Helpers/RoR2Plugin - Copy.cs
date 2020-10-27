//namespace ex
//{
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.ComponentModel;
//    using System.Linq;
//    using System.Runtime.CompilerServices;

//    public unsafe abstract class Catalog<TSelf, TDef, TIndex>
//        where TSelf : Catalog<TSelf, TDef, TIndex>, new()
//        where TDef : Def<TIndex>
//        where TIndex : unmanaged, Enum, IComparable<UInt64>, IEquatable<UInt64>, IComparable, IFormattable, IConvertible
//    {
//        //Various functions for looking up by index and name



//        public sealed class Token
//        {
//            public TDef def { get; }
//            public Boolean registered { get; private set; }
//            public Boolean added => this.index is not null;
//            public TIndex? index => this.def.index;
//            public void Register()
//            {
//                if(this.registered) return;
//                moddedEntries += this.RegistrationFunc;
//                this.registered = true;
//            }
//            public void Unregister()
//            {
//                if(!this.registered) return;
//                moddedEntries -= this.RegistrationFunc;
//                this.registered = false;
//            }
//            internal Token(TDef def) => this.def = def;
//            private TDef RegistrationFunc() => this.def;
//        }

//        public static Token Add(TDef item)
//        {
//            var tok = new Token(item);
//            tok.Register();
//            outstandingTokens.Add(tok);
//            return tok;
//        }

//        private static readonly List<Token> outstandingTokens = new();


//        public static event Action onPreInit;
//        public static event Action onPostInit;

//        private static event Func<TDef> moddedEntries;

//        private static TDef[] definitions;
//        private static readonly Dictionary<String, TDef> guidToDef = new();

//        private static Boolean initialized = false;


//        protected static readonly TSelf instance = new();

//        private static UInt64 _curIndex = 0ul;
//        private static TIndex currentIndex
//        {
//            get { fixed(UInt64* ptr = &_curIndex) return *(TIndex*)ptr; }
//            set => _curIndex = value.ToUInt64(null);
//        }
//        protected Catalog()
//        {
//            if(instance is not null) throw new InvalidOperationException("Instance already created");
//        }

//        internal static void EnsureInitialized()
//        {
//            if(!initialized)
//            {
//                InitCatalog();
//            }
//        }

//        private static void InitCatalog()
//        {
//            instance.EnsureDependenciesInitialized();


//            var preSubbed = onPreInit?.Subscribed();
//            if(preSubbed is not null)
//            {
//                foreach(var item in preSubbed)
//                {
//                    try
//                    {
//                        item.Invoke();
//                    } catch(Exception e)
//                    {
//                        //Log error
//                    }
//                }
//            }


//            foreach(var def in definitions)
//            {
//                def.active = false;
//                def.index = null;
//            }    
//            definitions = null; // Return to pool instead;
//            guidToDef.Clear();

//            InitEntries();
//            instance.ProcessAllDefinitions(definitions);

//            var postSubbed = onPostInit?.Subscribed();
//            if(postSubbed is not null)
//            {
//                foreach(var item in postSubbed)
//                {
//                    try
//                    {
//                        item.Invoke();
//                    } catch(Exception e)
//                    {
//                        //Log error
//                    }
//                }
//            }

//            initialized = true;
//        }

//        private static void InitEntries()
//        {
//            var baseEntries = instance.GetBaseEntries();

//            foreach(var entry in baseEntries)
//            {
//                Register(entry);
//            }

//            var moddedEntries = GetModdedEntries().OrderBy(def => def.guid);

//            foreach(var entry in moddedEntries)
//            {
//                Register(entry);
//            }

//            definitions = new TDef[_curIndex];
//            foreach(var def in guidToDef.Values)
//            {
//                if(def.index is not TIndex index)
//                {
//                    // Log potentially fatal error, this code should be entirely unreachable.
//                    continue;
//                }
//                definitions[index.ToUInt64(null)] = def;
//            }
//        }

//        private static IEnumerable<TDef> GetModdedEntries()
//        {
//            var result = Enumerable.Empty<TDef>();
//            var subs = moddedEntries?.Subscribed();
//            if(subs is null) return result;

//            foreach(var v in subs)
//            {
//                var item =
//                result = result.Append(v());
//            }
//            return result;
//        }

//        private static void Register(TDef def)
//        {
//            if(guidToDef.TryGetValue(def.guid, out var existing) && existing.index is TIndex index)
//            {
//                existing.active = false;
//                existing.index = null;
//                def.index = index;
//                //Log existing overwritten info
//            } else
//            {
//                def.index = currentIndex;
//                _curIndex++;
//            }

//            def.active = true;
//            guidToDef[def.guid] = def;

//            instance.RegisterDef(def);
//        }

//        protected abstract IEnumerable<TDef> GetBaseEntries();
//        protected abstract void EnsureDependenciesInitialized();
//        protected abstract void RegisterDef(TDef def);
//        protected abstract void ProcessAllDefinitions(TDef[] definitions);
//    }

//    public abstract class Def<TIndex>
//        where TIndex : unmanaged, Enum, IComparable<UInt64>, IEquatable<UInt64>, IComparable, IFormattable, IConvertible
//    {
//        public TIndex? index { get; internal set; }
//        public String guid { get; }
//        public Boolean active { get; internal set; }
//    }

//    internal static class MiscXtn
//    {
//        public static IEnumerable<TDelegate> Subscribed<TDelegate>(this TDelegate self)
//            where TDelegate : MulticastDelegate
//            => self.GetInvocationList().Cast<TDelegate>();
//    }
//}
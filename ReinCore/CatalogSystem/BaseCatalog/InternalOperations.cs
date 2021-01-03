namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using RoR2;


    public abstract partial class Catalog<TSelf, TDef, TBackend> : Catalog
        where TSelf : Catalog<TSelf, TDef, TBackend>, new()
        where TDef : Catalog<TSelf, TDef, TBackend>.ICatalogDef
		where TBackend : unmanaged, ICatalogBackend<TDef>
    {
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
                dep?.InitializeIfNeeded();
                if(!firstInitComplete && dep is not null)
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
                foreach(var def in definitions.Where(def => def is not null && def.entry is not null).ToArray())
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
                definitions[(UInt64)index-1] = def;
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
                entry.index = ++currentIndex;
            }

            guidToDef[def.guid] = def;

            instance.OnDefRegistered(def);
        }

        private static void OnDepReset()
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

        internal override void LogCatalog()
        {
            LogCatalogState();
        }
    }
}
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

    public static class SurvivorsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddEclipseUnlocks(String prefix, SurvivorDef def)
        {
            if(!UnlocksCore.loaded) throw new CoreNotLoadedException(nameof(UnlocksCore));
            if(!UnlocksCore.ableToAdd) throw new InvalidOperationException("Too late to add");
            if(def is null) throw new ArgumentNullException(nameof(def));
            emitEclipseFor.Add((prefix, def));
        }

        static SurvivorsCore()
        {
            //Log.Warning("SurvivorsCore loaded");
            HooksCore.RoR2.SurvivorCatalog.Init.On += Init_On;
            HooksCore.RoR2.UnlockableCatalog.Init.On += Init_On1;
            HooksCore.RoR2.EclipseRun.GetEclipseBaseUnlockableString.On += GetEclipseBaseUnlockableString_On;
            HooksCore.RoR2.UI.EclipseRunScreenController.SelectSurvivor.Il += SelectSurvivor_Il;
            //HooksCore.RoR2.EclipseRun.OnClientGameOver.Il += OnClientGameOver_Il;

            vanillaSurvivorsCount = SurvivorCatalog.idealSurvivorOrder.Length;
            vanillaSurvivorsCount2 = SurvivorCatalog.survivorMaxCount;
            //Log.Warning("SurvivorsCore loaded");
            loaded = true;
        }

        //private static void OnClientGameOver_Il(ILContext il) => new ILCursor(il)
        //    .GotoNext(MoveType.After, x => x.MatchStloc(1), x => x.MatchLdloc(1))
        //    .Dup_()
        //    .CallDel_<Action<UnlockableDef>>(LogDef);
        private static String GetEclipseText(String current, SurvivorIndex index) => identities.TryGetValue(index, out var id) ? $"Eclipse.{id}" : current;
        private static Int32 tempLocal = 0;
        private static void SelectSurvivor_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdstr("Eclipse.{0}")
            ).GotoNext(MoveType.AfterLabel, x => x.MatchStloc(0))
            .LdArg_(1)
            .CallDel_<Func<String, SurvivorIndex, String>>(GetEclipseText);



        private static String GetEclipseBaseUnlockableString_On(HooksCore.RoR2.EclipseRun.GetEclipseBaseUnlockableString.Orig orig)
        {
            var res = orig();
            //Log.Warning($"Input {res}");
            if(res == "") goto End;
            var spl = res.Split('.');
            if(spl.Length != 2) goto End;
            if(Enum.TryParse<SurvivorIndex>(spl[1], out var i))
            {
                if(identities.TryGetValue(i, out var identity))
                {
                    res = $"{spl[0]}.{identity}";
                    goto End;
                }
            }
        End:
            //Log.Warning($"Output {res}");
            return res;
        }
        private static readonly Dictionary<SurvivorIndex,String> identities = new Dictionary<SurvivorIndex, String>();

        private static String CreateOrGetIdentity(SurvivorIndex index, String pref, String type)
        {
            if(identities.TryGetValue(index, out var id)) return id;

            return identities[index] = $"{pref}_{type}";
        }

        private static void Init_On1(HooksCore.RoR2.UnlockableCatalog.Init.Orig orig)
        {
            var check = new HashSet<String>();
            foreach(var (prefix, survivor) in emitEclipseFor)
            {
                var identity = CreateOrGetIdentity(survivor.survivorIndex, prefix, survivor.name);
                for(Int32 i = 2; i <= 8; ++i)
                {
                    var name = $"Eclipse.{identity}.{i}";
                    //Log.Warning($"Eclipse id: {name}");
                    UnlocksCore.AddUnlockableOnly(name, new());
                    _ = check.Add(name);
                }
            }
            orig();


            foreach(var s in check)
            {
                if(UnlockableCatalog.GetUnlockableDef(s) is null)
                {
                    Log.Error($"{s} did not register properly");
                }
            }
        }



        private static readonly List<(String modPrefix, SurvivorDef survivor)> emitEclipseFor = new List<(String modPrefix, SurvivorDef survivor)>();
        //private static void EmitAndRegisterEclipseUnlockables()
        //{
        //    var module = ModuleDefinition.CreateModule("Rein_GeneratedEclipseUnlockables", new ModuleParameters
        //    {
        //        Kind = ModuleKind.Dll,
        //        ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault,
        //    });
        //    module.AddStandardAttributes();
        //    module.AddIgnoreAccessAttribute(typeof(SurvivorDef), typeof(SurvivorsCore));
        //    generateToModule = module;
        //    var allNames = emitEclipseFor.SelectMany(GenerateTypes).ToArray();
        //    //module.Write("TestOutput.dll");
        //    var asm = ReflectionHelper.Load(module);
        //    foreach(var type in allNames.Select(asm.GetType)) UnlocksCore.InternalAddUnlockable(type, EclipseHelpers.serverTracked);
        //}

        //private static String generateToNamespace { get => "Generated.EclipseUnlockables"; }
        //private static ModuleDefinition generateToModule;
        //private static readonly MethodInfo prereqGetter = typeof(Eclipse1Base).GetProperty(nameof(Eclipse1Base.prereq)).GetMethod;
        //private static readonly MethodInfo identityGetter = typeof(EclipseBase).GetProperty(nameof(EclipseBase.identity)).GetMethod;
        //private static unsafe String[] GenerateTypes((String modPrefix, SurvivorDef survivor) arg)
        //{
        //    var (modPrefix, survivorDef) = arg;
        //    static IEnumerable<Int32> GetRange(Int32 inclStart, Int32 inclEnd)
        //    {
        //        var res = Enumerable.Empty<Int32>();
        //        for(; inclStart <= inclEnd; ++inclStart) res = res.Append(inclStart);
        //        return res;
        //    }
        //    String TypeName(Int32 index) => survivorDef.name;

        //    static Type BaseType(Int32 index) => index switch
        //    {
        //        1 => typeof(Eclipse1Base),
        //        2 => typeof(Eclipse2Base),
        //        3 => typeof(Eclipse3Base),
        //        4 => typeof(Eclipse4Base),
        //        5 => typeof(Eclipse5Base),
        //        6 => typeof(Eclipse6Base),
        //        7 => typeof(Eclipse7Base),
        //        8 => typeof(Eclipse8Base),
        //        _ => throw new ArgumentOutOfRangeException(nameof(index)),
        //    };
            
        //    (Int32, String, Type, String, String, SurvivorIndex) SetupInfo(Int32 index)
        //    {
        //        return (index, TypeName(index), BaseType(index), survivorDef.unlockableName, modPrefix, survivorDef.survivorIndex);
        //    }

        //    static (Int32, String, Type, String, String, SurvivorIndex, TypeDefinition) EmitType((Int32, String, Type, String, String, SurvivorIndex) arg)
        //    {
        //        var (index, survName, baseType, prereq, prefix, survInd) = arg;

        //        return (index, survName, baseType, prereq, prefix, survInd, new TypeDefinition(generateToNamespace, $"{prefix}__{survName}__Eclipse{index}Unlockable", TypeAttributes.BeforeFieldInit | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, generateToModule.ImportReference(baseType)));
        //    }

        //    static TypeDefinition EmitMembers((Int32,String,Type,String,String,SurvivorIndex,TypeDefinition) arg)
        //    {
        //        var (index, typeName, baseType, prereqUnlockable, prefix, survInd, typeDef) = arg;
        //        generateToModule.Types.Add(typeDef);

        //        if(index == 1)
        //        {
        //            var prereqProperty = new PropertyDefinition("prereq", PropertyAttributes.None, generateToModule.TypeSystem.String)
        //            {
        //                HasThis = true
        //            };
        //            var prereqGet = new MethodDefinition("get_prereq", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.SpecialName, generateToModule.TypeSystem.String)
        //            {
        //                HasThis = true
        //            };
        //            //prereqGet.Overrides.Add(generateToModule.ImportReference(prereqGetter));
        //            var prereqProc = prereqGet.Body.GetILProcessor();
        //            prereqProc.Emit(OpCodes.Ldstr, prereqUnlockable);
        //            prereqProc.Emit(OpCodes.Ret);
        //            prereqProperty.GetMethod = prereqGet;
        //            typeDef.Methods.Add(prereqGet);
        //            typeDef.Properties.Add(prereqProperty);
        //        }

        //        var identityProperty = new PropertyDefinition("identity", PropertyAttributes.None, generateToModule.TypeSystem.String)
        //        {
        //            HasThis = true
        //        };
        //        var identityGet = new MethodDefinition("get_identity", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.SpecialName, generateToModule.TypeSystem.String)
        //        {
        //            HasThis = true,
        //        };
        //        //identityGet.Overrides.Add(generateToModule.ImportReference(identityGetter));
        //        var identityProc = identityGet.Body.GetILProcessor();
        //        identityProc.Emit(OpCodes.Ldstr, CreateOrGetIdentity(survInd, prefix, typeName));
        //        identityProc.Emit(OpCodes.Ret);
        //        identityProperty.GetMethod = identityGet;
        //        typeDef.Methods.Add(identityGet);
        //        typeDef.Properties.Add(identityProperty);

        //        var survivorProperty = new PropertyDefinition("targetSurvivor", PropertyAttributes.None, generateToModule.ImportReference(typeof(SurvivorIndex)))
        //        {
        //            HasThis = true
        //        };
        //        var survivorGet = new MethodDefinition("get_targetSurvivor", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.SpecialName, generateToModule.ImportReference(typeof(SurvivorIndex)))
        //        {
        //            HasThis = true,
        //        };
        //        //identityGet.Overrides.Add(generateToModule.ImportReference(identityGetter));
        //        var survivorProc = survivorGet.Body.GetILProcessor();
        //        var sIndex = SurvivorCatalog.FindSurvivorIndex(typeName);
        //        switch( (Int32)sIndex )
        //        {
        //            case 0:
        //                survivorProc.Emit(OpCodes.Ldc_I4_0);
        //                break;
        //            case 1:
        //                survivorProc.Emit(OpCodes.Ldc_I4_1);
        //                break;
        //            case 2:
        //                survivorProc.Emit(OpCodes.Ldc_I4_2);
        //                break;
        //            case 3:
        //                survivorProc.Emit(OpCodes.Ldc_I4_3);
        //                break;
        //            case 4:
        //                survivorProc.Emit(OpCodes.Ldc_I4_4);
        //                break;
        //            case 5:
        //                survivorProc.Emit(OpCodes.Ldc_I4_5);
        //                break;
        //            case 6:
        //                survivorProc.Emit(OpCodes.Ldc_I4_6);
        //                break;
        //            case 7:
        //                survivorProc.Emit(OpCodes.Ldc_I4_7);
        //                break;
        //            case 8:
        //                survivorProc.Emit(OpCodes.Ldc_I4_8);
        //                break;
        //            case Int32 i when i <= SByte.MaxValue || i >= SByte.MinValue:
        //                survivorProc.Emit(OpCodes.Ldc_I4_S, (SByte)i);
        //                break;
        //            default:
        //                survivorProc.Emit(OpCodes.Ldc_I4, (Int32)sIndex);
        //                break;
        //        }
        //        survivorProc.Emit(OpCodes.Ret);
        //        survivorProperty.GetMethod = survivorGet;
        //        typeDef.Methods.Add(survivorGet);
        //        typeDef.Properties.Add(survivorProperty);


        //        var baseCons = baseType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //        var baseConstr = baseCons[0];

        //        var constructor = new MethodDefinition(".ctor", MethodAttributes.HideBySig | MethodAttributes.ReuseSlot | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, generateToModule.TypeSystem.Void)
        //        {
        //            HasThis = true
        //        };
        //        var constructorProc = constructor.Body.GetILProcessor();
        //        constructorProc.Emit(OpCodes.Ldarg_0);
        //        constructorProc.Emit(OpCodes.Call, generateToModule.ImportReference(baseConstr));
        //        constructorProc.Emit(OpCodes.Ret);
        //        typeDef.Methods.Add(constructor);


        //        return typeDef;
        //    }

        //    static String GetTypeFullname(TypeDefinition type) => type.FullName;
        //    return GetRange(1,8).Select(SetupInfo).Select(EmitType).Select(EmitMembers).Select(GetTypeFullname).ToArray();
        //}
       


        private static readonly Int32 vanillaSurvivorsCount;
        private static readonly Int32 vanillaSurvivorsCount2;
        //private static readonly StaticAccessor<SurvivorDef[]> survivorDefs = new StaticAccessor<SurvivorDef[]>( typeof(SurvivorCatalog), "survivorDefs" );

        private static void Init_On( HooksCore.RoR2.SurvivorCatalog.Init.Orig orig )
        {
            orig();
            if( !loaded ) return;

            SurvivorDef[] defs = SurvivorCatalog.survivorDefs;

            if( vanillaSurvivorsCount <= defs.Length )
            {
                Int32 extraBoxesCount = vanillaSurvivorsCount2 - vanillaSurvivorsCount;
                Int32 startIndex = vanillaSurvivorsCount;
                Array.Resize<SurvivorIndex>( ref SurvivorCatalog.idealSurvivorOrder, defs.Length - 1 );
                for( Int32 i = startIndex; i < SurvivorCatalog.idealSurvivorOrder.Length; ++i )
                {
                    SurvivorCatalog.idealSurvivorOrder[i] = defs[i + 1].survivorIndex;
                }
                SurvivorCatalog.survivorMaxCount = SurvivorCatalog.idealSurvivorOrder.Length + extraBoxesCount;
            }

        }
    }
}

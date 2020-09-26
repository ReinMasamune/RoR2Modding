namespace ReinGeneralFixes
{
    using System;
    using System.Reflection;
    using BF = System.Reflection.BindingFlags;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;
    using RoR2.Skills;

    using UnityEngine;
    using System.Collections.Generic;
    using Mono.Cecil.Cil;
    using RoR2.Artifacts;
    using UnityEngine.Networking;

    internal partial class Main
    {
        private const Single eclipse1MonsterHealthBoost = 0.2f;
        private static String eclipse1Text => $"Monster Maximum Health: <style=cIsHealth>+{eclipse1MonsterHealthBoost * 100f}%</style>";
        private const String eclipse3Text = "Every 10 minutes <style=cIsHealth>Your doppelganger invades</style>";
        private const String eclipse8Text = "Enemies become <style=cIsHealth>Exponentially Stronger</style> over time";


        private static readonly MethodInfo runGetInstance = typeof(Run).GetProperty("instance", BF.Static | BF.NonPublic | BF.Public).GetGetMethod(true);
        private static readonly MethodInfo runGetSelectedDifficulty = typeof(Run).GetProperty("selectedDifficulty", BF.Instance | BF.NonPublic | BF.Public).GetGetMethod(true);

        partial void QoLEclipse()
        {
            Run.onRunStartGlobal += this.Run_onRunStartGlobal;
            Language.onCurrentLanguageChanged += EclipseText;
            this.Enable += Eclipse1Add;
            this.Disable += Eclipse1Remove;
            this.Enable += Eclipse3Add;
            this.Disable += Eclipse3Remove;
            this.Enable += Eclipse8Add;
            this.Disable += Eclipse8Remove;
            //this.FirstFrame += this.Main_FirstFrame;
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            if(NetworkServer.active)
            {
                _ = obj.AddComponent<EclipseDoppelgangerInvasionManager>();
            }
        }

        private static void EclipseText()
        {
            var cur = Language.currentLanguage;
            if(cur is null) return;
            for(Int32 i = 1; i <= 8; ++i)
            {
                var tok = $"ECLIPSE_{i}_DESCRIPTION";
                cur.stringsByToken[tok] = cur.stringsByToken[tok]
                    .Replace("Ally Starting Health: <style=cIsHealth>-50%</style>", eclipse1Text)
                    .Replace("Ally Fall Damage: <style=cIsHealth>+100% and lethal</style>", eclipse3Text)
                    .Replace("Allies recieve <style=cIsHealth>permanent damage</style>", eclipse8Text);
            }
        }

        private static void Eclipse8Remove()
        {

        }
        private static void Eclipse8Add()
        {
            HooksCore.RoR2.Run.RecalculateDifficultyCoefficentInternal.Il += RecalculateDifficultyCoefficentInternal_Il;
            HooksCore.RoR2.HealthComponent.TakeDamage.Il += TakeDamage_Il2;
        }

        private static void TakeDamage_Il2(ILContext il) => new ILCursor(il)
            .DefLabel(out var label)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchCallOrCallvirt(typeof(Run).GetProperty("instance").GetGetMethod()),
                x => x.MatchCallOrCallvirt(typeof(Run).GetProperty("selectedDifficulty").GetGetMethod()),
                x => x.MatchLdcI4(10),
                x => x.MatchBlt(out label)
            ).Br_(label)
            .RemoveRange(4);

        private static Single GetRunExponentialBase()
        {
            if(Run.instance.selectedDifficulty == DifficultyIndex.Eclipse8)/* is not EclipseRun run || EclipseRun.cvEclipseLevel.value < 8)*/ return 1.15f;
            const Single expBase = 1.16f;
            const Single expScale = 0.003f;
            const Single expStartMult = 0.975f;

            const Single effStart = expBase * expStartMult;
            const Single effScale = expBase * expScale;

            return effStart + (effScale * Run.instance.stageClearCount);
        }
        private static void RecalculateDifficultyCoefficentInternal_Il(ILContext il)
        {
            var cur = new ILCursor(il);
            while(cur.TryGotoNext(MoveType.AfterLabel, x => x.MatchLdcR4(1.15f)))
            {
                _ = cur
                    .Remove()
                    .CallDel_<Func<Single>>(GetRunExponentialBase);
            }
        }

        private static void Eclipse3Remove()
        {

        }
        private static void Eclipse3Add()
        {
            HooksCore.RoR2.GlobalEventManager.OnCharacterHitGround.Il += OnCharacterHitGround_Il;
            //HooksCore.RoR2.EclipseRun.OverrideRuleChoices.On += OverrideRuleChoices_On;
        }

        private static void OnCharacterHitGround_Il(ILContext il) => new ILCursor(il)
            .DefLabel(out var label)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchCallOrCallvirt(runGetInstance),
                x => x.MatchCallOrCallvirt(runGetSelectedDifficulty),
                x => x.MatchLdcI4(5),
                x => x.MatchBlt(out label)
            ).Br_(label);

        private static void Eclipse1Remove()
        {

        }
        private static void Eclipse1Add()
        {
            HooksCore.RoR2.CharacterBody.RecalculateStats.Il += RecalculateStats_Il;
            HooksCore.RoR2.CharacterMaster.OnBodyStart.Il += OnBodyStart_Il;
            // TODO: Edit text
        }

        private static void OnBodyStart_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.Before,
                x => x.MatchLdcR4(0.5f),
                x => x.MatchMul()
            ).Remove()
            .LdC_(1f);

        private static Single IncreaseHealthIfMonster(Single currentHealth, CharacterBody body)
        {
            return body.teamComponent.teamIndex == TeamIndex.Monster && Run.instance.selectedDifficulty >= DifficultyIndex.Eclipse1
                ? currentHealth * (1f + eclipse1MonsterHealthBoost)
                : currentHealth;
        }
        private static Int32 dopLoc = 0;
        private static Single CalcDoppelHealthMult(Single input) => Run.instance.selectedDifficulty >= DifficultyIndex.Eclipse3/* is EclipseRun && EclipseRun.cvEclipseLevel.value >= 3*/ ? input * 0.5f : input;
        private static Single CalcDoppelDmgMult(Single input) => Run.instance.selectedDifficulty >= DifficultyIndex.Eclipse3/* is EclipseRun && EclipseRun.cvEclipseLevel.value >= 3*/ ? input / 0.5f : input;
        private static void RecalculateStats_Il(MonoMod.Cil.ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.After,
                x => x.MatchLdarg(0),
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchLdcI4((Int32)ItemIndex.InvadingDoppelganger),
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchStloc(out dopLoc)
            ).GotoNext(MoveType.After,
                x => x.MatchLdloc(dopLoc),
                x => x.MatchLdcI4(0),
                x => x.MatchBle(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchLdcR4(out _)
            ).CallDel_<Func<Single, Single>>(CalcDoppelHealthMult)
            .GotoNext(MoveType.Before, x => x.MatchCallOrCallvirt(typeof(CharacterBody).GetProperty("maxHealth", BF.Public | BF.NonPublic | BF.Instance).GetSetMethod(true)))
            .LdArg_(0)
            .CallDel_<Func<Single, CharacterBody, Single>>(IncreaseHealthIfMonster)
            .GotoNext(MoveType.After,
                x => x.MatchLdloc(dopLoc),
                x => x.MatchLdcI4(0),
                x => x.MatchBle(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchLdcR4(out _)
            ).CallDel_<Func<Single, Single>>(CalcDoppelDmgMult);
    }


    internal class EclipseDoppelgangerInvasionManager : DoppelgangerInvasionManager
    {
        private static MasterCatalog.MasterIndex defaultMasterIndex;

        private Boolean isEnabled { get; set; }
        

        private new void Start()
        {
            defaultMasterIndex = MasterCatalog.FindAiMasterIndexForBody(BodyCatalog.FindBodyIndex("CommandoBody"));

            base.Start();
            this.isEnabled = base.run.selectedDifficulty >= DifficultyIndex.Eclipse3/* is EclipseRun && EclipseRun.cvEclipseLevel.value >= 3*/;
        }
        private new void OnEnable()
        {
            GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeathGlobal;
        }
        private new void OnDisable()
        {
            GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeathGlobal;
        }
        private new void FixedUpdate()
        {
            if(this.isEnabled)
            {
                Int32 currentInvasionCycle = this.GetCurrentInvasionCycle();
                if(this.previousInvasionCycle < currentInvasionCycle)
                {
                    this.previousInvasionCycle = currentInvasionCycle;
                    EclipseDoppelgangerInvasionManager.PerformEclipseInvasion(new Xoroshiro128Plus(this.seed + (UInt64)currentInvasionCycle));


                    this.run.targetMonsterLevel = 42f;
                }
            }
        }

        private new void OnCharacterDeathGlobal(DamageReport report)
        {
            if(this.isEnabled) base.OnCharacterDeathGlobal(report);
        }



        private static void PerformEclipseInvasion(Xoroshiro128Plus rng)
        {
            for(int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
            {
                CharacterMaster characterMaster = CharacterMaster.readOnlyInstancesList[i];
                if(characterMaster.teamIndex == TeamIndex.Player && characterMaster.playerCharacterMasterController)
                {
                    EclipseDoppelgangerInvasionManager.CreateDoppelganger(characterMaster, rng);
                }
            }
        }

        private static void CreateEclipseDoppelganger(CharacterMaster master, Xoroshiro128Plus rng)
        {
            var card = DoppelgangerSpawnCard.FromMaster(master);
            if(card is null) return;
            if(card.prefab is null)
            {
                card.prefab = MasterCatalog.GetMasterPrefab(defaultMasterIndex);
            }

            Transform spawnOnTarget;
            DirectorCore.MonsterSpawnDistance input;
            if(TeleporterInteraction.instance)
            { 
                spawnOnTarget = TeleporterInteraction.instance.transform;
                input = DirectorCore.MonsterSpawnDistance.Close;
            } else
            {
                spawnOnTarget = master.GetBody().coreTransform;
                input = DirectorCore.MonsterSpawnDistance.Far;
            }
            DirectorPlacementRule directorPlacementRule = new DirectorPlacementRule
            {
                spawnOnTarget = spawnOnTarget,
                placementMode = DirectorPlacementRule.PlacementMode.NearestNode
            };
            DirectorCore.GetMonsterSpawnDistance(input, out directorPlacementRule.minDistance, out directorPlacementRule.maxDistance);
            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(card, directorPlacementRule, rng);
            directorSpawnRequest.teamIndexOverride = new TeamIndex?(TeamIndex.Monster);
            directorSpawnRequest.ignoreTeamMemberLimit = true;

            CombatSquad squad = null;

            DirectorSpawnRequest directorSpawnRequest2 = directorSpawnRequest;
            directorSpawnRequest2.onSpawnedServer = DelegateHelper.Combine(directorSpawnRequest2.onSpawnedServer, (res) =>
            {
                if(squad is null)
                {
                    squad = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/Encounters/ShadowCloneEncounter")).GetComponent<CombatSquad>();
                }
                squad.AddMember(res.spawnedInstance.GetComponent<CharacterMaster>());
            });

            DirectorCore.instance.TrySpawnObject(directorSpawnRequest);

            if(squad is not null)
            {
                NetworkServer.Spawn(squad.gameObject);
            }
            UnityEngine.Object.Destroy(card);
        }
    }
}
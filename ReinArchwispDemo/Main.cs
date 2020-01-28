using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using R2API;
using GeneralPluginStuff;

namespace ReinArchWispDemo
{
    [R2APISubmoduleDependency(
        nameof( DirectorAPI ),
        nameof( R2API.AssetPlus.AssetPlus )
        )]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Rein.ReinDirectorCardDemoArchWisp", "ReinArchWisps", "2.0.1")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            var archWispSpawnCard = Resources.Load<CharacterSpawnCard>( "SpawnCards/CharacterSpawnCards/cscArchWisp");
            archWispSpawnCard.directorCreditCost = 300;

            var archWispDirCard = new DirectorCard();
            archWispDirCard.allowAmbushSpawn = true;
            archWispDirCard.forbiddenUnlockable = "";
            archWispDirCard.minimumStageCompletions = 4;
            archWispDirCard.preventOverhead = false;
            archWispDirCard.requiredUnlockable = "";
            archWispDirCard.selectionWeight = 1;
            archWispDirCard.spawnCard = archWispSpawnCard;
            archWispDirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            var archWispCard = new DirectorAPI.DirectorCardHolder();
            archWispCard.SetCard(archWispDirCard );
            archWispCard.SetInteractableCategory( DirectorAPI.InteractableCategory.None );
            archWispCard.SetMonsterCategory( DirectorAPI.MonsterCategory.Minibosses );

            DirectorAPI.MonsterActions += ( list, stage ) =>
            {
                foreach( DirectorAPI.DirectorCardHolder card in list )
                {
                    var csc = card.GetCard().spawnCard as CharacterSpawnCard;
                    if( csc.noElites && csc.name == "cscelectricworm" )
                    {
                        csc.noElites = false;
                    }
                }

                if( !list.Contains( archWispCard ) )
                {
                    list.Add( archWispCard );
                }
            };

            R2API.AssetPlus.Languages.AddToken( "ARCHWISP_BODY_NAME", "Archaic Wisp" );
        }
    }
}
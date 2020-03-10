#if SOUNDPLAYER

using System;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using RoR2;
using System.Collections.Generic;

namespace RogueWispPlugin
{
    partial class Main
    {
        partial void SetupSoundPlayer()
        {
            this.Frame += this.Main_Frame1;
            this.GUI += this.Main_GUI;

            //var pattern = new Regex( @"^\s*\d*\s*(?<SoundName>[[:upper:]](?:_|\w)*)" );
            var pattern = new Regex( @"\d+\s*(?<SoundName>[A-Z](_|\w)*)", RegexOptions.Compiled | RegexOptions.ExplicitCapture );
            var path = Application.dataPath + "\\StreamingAssets\\Audio\\GeneratedSoundBanks\\Windows\\Global.txt";
            var file = File.ReadAllLines(path);
            var list = new List<String>();
            Int32 failCounter = 0;
            
            foreach( var line in file )
            {
                //Main.LogI( "============================================" );
                //Main.LogI( "Matching: " + line );

                var match = pattern.Match( line );
                if( match.Success )
                {
                    //Main.LogI( "Match succeeded" );
                    var group = match.Groups["SoundName"];
                    //Main.LogI( "Value: " + group.Value );


                    list.Add( group.Value );
                } else
                {
                    //Main.LogI( "Match failed" );
                    if( failCounter++ > 10 )
                    {
                        //Main.LogI( "Breaking due to failures" );
                        break;
                    }
                }
            }

            //this.possibleSounds = list.ToArray();
            this.possibleSounds = new[]
            {
                "Play_healing_drone_heal_loop",
                "Play_imp_overlord_attack1_throw",
                "Play_lemurianBruiser_m1_fly_loop",
                "Play_gravekeeper_attack1_fire",
                "Play_huntress_R_aim_loop",
                "Play_gravekeeper_attack2_charge",
                "Play_greater_wisp_attack",
                "Play_gravekeeper_attack2_shoot",
                "Play_gravekeeper_attack1_open",
            };
















            //this.possibleSounds = new String[matches.Count];
            //matches.CopyTo( this.possibleSounds, 0 );

            //var matches = pattern.Matches(file,0);
            //var temp = new List<String>(matches.Count);
            //foreach( Match m in matches )
            //{
            //    var s = m.Groups[0].Captures[0].Value;
            //    temp.Add( s );
            //    Main.LogI( s );
            //}
            //this.possibleSounds = temp.ToArray();
        }

        private Boolean soundDisplayActive = false;
        private GameObject playFrom;
        private void Main_Frame1()
        {
            if( Input.GetKeyDown( KeyCode.F6 ) )
            {
                this.soundDisplayActive = !this.soundDisplayActive;
                if( this.soundDisplayActive )
                {
                    this.playFrom = Camera.main.gameObject;
                }
            }
        }

        private String[] possibleSounds;


        private void Main_GUI()
        {
            if( this.soundDisplayActive )
            {
                GUILayout.Window( 10, new Rect( 400f, 0f, 1300f, 1050f ), this.SoundWindow, "Sounds" );
            }
        }

        private Vector2 scroll1 = Vector2.zero;
        private Vector2 scroll2 = Vector2.zero;
        private Int32 selectedSoundID = 0;
        private List<UInt32> activeSounds = new List<UInt32>();
        private Single scaleNumber = 1f;
        private Single multM1CountdownNum = 1f;
        private Single casinoChestSwapSpeedNum = 1f;
        private Single itemLaserTurbineChargeNum = 1f;
        private Single stickyBombCountdown = 1f;
        private Single itemArmorReductionHitCount = 1f;
        private Single droneSpeed = 1f;
        private Single loaderShiftChargeAmount = 1f;
        private Single explosionSize = 1f;
        private Single azimuth = 1f;
        private Single fallIntensity = 1f;
        private Single elevation = 1f;
        private Single loaderM2GrappleRemain = 1f;
        private Single attackSpeed = 1f;
        private Single engiM1ChargePercent = 1f;
        private Single characterSize = 1f;
        private Single eliteEnemy = 1f;
        private Single cooldownRefreshPitch = 1f;
        private Single droneSize = 1f;
        private Single charMultSpeed = 1f;
        private Single audioTeam = 1f;
        private Single commandoM2Countdown = 1f;
        private Single goldGatSpeed = 1f;
        private Single damageDirection = 1f;
        private Single intensityWeather = 1f;
        private Single podDistanceToGround = 1f;
        private void PlaySoundRTPCMenu( ref Single value, String rtpcName, Single min, Single max, Single standard )
        {
            GUILayout.BeginHorizontal();
            {
                value = GUILayout.HorizontalSlider( value, min, max );

                if( GUILayout.Button( "R", GUILayout.Width( 26f ) ) )
                {
                    value = standard;
                }
            }
            GUILayout.EndHorizontal();

            if( GUILayout.Button( "Play" + rtpcName ) )
            {
                this.activeSounds.Add( Util.PlaySound( this.possibleSounds[this.selectedSoundID], this.playFrom, rtpcName, value ) );
            }
        }

        private void PlaySoundScaled( ref Single value, Single min, Single max, Single standard )
        {
            GUILayout.BeginHorizontal();
            {
                value = GUILayout.HorizontalSlider( value, min, max );

                if( GUILayout.Button( "R", GUILayout.Width( 30f ) ) )
                {
                    value = standard;
                }
            }
            GUILayout.EndHorizontal();

            if( GUILayout.Button( "PlayScaled" ) )
            {
                this.activeSounds.Add( Util.PlayScaledSound( this.possibleSounds[this.selectedSoundID], this.playFrom, value ) );
            }
        }


        private void SoundWindow( Int32 id )
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    this.scroll1 = GUILayout.BeginScrollView( this.scroll1 );
                    {
                        this.selectedSoundID = GUILayout.SelectionGrid( this.selectedSoundID, this.possibleSounds, 1 );
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if( GUILayout.Button( "Play" ) )
                    {
                        this.activeSounds.Add( Util.PlaySound( this.possibleSounds[this.selectedSoundID], this.playFrom ) );
                    }

                    this.PlaySoundScaled( ref this.scaleNumber, 0f, 10f, 1f );

                    this.scroll2 = GUILayout.BeginScrollView( this.scroll2 );
                    {
                        //this.PlaySoundRTPCMenu( ref this.multM1CountdownNum, "multM1_countdown", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.casinoChestSwapSpeedNum, "casinoChest_swapSpeed", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.itemLaserTurbineChargeNum, "item_laserTurbine_charge", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.stickyBombCountdown, "stickyBomb_countdown", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.itemArmorReductionHitCount, "item_armorReduction_hitCount", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.droneSpeed, "droneSpeed", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.loaderShiftChargeAmount, "loaderShift_chargeAmount", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.explosionSize, "explosionSize", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.azimuth, "azimuth", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.fallIntensity, "fallIntensity", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.elevation, "elevation", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.loaderM2GrappleRemain, "loaderM2_grappleRemain", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.attackSpeed, "attackSpeed", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.engiM1ChargePercent, "engiM1_chargePercent", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.characterSize, "characterSize", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.eliteEnemy, "eliteEnemy", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.cooldownRefreshPitch, "cooldownRefresh_pitch", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.droneSize, "droneSize", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.charMultSpeed, "charMultSpeed", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.audioTeam, "audioTeam", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.commandoM2Countdown, "commandoM2_countdown", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.goldGatSpeed, "goldgat_speed", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.damageDirection, "damageDirection", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.intensityWeather, "intensityWeather", 0f, 10f, 0f );
                        //this.PlaySoundRTPCMenu( ref this.podDistanceToGround, "podDistanceToGround", 0f, 10f, 0f );
                    }
                    GUILayout.EndScrollView();



                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical();
                {
                    if( GUILayout.Button( "Stop All" ) )
                    {
                        foreach( var soundID in this.activeSounds )
                        {
                            AkSoundEngine.StopPlayingID( soundID );
                        }
                        this.activeSounds.Clear();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif
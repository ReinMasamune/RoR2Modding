using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ImprovedVisions
{
    public partial class Main
    {
        partial void Hook()
        {
            this.Enable += this.AddHooks;
            this.Disable += this.RemoveHooks;
        }

        private void RemoveHooks()
        {

        }



        private void AddHooks()
        {
            var allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
            this.charBody = typeof( EntityState ).GetProperty( "characterBody", allFlags );
            this.duration = typeof( EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle ).GetField( "duration", allFlags );
            this.attackSpeed = typeof( EntityStates.BaseState ).GetField( "attackSpeedStat", allFlags );
            this.crossfade1 = typeof( EntityStates.EntityState ).GetMethod( "PlayCrossfade", allFlags, default(Binder), new Type[5]
            {
                typeof( String),
                typeof( String ),
                typeof( String ),
                typeof( Single ),
                typeof( Single )
            }, Array.Empty<ParameterModifier>() );
            this.animation1 = typeof( EntityStates.EntityState ).GetMethod( "PlayAnimation", allFlags, default(Binder), new Type[4]
            {
                typeof( String ),
                typeof( String ),
                typeof( String ),
                typeof( Single )
            }, Array.Empty<ParameterModifier>() );
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += this.FireLunarNeedle_OnEnter;
        }

        private PropertyInfo charBody;
        private FieldInfo duration;
        private FieldInfo attackSpeed;
        private MethodInfo crossfade1;
        private MethodInfo animation1;

        private Dictionary<CharacterBody,Boolean> commandoState = new Dictionary<CharacterBody, Boolean>();
        
        private void FireLunarNeedle_OnEnter( On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle self )
        {
            orig( self );

            var baseSelf = (EntityState)self;
            var baseSelf2 = (BaseState)self;
            var characterBody = (CharacterBody)this.charBody.GetValue( baseSelf );
            var dur = (Single)this.duration.GetValue( self );
            var attackSpeed = (Single)this.attackSpeed.GetValue( baseSelf2 );

            object[] args;

            switch( characterBody.baseNameToken )
            {
                default:
                    Debug.Log( "Unhandled Body: " + characterBody.baseNameToken );
                    break;
                case "COMMANDO_BODY_NAME":
                    if( !commandoState.ContainsKey( characterBody ) ) this.commandoState[characterBody] = false;
                    if( this.commandoState[characterBody] )
                    {
                        this.commandoState[characterBody] = false;
                        break;
                    }
                    args = new object[4]
                    {
                        "Gesture",
                        "FirePistolJoint",
                        "FirePistol.playbackRate",
                        dur
                    };
                    this.animation1.Invoke( baseSelf, args );
                    break;
                case "HUNTRESS_BODY_NAME":
                    args = new object[5]
                    {
                        "Gesture, Override",
                        "FireSeekingShot",
                        "FireSeekingShot.playbackRate",
                        dur,
                        dur * 0.2f / attackSpeed
                    };
                    this.crossfade1.Invoke( baseSelf, args  );
                    args[0] = (object)"Gesture, Additive";
                    this.crossfade1.Invoke( baseSelf, args );
                    break;
                case "TOOLBOT_BODY_NAME":

                    break;
                case "ENGI_BODY_NAME":

                    break;
                case "MAGE_BODY_NAME":

                    break;
                case "MERC_BODY_NAME":

                    break;
                case "TREEBOT_BODY_NAME":

                    break;
                case "LOADER_BODY_NAME":

                    break;
                case "CROCO_BODY_NAME":

                    break;
                case "Bandit":

                    break;
                case "RogueWisp":

                    break;
            }
        }
    }
}

#if DPSMETER
using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;

namespace ReinGeneralFixes
{

    internal partial class Main
    {
        partial void SetupDPSMeter()
        {
            this.Load += this.DPSMerterInit;
            this.GUI += this.DPSMeterDraw;
            this.Enable += this.AddDPSMeterHooks;
            this.Disable += this.RemoveDPSMeterHooks;
        }

        private void RemoveDPSMeterHooks()
        {
            RoR2.GlobalEventManager.onServerDamageDealt -= this.GlobalEventManager_onServerDamageDealt;
            RoR2.Run.OnServerGameOver -= this.Run_OnServerGameOver;
        }



        private void AddDPSMeterHooks()
        {
            RoR2.GlobalEventManager.onServerDamageDealt += this.GlobalEventManager_onServerDamageDealt;
            RoR2.Run.OnServerGameOver += this.Run_OnServerGameOver;
        }

        private void Run_OnServerGameOver( Run arg1, GameResultType arg2 )
        {
            var t = Time.time;
            foreach( KeyValuePair<Int32,List<Int32>> kv in this.bodyHistory )
            {
                base.Logger.LogWarning( kv.Key.ToString() );
                foreach( QueryInfo q in this.queries )
                {
                    var context = new QueryContext
                    {
                        curTime = t,
                        maxTime = t,
                        minTime = t,
                        runningTotal = 0f
                    };

                    foreach( Int32 body in kv.Value )
                    {
                        if( this.damageStamps.ContainsKey( body ) )
                        {
                            foreach( DamageStamp stamp in this.damageStamps[body] )
                            {
                                if( !q.condition( context, stamp ) ) continue;
                                context = q.addDamageEnd( context, stamp );
                            }
                        }

                    }

                    base.Logger.LogInfo( q.prefix + q.calcDamageEnd( context ) );
                }
            }
        }

        private void GlobalEventManager_onServerDamageDealt( DamageReport obj )
        {
            if( !this.damageStamps.ContainsKey(this.SafeInstID(obj.attackerBody) ) )
            {
                this.damageStamps[this.SafeInstID(obj.attackerBody)] = new List<DamageStamp>();
            }

            this.damageStamps[this.SafeInstID(obj.attackerBody)].Add( new DamageStamp( obj ) );
        }

        internal class DamageStamp
        {
            internal Single timestamp;
            internal Single baseDamage;
            internal DotController.DotIndex dotType;

            internal Single damage;
            internal Single nonOverkillDamage;
            internal Single damageFrac;
            internal Single nonOverkillDamageFrac;
            internal DamageStamp( DamageReport report )
            {
                this.timestamp = Time.time;
                this.baseDamage = report.attackerBody.damage;
                this.dotType = report.damageInfo.dotIndex;

                this.damage = report.damageDealt;
                this.nonOverkillDamage = Mathf.Min( this.damage, report.victimBody.healthComponent.combinedHealth + report.victimBody.healthComponent.barrier );

                this.damageFrac = this.damage / this.baseDamage;
                this.nonOverkillDamageFrac = this.nonOverkillDamage / this.baseDamage;
            }
        }


        private Dictionary<Int32,List<DamageStamp>> damageStamps = new Dictionary<Int32, List<DamageStamp>>();
        private Dictionary<Int32,List<Int32>> bodyHistory = new Dictionary<Int32, List<Int32>>();

        private void DPSMeterDraw()
        {
            CharacterBody body = null;
            foreach( PlayerCharacterMasterController player in PlayerCharacterMasterController.instances )
            {
                if( player.localPlayerAuthority )
                {
                    body = player.master.GetBody();
                    if( !this.bodyHistory.ContainsKey(this.SafeInstID(player)))
                    {
                        this.bodyHistory[this.SafeInstID( player )] = new List<Int32>();
                    }
                    if( !this.bodyHistory[this.SafeInstID(player)].Contains( this.SafeInstID(body)) )
                    {
                        this.bodyHistory[this.SafeInstID(player)].Add( this.SafeInstID(body) );
                    }
                }
            }
            if( body != null )
            {
                if( this.damageStamps.ContainsKey( this.SafeInstID(body) ) )
                {
                    for( Int32 i = 0; i < this.queries.Length; i++ )
                    {
                        UnityEngine.GUI.Label( new Rect( 50f, 50f + 50f * i, 300f, 50f ), this.queries[i].prefix + this.QueryDPS( this.damageStamps[this.SafeInstID(body)], this.queries[i] ) );
                    }
                }
            }

        }

        private Single QueryDPS( List<DamageStamp> stamps, QueryInfo query)
        {
            var t = Time.time;
            var context = new QueryContext
            {
                curTime = t,
                maxTime = t,
                minTime = t,
                runningTotal = 0f
            };

            foreach( DamageStamp stamp in stamps )
            {
                if( !query.condition( context, stamp ) )
                {
                    continue;
                }
                context = query.addDamageRuntime( context, stamp );
            }

            return query.calcDamageRuntime( context );
        }

        private QueryInfo[] queries;

        private void DPSMerterInit()
        {
            this.queries = new QueryInfo[]
            {
                new QueryInfo
                {
                    prefix = "Poson: ",
                    condition = (context, stamp) => stamp.dotType == DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.damage / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.damage;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                },
                new QueryInfo
                {
                    prefix = "Poson - non-overkill: ",
                    condition = (context, stamp) => stamp.dotType == DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.nonOverkillDamage / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.nonOverkillDamage;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                },
                new QueryInfo
                {
                    prefix = "Non-Poison: ",
                    condition = (context, stamp) => stamp.dotType != DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.damage / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.damage;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                },

                new QueryInfo
                {
                    prefix = "Poison frac: ",
                    condition = (context, stamp) => stamp.dotType != DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.damageFrac / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.damageFrac;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                },
                new QueryInfo
                {
                    prefix = "Poison nonoverkill frac: ",
                    condition = (context, stamp) => stamp.dotType == DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.nonOverkillDamageFrac / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.nonOverkillDamageFrac;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                },
                new QueryInfo
                {
                    prefix = "Non-Poison frac: ",
                    condition = (context, stamp) => stamp.dotType != DotController.DotIndex.Poison,
                    addDamageRuntime = (context, stamp ) =>
                    {
                        context.minTime = Mathf.Min( stamp.timestamp, context.minTime );
                        var delta = 1f + context.curTime - stamp.timestamp;
                        context.runningTotal += stamp.damageFrac / Mathf.Pow( delta, 2 );
                        return context;
                    },
                    addDamageEnd = (context, stamp) =>
                    {
                        context.runningTotal += stamp.damageFrac;
                        return context;
                    },
                    calcDamageRuntime = (context) =>
                    {
                        return context.runningTotal;
                    },
                    calcDamageEnd = (context) =>
                    {
                        return context.runningTotal;
                    }
                }
            };
        }

        internal struct QueryContext
        {
            public Single curTime;
            public Single minTime;
            public Single maxTime;
            public Single runningTotal;
        }

        internal class QueryInfo
        {
            public String prefix = "";
            public Func<QueryContext, DamageStamp, Boolean> condition;
            public Func<QueryContext, DamageStamp, QueryContext> addDamageRuntime;
            public Func<QueryContext, DamageStamp, QueryContext> addDamageEnd;
            public Func<QueryContext, Single> calcDamageRuntime;
            public Func<QueryContext, Single> calcDamageEnd;
        }

        private Int32 SafeInstID( UnityEngine.Object obj )
        {
            if( obj != null ) return obj.GetInstanceID(); else return 0;
        }
    }

}
#endif
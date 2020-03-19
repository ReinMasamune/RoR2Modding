#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal static class MenuStructure<TMenu>
    {
        private static List<PropertyWrapper<TMenu>> wrappers = new List<PropertyWrapper<TMenu>>();
        private static Dictionary<String, MenuSection<TMenu>> sections = new Dictionary<String, MenuSection<TMenu>>();
        private static SortedList<Int32, MenuSection<TMenu>> sortedSections = new SortedList<Int32, MenuSection<TMenu>>();
        private static Int32 defaultPosition = 65536;
        private static Vector2 scroll1;

        internal static void Draw( TMenu instance )
        {
            scroll1 = GUILayout.BeginScrollView( scroll1 );
            {
                foreach( var section in sections )
                {
                    section.Value.Draw( instance );
                }
                if( wrappers.Count > 0 )
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label( "No Section" );
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    foreach( var wrapper in wrappers )
                    {
                        wrapper?.Draw( instance );
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        static MenuStructure()
        {
            foreach( var property in typeof(TMenu).GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic ) )
            {
                var settings = property.GetCustomAttribute<MenuAttribute>();
                if( settings == null )
                {
                    Main.LogI( "Skipped property: " + property.Name );
                    continue;
                }

                var wrapper = PropertyWrapper<TMenu>.CreatePropertyWrapper( property, settings );
                if( wrapper != null )
                {
                    var sectionName = settings.sectionName;
                    if( String.IsNullOrEmpty( sectionName ) )
                    {
                        wrappers.Add( wrapper );
                    } else
                    {
                        if( !sections.ContainsKey( sectionName ) || sections[sectionName] == null ) sections[sectionName] = new MenuSection<TMenu>( sectionName );
                        sections[sectionName].AddNewEntry( wrapper, settings );
                    }
                }
            }

            foreach( var sectionKV in sections )
            {
                if( sectionKV.Value.order > 0 )
                {
                    sortedSections.Add( sectionKV.Value.order, sectionKV.Value );
                } else
                {
                    sortedSections.Add( defaultPosition++, sectionKV.Value );
                }
            }
        }

        private class MenuSection<TMenu>
        {
            internal MenuSection( String name )
            {
                this.name = name;
            }

            internal void AddNewEntry( PropertyWrapper<TMenu> wrapper, MenuAttribute settings )
            {
                if( settings.orderInSection > 0 )
                {
                    this.sectionMembers.Add( settings.orderInSection, wrapper );
                } else
                {
                    this.sectionMembers.Add( this.defaultPosition++, wrapper );
                }

                if( settings.sectionOrder != 0 )
                {
                    this.order = settings.sectionOrder;
                }
            }

            internal void Draw( TMenu instance )
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label( this.name );
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space( 5f );
                GUILayout.EndVertical();
                foreach( var kv in this.sectionMembers )
                {
                    kv.Value?.Draw( instance );
                }
                GUILayout.BeginVertical();
                GUILayout.Space( 10f );
                GUILayout.EndVertical();
            }

            internal Int32 defaultPosition = 65536;
            internal Int32 order;

            private String name;
            private SortedList<Int32,PropertyWrapper<TMenu>> sectionMembers = new SortedList<Int32,PropertyWrapper<TMenu>>();
        }
    }
}
#endif
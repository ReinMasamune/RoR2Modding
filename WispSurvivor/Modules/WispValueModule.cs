#define ALLOWVALUECHANGES
/*
using System;

namespace RogueWispPlugin
{
#if DEBUG
    internal partial class WispSurvivorMain
    {
        [DebugMenu("Window.Width", DebugMenu.MenuType.TextEntry)]
        private static Single windowWidth = 512f;
        [DebugMenu("Window.Height" , DebugMenu.MenuType.TextEntry)]
        private static Single windowHeight = 512f;

        private Boolean menuEnabled = false;
        private Rect windowRect;

        private Vector2 scroll;

        private WindowLayer mainLayer;

        private GUISkin mainSkin;
        private GUISkin tempSkin;

        private static Int32 indentSize = 12;
        private static Int32 itemOffset = 6;

        public void BuildMainLayer()
        {
            mainLayer = new WindowLayer
            {
                name = "Main",
                open = true,
                entries = new List<IWindowEntry>(),
                subLayers = new Dictionary<String, WindowLayer>()
            };

            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes();

            List<FieldBundle> fieldBundles = new List<FieldBundle>();
            List<PropertyBundle> propertyBundles = new List<PropertyBundle>();

            for( Int32 i = 0; i < types.Length; i++ )
            {
                var t = types[i];
                var fields = t.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );
                for( Int32 j = 0; j < fields.Length; j++ )
                {
                    var f = fields[j];
                    var atrib = System.Attribute.GetCustomAttribute( f , typeof(DebugMenu) ) as DebugMenu;
                    if( atrib != null )
                    {
                        fieldBundles.Add( new FieldBundle
                        {
                            field = f,
                            path = atrib.path,
                            type = t,
                            settings = atrib.settings,
                            defaultValue = f.GetValue( t )
                        } );
                    }
                }

                var properties = t.GetProperties( BindingFlags.Static & ( BindingFlags.Public | BindingFlags.NonPublic ) );

                for( Int32 j = 0; j < properties.Length; j++ )
                {
                    var p = properties[j];
                    var atrib = System.Attribute.GetCustomAttribute( p , typeof(DebugMenu) ) as DebugMenu;

                    if( atrib != null )
                    {
                        propertyBundles.Add( new PropertyBundle
                        {
                            property = p,
                            path = atrib.path,
                            type = t,
                            settings = atrib.settings,
                            defaultValue = p.GetValue( t )
                        } );
                    }
                }
            }

            Dictionary<String,WindowLayer> pathBuilder = new Dictionary<String, WindowLayer>();

            for( Int32 i = 0; i < fieldBundles.Count; i++ )
            {
                var bundle = fieldBundles[i];
                String[] path = Array.Empty<String>();
                String key = "";
                String name;
                if( bundle.path.Contains("." ) )
                {
                    path = bundle.path.Split( '.' );
                    name = path[path.Length - 1];
                    Array.Resize<String>( ref path, path.Length - 1 );
                    for( Int32 j = 0; j < path.Length; j++ )
                    {
                        key += path[j];
                        key += '.';
                    }
                } else
                {
                    name = bundle.path;
                }

                IWindowEntry entry;

                switch( bundle.settings.menuType )
                {
                    default:
                        entry = CreateFieldEntry( bundle.field.FieldType, bundle, name );
                        break;
                    case DebugMenu.MenuType.TextEntry:
                        entry = CreateFieldEntry( bundle.field.FieldType, bundle, name );
                        break;
                    case DebugMenu.MenuType.Slider:
                        entry = CreateSlider( bundle.field.FieldType, bundle, name );
                        break;
                }

                if( !pathBuilder.ContainsKey( key ) )
                {
                    pathBuilder[key] = new WindowLayer
                    {
                        name = path[path.Length - 1],
                        path = path,
                        open = false,
                        entries = new List<IWindowEntry>()
                    };
                }

                var layer = pathBuilder[key];
                layer.entries.Add( entry );
            }

            for( Int32 i = 0; i < propertyBundles.Count; i++ )
            {

            }

            foreach( var kv in pathBuilder )
            {
                var path = kv.Value.path;
                var currentLayer = mainLayer;

                var tempPath = path == null || path.Length == 0 ? new List<String>() : path.ToList();
                if( path != null )
                {
                    for( Int32 i = 0; i < path.Length - 1; i++ )
                    {
                        if( !currentLayer.subLayers.ContainsKey( path[i] ) )
                        {
                            currentLayer.subLayers[path[i]] = new WindowLayer
                            {
                                name = path[i],
                                open = false,
                                path = tempPath.GetRange( 0, i ).ToArray(),
                                entries = new List<IWindowEntry>(),
                                subLayers = new Dictionary<string, WindowLayer>()
                            };
                        }

                        currentLayer = currentLayer.subLayers[path[i]];
                    }
                }
                currentLayer.subLayers[kv.Value.name] = kv.Value;
            }
        }

        public void BuildSkin()
        {
            mainSkin = Instantiate<GUISkin>( GUI.skin );

            Texture2D windowBG = CreateSolidTex(new Color( 0.25f, 0.25f, 0.25f, 0.4f ) , 128, 128);
            Texture2D windowFoc = CreateSolidTex(new Color( 0.25f, 0.25f, 0.25f, 0.9f ) , 128, 128);

            //Texture2D folderBG;


            var windowStyle = mainSkin.window;
            windowStyle.clipping = TextClipping.Clip;
            windowStyle.stretchWidth = true;
            windowStyle.stretchHeight = true;
            windowStyle.wordWrap = false;
            windowStyle.imagePosition = ImagePosition.ImageOnly;
            windowStyle.padding = new RectOffset( 8, 8, 16, 8 );
            windowStyle.overflow = new RectOffset( 0, 0, 0, 0 );
            windowStyle.border = new RectOffset( 0, 0, 0, 0 );
            windowStyle.normal.background = windowBG; //Default unfocused
            windowStyle.onNormal.background = windowFoc;  //Selected
            //windowStyle.hover.background = windowBG; //Hover
            //windowStyle.onHover.background = windowFoc; //Selected + Hover
            //windowStyle.active.background = windowBG; //???
            //windowStyle.onActive.background = windowFoc; //???
            //windowStyle.onFocused.background = windowFoc; //???
            //windowStyle.focused.background = windowFoc; //???

            var folderStyle = new GUIStyle(mainSkin.toggle);
            folderStyle.clipping = TextClipping.Clip;
            folderStyle.stretchWidth = true;
            folderStyle.stretchHeight = true;
            folderStyle.wordWrap = false;
            folderStyle.imagePosition = ImagePosition.TextOnly;
            //folderStyle.padding = new RectOffset( 8, 8, 16, 8 );
            //folderStyle.overflow = new RectOffset( 0, 0, 0, 0 );
            //folderStyle.border = new RectOffset( 0, 0, 0, 0 );
            //folderStyle.normal.background = windowBG; //Default unfocused
            //folderStyle.normal.textColor = new Color( 0.8f, 0.8f, 0.8f, 1f );
            //folderStyle.onNormal.background = windowFoc;  //Selected
            //folderStyle.hover.background = windowBG; //Hover
            //folderStyle.onHover.background = windowFoc; //Selected + Hover
            //folderStyle.active.background = windowBG; //???
            //folderStyle.active.textColor = new Color( 1f, 1f, 1f, 1f );
            //folderStyle.onActive.background = windowFoc; //???
            //folderStyle.onFocused.background = windowFoc; //???
            //folderStyle.focused.background = windowFoc; //???

            mainSkin.customStyles = new GUIStyle[1]
            {
                folderStyle
            };

            var scrollStyle = mainSkin.verticalScrollbar;
        }

        public void KeyListen()
        {
            if( Input.GetKeyDown( KeyCode.F5 ) ) menuEnabled = !menuEnabled;
        }

        public void DrawUI()
        {
            if( !mainSkin ) BuildSkin();
            if( !menuEnabled )
            {
                if( GUI.skin == mainSkin )
                {
                    GUI.skin = tempSkin;
                }
                return;
            }

            if( GUI.skin != mainSkin )
            {
                tempSkin = GUI.skin;
                GUI.skin = mainSkin;
            }

            windowRect = GUILayout.Window( 0, new Rect( 20f, 20f, windowWidth, windowHeight), DebugWindow, "Debug Menu" );
        }

        public void DebugWindow( int id )
        {
            scroll = GUILayout.BeginScrollView( scroll, false, true, GUILayout.MinWidth(windowWidth) );
            DrawLayer( mainLayer );
            GUILayout.EndScrollView();
        }

        public void DrawLayer( WindowLayer layer, Int32 depth = 0 )
        {
            var offset = new RectOffset( depth * indentSize , 0, 0, 0);
            var folderStyle = new GUIStyle(mainSkin.customStyles[0]) ;
            folderStyle.padding = offset;

            for( Int32 i = 0; i < layer.entries.Count; i++ )
            {
                layer.entries[i].Draw(depth);
            }
            if( layer == null || layer.subLayers == null ) return;
            foreach( WindowLayer subLayer in layer.subLayers.Values )
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space( depth * indentSize );
                subLayer.open = GUILayout.Toggle( subLayer.open, subLayer.name, folderStyle );
                GUILayout.EndHorizontal();
                if( subLayer.open )
                {
                    DrawLayer( subLayer, depth + 1 );
                }
            }
        }

        public class WindowLayer
        {
            public String[] path;
            public String name;
            public Boolean open;
         
            public Dictionary<String, WindowLayer> subLayers;
            public List<IWindowEntry> entries;
        }

        public interface IWindowEntry
        {
            String name { get; }
            void Draw( Int32 depth );
        }

        public interface IBackingValue<T>
        {
            T val { get; set; }
            void Reset();
        }

        public struct TestBackingValue<T> : IBackingValue<T>
        {
            public T cachedValue;
            public T defaultValue;

            public T val
            {
                get
                {
                    return cachedValue;
                }
                set
                {
                    if( !cachedValue.Equals( value ) )
                    {
                        Debug.Log( "Value set" );
                        cachedValue = value;
                    }
                }
            }

            public void Reset()
            {
                val = defaultValue;
            }
        }

        public struct DebugStaticField<T> : IBackingValue<T>
        {
            public Type type;
            public FieldInfo field;
            public T cachedValue;
            public T defaultValue;

            public T val 
            {
                get
                {
                    return cachedValue;
                }
                set
                {
                    if( !cachedValue.Equals( value ) )
                    {
                        field.SetValue( type, value );
                        cachedValue = value;
                    }
                }
            }

            public void Reset()
            {
                val = defaultValue;
            }
        }

        public class Checkbox : IWindowEntry
        {
            public String name { get; set; }

            public IBackingValue<Boolean> backingValue;

            public void Draw( Int32 depth )
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space( depth * indentSize + itemOffset );
                backingValue.val = GUILayout.Toggle( backingValue.val, name );
                GUILayout.EndHorizontal();
            }
        }

        public class Slider<T> : IWindowEntry where T : IConvertible
        {
            public String name { get; set; }

            public IBackingValue<T> backingValue;
            public T minValue;
            public T maxValue;

            public Slider( String name, IBackingValue<T> backingValue, T minValue, T maxValue )
            {
                this.name = name ?? throw new ArgumentNullException( nameof( name ) );
                this.backingValue = backingValue ?? throw new ArgumentNullException( nameof( backingValue ) );
                this.minValue = minValue;
                this.maxValue = maxValue;
            }

            public void Draw( Int32 depth )
            {
                Single remainingWidth = windowWidth - (depth * indentSize + itemOffset);
                GUILayout.BeginHorizontal();
                GUILayout.Space( depth * indentSize + itemOffset );
                GUILayout.Label( name, GUILayout.MinWidth( remainingWidth * 0.15f ), GUILayout.MaxWidth( remainingWidth * 0.15f ) );
                GUILayout.FlexibleSpace();
                GUILayout.Label( minValue.ToString(), GUILayout.MinWidth( remainingWidth * 0.05f ), GUILayout.MaxWidth( remainingWidth * 0.05f ) );
                backingValue.val = (T)Convert.ChangeType(GUILayout.HorizontalSlider( Convert.ToSingle(this.backingValue.val), Convert.ToSingle(minValue), Convert.ToSingle(maxValue), GUILayout.MinWidth( remainingWidth * 0.3f ), GUILayout.MaxWidth( remainingWidth * 0.3f ) ),typeof(T));
                GUILayout.Label( maxValue.ToString(), GUILayout.MinWidth( remainingWidth * 0.05f ), GUILayout.MaxWidth( remainingWidth * 0.05f ) );
                GUILayout.Label( backingValue.val.ToString(), GUILayout.MinWidth( remainingWidth * 0.2f ), GUILayout.MaxWidth( remainingWidth * 0.2f ) );
                if( GUILayout.Button( "Reset", GUILayout.MinWidth( remainingWidth * 0.15f ), GUILayout.MaxWidth( remainingWidth * 0.15f ) ) )
                {
                    backingValue.Reset();
                }
                GUILayout.EndHorizontal();
            }
        }

        public class FieldEntry<T> : IWindowEntry where T : IFormattable
        {
            public String name { get; set; }

            public IBackingValue<T> backingValue;

            public String text;

            public FieldEntry( String name, IBackingValue<T> backingValue )
            {
                this.name = name ?? throw new ArgumentNullException( nameof( name ) );
                this.backingValue = backingValue ?? throw new ArgumentNullException( nameof( backingValue ) );
                this.text = backingValue.val.ToString();
            }

            public void Draw( Int32 depth )
            {
                Single remainingWidth = windowWidth - (depth * indentSize + itemOffset);
                GUILayout.BeginHorizontal();
                GUILayout.Space( depth * indentSize + itemOffset );
                GUILayout.Label( name, GUILayout.MinWidth( remainingWidth * 0.15f ), GUILayout.MaxWidth( remainingWidth * 0.15f ) );
                GUILayout.FlexibleSpace();
                text = GUILayout.TextField( text, GUILayout.MinWidth( remainingWidth * 0.2f ), GUILayout.MaxWidth( remainingWidth * 0.2f ) );
                GUILayout.Label( backingValue.val.ToString(), GUILayout.MinWidth( remainingWidth * 0.2f ), GUILayout.MaxWidth( remainingWidth * 0.2f ) );
                if( GUILayout.Button( "Set" , GUILayout.MinWidth( remainingWidth * 0.15f ), GUILayout.MaxWidth( remainingWidth * 0.15f ) ) )
                {
                    var v = (T)Convert.ChangeType( text, typeof( T ) );
                    if( v != null )
                    {
                        backingValue.val = v;
                    }
                    else
                    {
                        Debug.Log( "Parse Failed" );
                    }
                }
                if( GUILayout.Button( "Reset", GUILayout.MinWidth( remainingWidth * 0.15f ), GUILayout.MaxWidth( remainingWidth * 0.15f ) ) )
                {
                    backingValue.Reset();
                    text = backingValue.val.ToString();
                }
                GUILayout.EndHorizontal();
            }
        }

        
        private struct FieldBundle
        {
            public String path;
            public DebugMenu.MenuSettings settings;
            public Type type;
            public FieldInfo field;
            public System.Object defaultValue;
        }

        private struct PropertyBundle
        {
            public String path;
            public DebugMenu.MenuSettings settings;
            public Type type;
            public PropertyInfo property;
            public System.Object defaultValue;
        }

        private MethodInfo rCreateSlider;
        private static IWindowEntry InternalCreateSlider<T>( FieldBundle field, String name ) where T : IConvertible
        {
            return new Slider<T>( name, new DebugStaticField<T>
            {
                defaultValue = (T)field.defaultValue,
                type = field.type,
                cachedValue = (T)field.defaultValue,
                field = field.field
            }, (T)Convert.ChangeType(field.settings.sliderMin,typeof(T)), (T)Convert.ChangeType( field.settings.sliderMax, typeof( T ) ) );
        }

        private IWindowEntry CreateSlider( Type t, FieldBundle field, String name )
        {
            if( rCreateSlider == null )
            {
                rCreateSlider = this.GetType().GetMethod( "InternalCreateSlider", BindingFlags.NonPublic | BindingFlags.Static );
            }
            var genericTemp = rCreateSlider.MakeGenericMethod(t);

            return (IWindowEntry)genericTemp.Invoke( null, new System.Object[2] { field, name } );
        }


        private MethodInfo rCreateFieldEntry;
        private static IWindowEntry InternalCreateFieldEntry<T>( FieldBundle field, String name ) where T : IFormattable
        {
            return new FieldEntry<T>( name, new DebugStaticField<T>
            {
                defaultValue = (T)field.defaultValue,
                cachedValue = (T)field.defaultValue,
                type = field.type,
                field = field.field
            });
        }

        private IWindowEntry CreateFieldEntry( Type t, FieldBundle field, String name )
        {
            if( rCreateFieldEntry == null )
            {
                rCreateFieldEntry = this.GetType().GetMethod( "InternalCreateFieldEntry", BindingFlags.NonPublic | BindingFlags.Static );
            }
            var genericTemp = rCreateFieldEntry.MakeGenericMethod(t);

            return (IWindowEntry)genericTemp.Invoke( null, new System.Object[2] { field, name } );
        }

        private static Texture2D CreateSolidTex( Color baseColor, Int32 width, Int32 height)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            for( Int32 x = 0; x < width; x++ )
            {
                for( Int32 y = 0; y < height; y++ )
                {
                    tex.SetPixel( x, y, baseColor );
                }
            }
            tex.wrapMode = TextureWrapMode.Clamp;

            tex.Apply();
            return tex;
        }
    }
#endif

    [AttributeUsage( AttributeTargets.Field, AllowMultiple = true, Inherited = false )]
    public class DebugMenu : System.Attribute
    {
        public struct MenuSettings
        {
            public MenuType menuType;
            public Double sliderMin;
            public Double sliderMax;
        }

        public enum MenuType
        {
            TextEntry = 0,
            Slider = 1,
            Checkbox = 2,
        }

        public String path;
        public MenuSettings settings;

        public DebugMenu( String path, MenuType menuType, Double sliderMin = 0.0, Double sliderMax = 100.0 )
        {
            this.path = path;
            this.settings = new MenuSettings
            {
                menuType = menuType,
                sliderMin = sliderMin,
                sliderMax = sliderMax
            };
        }
    }
}
*/
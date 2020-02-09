#if MATEDITOR
using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MaterialEditor : MonoBehaviour
    {
        const Int32 windowID = 1;
        internal const Single width = 800f;
        internal const Single height = 1100f;

        const Int32 cursorWidth = 32;
        const Int32 cursorHeight = 32;

        private Material material;

        private String[] selectionNames;

        private StandardMaterial standardMat;
        private Menu<StandardMaterial> standardMaterialMenu;

        private CloudMaterial cloudMat;
        private Menu<CloudMaterial> cloudMaterialMenu;

        private TransformControls transformControls;
        private Menu<TransformControls> transformControlsMenu;

        private TransformControls camControls;
        private Menu<TransformControls> camControlMenu;

        private IntersectionCloudMaterial intersectionCloudMat;
        private Menu<IntersectionCloudMaterial> intersectionCloudMaterialMenu;


        internal static GUIStyle windowStyle;
        private static Texture2D bgtex;
        private Texture2D cursorTex;
        private void Awake()
        {
            this.selectionNames = new String[]
            {
                "Transform Control",
                "Standard Material",
                "Cloud Material",
                "Cloud Intersection Material"
            };


            

            // TODO: Setup generic materials preview

            if( !bgtex )
            {
                bgtex = new Texture2D( (Int32)width, (Int32)height );
                Int32 pix = (Int32)width * (Int32)height;
                var cols = new Color[pix];
                for( Int32 i = 0; i < pix; ++i )
                {
                    cols[i] = new Color( 0f, 0f, 0f, 0.75f );
                }
                bgtex.SetPixels( 0, 0, (Int32)width, (Int32)height, cols );
                bgtex.Apply();
            }
            


            this.cursorTex = new Texture2D( cursorWidth, cursorHeight );
            for( Int32 x = 0; x < cursorWidth; ++x )
            {
                Int32 colMax = x;
                for( Int32 y = 0; y < cursorHeight; ++y )
                {
                    this.cursorTex.SetPixel( x, y, (y > colMax ? Color.white : Color.clear) );
                }
            }
            this.cursorTex.Apply();
        }

        private void Start()
        {
            var model = base.GetComponentInChildren<CharacterModel>();
            var skins = model.GetComponent<ModelSkinController>();
            skins.ApplySkin( 0 );


            this.standardMat = new StandardMaterial(Main.armorMaterials[0]);
            this.cloudMat = new CloudMaterial( Main.fireMaterials[0][0] );
            this.transformControls = new TransformControls( base.transform );
            this.camControls = new TransformControls( base.transform.parent );

            this.transformControlsMenu = new Menu<TransformControls>( this.transformControls );
            this.standardMaterialMenu = new Menu<StandardMaterial>( this.standardMat );
            this.cloudMaterialMenu = new Menu<CloudMaterial>( this.cloudMat );
            this.camControlMenu = new Menu<TransformControls>( this.camControls );
        }

        private void OnGUI()
        {
            if( this.standardMat != null )
            {
                if( windowStyle == null )
                {
                    windowStyle = GUI.skin.window;
                    windowStyle.focused.background = bgtex;
                    windowStyle.active.background = bgtex;
                    windowStyle.hover.background = bgtex;
                    windowStyle.normal.background = bgtex;
                    windowStyle.onActive.background = bgtex;
                    windowStyle.onFocused.background = bgtex;
                    //this.windowStyle.onHover.background = this.bgtex;
                    windowStyle.onNormal.background = bgtex;
                }

                this.windowRect = GUILayout.Window( windowID, new Rect( Screen.width - width, 0f, width, height ), this.DrawWindow, "Material Editor", windowStyle );

                var cursor = Input.mousePosition;
                var cursorRect = new Rect( cursor.x, Screen.height - cursor.y, cursorWidth, cursorHeight );
                if( this.windowRect.Contains( cursorRect.position ) )
                {
                    //cursorRect.position -= this.windowRect.position;
                    GUI.DrawTexture( cursorRect, this.cursorTex );
                }
            }
        }

        private Rect windowRect;

        private void DrawWindow( Int32 id )
        {
            var tempSelection = GUILayout.Toolbar( this.windowSelection, this.selectionNames );
            if( GUI.changed && tempSelection != this.windowSelection )
            {
                this.windowSelection = tempSelection;
            }

            switch( this.windowSelection )
            {
                case 0:
                    this.transformControlsMenu?.Draw();
                    this.camControlMenu?.Draw();
                    break;
                case 1:
                    this.standardMaterialMenu?.Draw();
                    break;
                case 2:
                    this.cloudMaterialMenu?.Draw();
                    break;
                case 3:
                    this.intersectionCloudMaterialMenu?.Draw();
                    break;
                default:
                    break;
                    
            }
        }

        private Int32 windowSelection = 0;
    }
}
#endif
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

        private WispModelBitSkinController skinController;

        private TransformControls transformControls;
        private Menu<TransformControls> transformControlsMenu;

        private TransformControls camControls;
        private Menu<TransformControls> camControlMenu;

        private WispBitSkinMenuWrapper bitWrapper;
        private Menu<WispBitSkinMenuWrapper> bitWrapperMenu;

        private StandardMaterial armorMat1;
        private Menu<StandardMaterial> armorMat1Menu;

        private CloudMaterial armorMat2Main;
        private Menu<CloudMaterial>armorMat2MainMenu;

        private DistortionMaterial armorMat2Sec;
        private Menu<DistortionMaterial> armorMat2SecMenu;

        private CloudMaterial flameMaterial;
        private Menu<CloudMaterial> flameMaterialMenu;

        private Material[] prevFlameMats = Array.Empty<Material>();
        private Material[] prevArmMats = Array.Empty<Material>();


        internal static GUIStyle windowStyle;
        private static Texture2D bgtex;
        private Texture2D cursorTex;
        private void Awake()
        {
            this.selectionNames = new String[]
            {
                "Transform Control",
                "Bit Skin",
                "Flames Cloud",
                "Armor Standard",
                "Armor Cloud",
                "Armor Distortion",
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
            //var skins = model.GetComponent<ModelSkinController>();
            //skins.ApplySkin( 0 );

            this.skinController = model.GetComponent<WispModelBitSkinController>();

            this.transformControls = new TransformControls( base.transform );
            this.camControls = new TransformControls( base.transform.parent );
            this.bitWrapper = new WispBitSkinMenuWrapper( base.GetComponentInChildren<WispModelBitSkinController>() );

            this.transformControlsMenu = new Menu<TransformControls>( this.transformControls );
            this.camControlMenu = new Menu<TransformControls>( this.camControls );
            this.bitWrapperMenu = new Menu<WispBitSkinMenuWrapper>( this.bitWrapper );
        }

        private void FixedUpdate()
        {
            /*
            var tempFlames = this.skinController.activeFlameMaterial;
            if( this.prevFlameMats == null || tempFlames != this.prevFlameMats )
            {
                this.prevFlameMats = tempFlames;
                this.flameMaterial = new CloudMaterial( tempFlames[0] );
                this.flameMaterialMenu = new Menu<CloudMaterial>( this.flameMaterial );
            }
            var tempArms = this.skinController.activeArmorMaterial;
            if( this.prevArmMats == null || tempArms != this.prevArmMats )
            {
                this.prevArmMats = tempArms;
                if( tempArms.Length == 1 )
                {
                    this.armorMat1 = new StandardMaterial( tempArms[0] );
                    this.armorMat1Menu = new Menu<StandardMaterial>( this.armorMat1 );
                    this.armorMat2Main = null;
                    this.armorMat2MainMenu = null;
                    this.armorMat2Sec = null;
                    this.armorMat2SecMenu = null;
                } else
                {
                    this.armorMat1 = null;
                    this.armorMat1Menu = null;
                    this.armorMat2Main = new CloudMaterial( tempArms[0] );
                    this.armorMat2MainMenu = new Menu<CloudMaterial>( this.armorMat2Main );
                    this.armorMat2Sec = new DistortionMaterial( tempArms[1] );
                    this.armorMat2SecMenu = new Menu<DistortionMaterial>( this.armorMat2Sec );
                }
            }
            */
        }

        private void OnGUI()
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

        private Rect windowRect;

        private void DrawWindow( Int32 id )
        {
            var tempSelection = GUILayout.SelectionGrid( this.windowSelection, this.selectionNames, 4 );
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
                    this.bitWrapperMenu?.Draw();
                    break;
                case 2:
                    this.flameMaterialMenu?.Draw();
                    break;
                case 3:
                    this.armorMat1Menu?.Draw();
                    break;
                case 4:
                    this.armorMat2MainMenu?.Draw();
                    break;
                case 5:
                    this.armorMat2SecMenu?.Draw();
                    break;
                default:
                    break;
                    
            }
        }

        private Int32 windowSelection = 0;
    }
}
#endif
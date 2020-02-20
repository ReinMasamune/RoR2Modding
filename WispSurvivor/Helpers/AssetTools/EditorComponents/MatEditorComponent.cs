#if MATEDITOR
using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class MaterialEditor : MonoBehaviour
    {
        internal static MaterialEditor instance;

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

        private StandardMaterial armorMat;
        private Menu<StandardMaterial> armorMatMenu;

        private CloudMaterial flameMat;
        private Menu<CloudMaterial> flameMatMenu;

        //private OpaqueCloudMaterial flameMat;
        //private Menu<OpaqueCloudMaterial> flameMatMenu;

        private CloudMaterial tracerMat;
        private Menu<CloudMaterial> tracerMatMenu;

        private CloudMaterial pillarMat;
        private Menu<CloudMaterial> pillarMatMenu;

        private IntersectionCloudMaterial indicatorMat;
        private Menu<IntersectionCloudMaterial> indicatorMatMenu;

        private CloudMaterial explosionMat;
        private Menu<CloudMaterial> explosionMatMenu;

        private CloudMaterial beamMat;
        private Menu<CloudMaterial> beamMatMenu;


        internal static GUIStyle windowStyle;
        private static Texture2D bgtex;
        private Texture2D cursorTex;
        private void Awake()
        {
            if( instance != null )
            {
                Destroy( instance );
               
            }
            instance = this;
            this.enabled = false;

            this.selectionNames = new String[]
            {
                "Transform Control",
                "Bit Skin",
                "Armor",
                "Flame",
                "Tracer",
                "Pillar",
                "Indicator",
                "Explosion",
                "Beam",
            };
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
            if( this.armorMat == null || this.armorMat.material != this.skinController.activeArmorMaterial )
            {
                this.armorMat = new StandardMaterial( this.skinController.activeArmorMaterial );
                this.armorMatMenu = new Menu<StandardMaterial>( this.armorMat );
            }

            if( this.flameMat == null || this.flameMat.material != this.skinController.activeFlameMaterial )
            {
                this.flameMat = new CloudMaterial( this.skinController.activeFlameMaterial );
                this.flameMatMenu = new Menu<CloudMaterial>( this.flameMat );

                //this.flameMat = new OpaqueCloudMaterial( this.skinController.activeFlameMaterial );
                //this.flameMatMenu = new Menu<OpaqueCloudMaterial>( this.flameMat );
            }

            if( this.tracerMat == null || this.tracerMat.material != this.skinController.activeTracerMaterial )
            {
                this.tracerMat = new CloudMaterial( this.skinController.activeTracerMaterial );
                this.tracerMatMenu = new Menu<CloudMaterial>( this.tracerMat );
            }

            if( this.pillarMat == null || this.pillarMat.material != this.skinController.activeFlamePillarMaterial )
            {
                this.pillarMat = new CloudMaterial( this.skinController.activeFlamePillarMaterial );
                this.pillarMatMenu = new Menu<CloudMaterial>( this.pillarMat );
            }
            
            if( this.indicatorMat == null || this.indicatorMat.material != this.skinController.activeAreaIndicatorMaterial )
            {
                this.indicatorMat = new IntersectionCloudMaterial( this.skinController.activeAreaIndicatorMaterial );
                this.indicatorMatMenu = new Menu<IntersectionCloudMaterial>( this.indicatorMat );
            }

            if( this.explosionMat == null || this.explosionMat.material != this.skinController.activeExplosionMaterial )
            {
                this.explosionMat = new CloudMaterial( this.skinController.activeExplosionMaterial );
                this.explosionMatMenu = new Menu<CloudMaterial>( this.explosionMat );
            }

            if( this.beamMat == null || this.beamMat.material != this.skinController.activeBeamMaterial )
            {
                this.beamMat = new CloudMaterial( this.skinController.activeBeamMaterial );
                this.beamMatMenu = new Menu<CloudMaterial>( this.beamMat );
            }
            
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
                    this.armorMatMenu?.Draw();
                    break;
                case 3:
                    this.flameMatMenu?.Draw();
                    break;
                case 4:
                    this.tracerMatMenu?.Draw();
                    break;
                case 5:
                    this.pillarMatMenu?.Draw();
                    break;
                case 6:
                    this.indicatorMatMenu?.Draw();
                    break;
                case 7:
                    this.explosionMatMenu?.Draw();
                    break;
                case 8:
                    this.beamMatMenu?.Draw();
                    break;
                default:
                    break;
                    
            }
        }

        private Int32 windowSelection = 0;
    }
}
#endif